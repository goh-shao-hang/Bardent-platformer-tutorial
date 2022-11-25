using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState //Not a superstate but is not part of any superstate
{
    private Movement movement;
    private CollisionSenses collisionSenses;

    protected Movement Movement => movement ??= core.GetCoreComponent<Movement>();
    private CollisionSenses CollisionSenses => collisionSenses ??= core.GetCoreComponent<CollisionSenses>();

    //Input
    private int xInput;
    private bool jumpInput;
    private bool jumpInputStop;
    private bool grabInput;
    private bool dashInput;

    //Checks
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingWallBack;
    private bool isTouchingLedge;
    private bool isInCoyoteTime;
    private bool isInWallJumpCoyoteTime;
    private bool isTouchingWallPreviousFrame; //These previous frame checks are used to determine if we should start the wall jump coyote time
    private bool isTouchingWallBackPreviousFrame;
    private float wallJumpCoyoteTimeStart;
    private bool isJumping;

    public PlayerInAirState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, PlayerData playerData) : base(entity, stateMachine, animBoolName, playerData)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isTouchingWallPreviousFrame = isTouchingWall;
        isTouchingWallBackPreviousFrame = isTouchingWallBack;

        if (CollisionSenses)
        {
            isGrounded = CollisionSenses.Ground;
            isTouchingWall = CollisionSenses.WallFront;
            isTouchingWallBack = CollisionSenses.WallBack;
            isTouchingLedge = CollisionSenses.LedgeHorizontal;
        }
        
        if (isTouchingWall && !isTouchingLedge)
        {
            player.LedgeClimbState.SetDetectedPosition(player.transform.position);
        }

        if (!isInWallJumpCoyoteTime && !isTouchingWall && !isTouchingWallBack && (isTouchingWallPreviousFrame || isTouchingWallBackPreviousFrame)) //If we are not touching any walls but were touching at least one wall last frame, start wall jump coyote time
            StartWallJumpCoyoteTime();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        //These 4 lines are to make sure the player doesn't activate inAirCoyoteTime right after performing a wall jump.
        //If these bools weren't reset, we risk activating coyote time by accident in the dochecks function called in Enter() right after performing a wall jump
        isTouchingWallPreviousFrame = false;
        isTouchingWallBackPreviousFrame = false;
        isTouchingWall = false;
        isTouchingWallBack = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();

        xInput = player.InputHandler.NormalizedInputX;
        jumpInput = player.InputHandler.JumpInput;
        jumpInputStop = player.InputHandler.JumpInputStop;
        grabInput = player.InputHandler.GrabInput;
        dashInput = player.InputHandler.DashInput;

        CheckJumpMultiplier();

        if (player.InputHandler.AttackInputs[((int)CombatInputs.Primary)])
        {
            stateMachine.ChangeState(player.PrimaryAttackState);
        }
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.Secondary])
        {
            stateMachine.ChangeState(player.SecondaryAttackState);
        }
        else if (isGrounded && Movement?.CurrentVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(player.LandState);
        }
        else if (isTouchingWall && !isTouchingLedge && !isGrounded) //Ledge climb
        {
            stateMachine.ChangeState(player.LedgeClimbState);
        }
        else if (jumpInput && (isTouchingWall || isTouchingWallBack || isInWallJumpCoyoteTime)) //Wall jump
        {
            StopWallJumpCoyoteTime();
            isTouchingWall = CollisionSenses.WallFront; //We need to manually set this again since isTouchingWall is normally checked in fixed update but this code is ran in update. Without this, isTouchingWall might sometimes not be updated, causing the player to wall jump towards the wrong direction.
            player.WallJumpState.DetermineWallJumpDirection(isTouchingWall);
            stateMachine.ChangeState(player.WallJumpState);
        }
        else if (jumpInput && player.JumpState.CanJump) //Jumping in air (double jump etc.)
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if (isTouchingWall && grabInput && isTouchingLedge)
        {
            stateMachine.ChangeState(player.WallGrabState);
        }
        else if (isTouchingWall && xInput == Movement?.FacingDirection && Movement?.CurrentVelocity.y <= 0) //If player input towards wall, wall slide (also make sure player is falling before entering wall slide)
        {
            stateMachine.ChangeState(player.WallSlideState);
        }
        else if (dashInput && player.DashState.CheckIfCanDash)
        {
            stateMachine.ChangeState(player.DashState);
        }
        else
        {
            Movement?.CheckIfShouldFlip(xInput);
            Movement?.SetVelocityX(playerData.moveSpeed * xInput);
            player.anim.SetFloat("yVelocity", Movement.CurrentVelocity.y);
            player.anim.SetFloat("xVelocity", Mathf.Abs(Movement.CurrentVelocity.x));
        }
    }

    private void CheckJumpMultiplier()
    {
        if (isJumping)
        {
            if (jumpInputStop)
            {
                isJumping = false;
                Movement?.SetVelocityY(Movement.CurrentVelocity.y * playerData.variableJumpHeightMultiplier);
            }
            else if (Movement?.CurrentVelocity.y <= 0)
                isJumping = false;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void StartCoyoteTime() => isInCoyoteTime = true;

    private void CheckCoyoteTime()
    {
        if (isInCoyoteTime && Time.time > startTime + playerData.coyoteTime)
        {
            isInCoyoteTime = false;
            player.JumpState.DecreaseAmountOfJumpsLeft();
        }
    }

    public void StartWallJumpCoyoteTime()
    {
        isInWallJumpCoyoteTime = true;
        wallJumpCoyoteTimeStart = Time.time;
    }

    public void StopWallJumpCoyoteTime() => isInWallJumpCoyoteTime = false;

    private void CheckWallJumpCoyoteTime()
    {
        if (isInWallJumpCoyoteTime && Time.time > wallJumpCoyoteTimeStart + playerData.coyoteTime)
        {
            StopWallJumpCoyoteTime();
        }
    }

    public void SetIsJumping() => isJumping = true;
}
