using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{
    private Movement movement;
    private CollisionSenses collisionSenses;

    protected Movement Movement => movement ??= core.GetCoreComponent<Movement>();
    private CollisionSenses CollisionSenses => collisionSenses ??= core.GetCoreComponent<CollisionSenses>();

    private Vector2 vector2Workspace;
    private Vector2 detectedPos;
    private Vector2 cornerPos;
    private Vector2 startPos;
    private Vector2 endPos;

    private bool isHanging;
    private bool isClimbing;
    private bool isTouchingCeiling; //Cast a ray from the ledge corner (not the player) to see if we need to stand or crouch after climbing a ledge. Different from the ceiling check function in the player

    private int xInput;
    private int yInput;
    private bool jumpInput;

    public PlayerLedgeClimbState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, PlayerData playerData) : base(entity, stateMachine, animBoolName, playerData)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Movement?.SetVelocityZero();
        player.transform.position = detectedPos; //Lock player in this position before ledge climbing. We actually just want to use this position to do calculations.
        cornerPos = DetermineLedgeCornerPosition();

        //                substract here             because if we are facing right, we want to shift the xOffset to the left (by subtracting it) to get the startPos and vice versa. Reversed for endPos.
        startPos.Set(cornerPos.x - (Movement.FacingDirection * playerData.startOffset.x), cornerPos.y - playerData.startOffset.y);
        endPos.Set(cornerPos.x + (Movement.FacingDirection * playerData.startOffset.x), cornerPos.y + playerData.endOffset.y);

        player.transform.position = startPos;
    }
    
    public override void Exit()
    {
        base.Exit();

        isHanging = false;

        if (isClimbing)
        {
            player.transform.position = endPos;
            isClimbing = false;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            if (isTouchingCeiling)
            {
                stateMachine.ChangeState(player.CrouchIdleState); //Automatically crouches after a ledge climb if there is not enough space to stand
            }
            else
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
        else
        {
            xInput = player.InputHandler.NormalizedInputX;
            yInput = player.InputHandler.NormalizedInputY;
            jumpInput = player.InputHandler.JumpInput;

            Movement?.SetVelocityZero();
            player.transform.position = startPos;

            if ((xInput == Movement?.FacingDirection || yInput == 1) && isHanging && !isClimbing) //Start climbing if player inputs towards the ledge
            {
                CheckForCeilingAboveLedge();
                isClimbing = true;
                player.anim.SetBool("climbLedge", true);
            }
            else if ((yInput == -1 || xInput == -Movement?.FacingDirection) && isHanging && !isClimbing) //Drop from ledge if player inputs down
            {
                stateMachine.ChangeState(player.InAirState);
            }
            else if (jumpInput && !isClimbing)
            {
                player.WallJumpState.DetermineWallJumpDirection(true);
                stateMachine.ChangeState(player.WallJumpState);
            }
        }
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        isHanging = true;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        player.anim.SetBool("climbLedge", false);
    }

    public void SetDetectedPosition(Vector2 pos) => detectedPos = pos;
                                                                                    
    private void CheckForCeilingAboveLedge()
    {
                                                            //Offset upwards        Offset into ledge
        isTouchingCeiling = Physics2D.Raycast(cornerPos + (Vector2.up * 0.015f) + (Vector2.right * 0.015f * Movement.FacingDirection), Vector2.up, playerData.standColliderHeight, CollisionSenses.WhatIsGround);
        player.anim.SetBool("isTouchingCeiling", isTouchingCeiling);
    }

    private Vector2 DetermineLedgeCornerPosition()
    {
        RaycastHit2D xHit = Physics2D.Raycast(CollisionSenses.WallCheck.position, Vector2.right * Movement.FacingDirection, CollisionSenses.WallCheckDistance, CollisionSenses.WhatIsGround);
        float xDist = xHit.distance; //Distance of the raycast hit position from the raycast origin used to determine the position of the ledge corner

        vector2Workspace.Set((xDist + 0.015f) * Movement.FacingDirection, 0f);
        RaycastHit2D yHit = Physics2D.Raycast(CollisionSenses.LedgeCheckHorizontal.position + (Vector3)vector2Workspace, Vector2.down, CollisionSenses.LedgeCheckHorizontal.position.y - CollisionSenses.WallCheck.position.y + 0.015f, CollisionSenses.WhatIsGround);
        //By offsetting our y raycast with the x distance, we ensure that we can fire a vertical raycast to hit the ledge corner and determine the y position of the ledge
        float yDist = yHit.distance;

        vector2Workspace.Set(CollisionSenses.WallCheck.position.x + (xDist * Movement.FacingDirection), CollisionSenses.LedgeCheckHorizontal.position.y - yDist); //Final determined corner position
        return vector2Workspace;
    }
}
