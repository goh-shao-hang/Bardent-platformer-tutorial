using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchingWallState : PlayerState
{
    private Movement movement;
    private CollisionSenses collisionSenses;

    protected Movement Movement => movement ??= core.GetCoreComponent<Movement>();
    protected CollisionSenses CollisionSenses => collisionSenses ??= core.GetCoreComponent<CollisionSenses>();

    protected bool isGrounded;
    protected bool isTouchingWall;
    protected bool isTouchingLedge;
    protected int xInput;
    protected int yInput;
    protected bool jumpInput;
    protected bool grabInput;

    public PlayerTouchingWallState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, PlayerData playerData) : base(entity, stateMachine, animBoolName, playerData)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if (collisionSenses)
        {
            isGrounded = CollisionSenses.Ground;
            isTouchingWall = CollisionSenses.WallFront;
            isTouchingLedge = CollisionSenses.LedgeHorizontal;
        }

        if (isTouchingWall && !isTouchingLedge)
        {
            Movement?.SetVelocityZero();
            player.LedgeClimbState.SetDetectedPosition(player.transform.position);
        }
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormalizedInputX;
        yInput = player.InputHandler.NormalizedInputY;
        grabInput = player.InputHandler.GrabInput;
        jumpInput = player.InputHandler.JumpInput;

        if (jumpInput) //We already know that we are touching a wall
        {
            player.WallJumpState.DetermineWallJumpDirection(isTouchingWall);
            stateMachine.ChangeState(player.WallJumpState);
        }
        else if (isGrounded && !grabInput)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        else if (!isTouchingWall || (xInput != Movement?.FacingDirection && !grabInput)) //player is in air when not touching wall, or is touching wall but not trying to grab it
        {
            stateMachine.ChangeState(player.InAirState);
        }
        else if (isTouchingWall && !isTouchingLedge)
        {
            stateMachine.ChangeState(player.LedgeClimbState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
