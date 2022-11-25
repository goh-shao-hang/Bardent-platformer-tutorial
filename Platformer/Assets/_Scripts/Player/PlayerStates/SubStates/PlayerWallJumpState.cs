using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerAbilityState
{
    private int wallJumpDirection;

    public PlayerWallJumpState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, PlayerData playerData) : base(entity, stateMachine, animBoolName, playerData)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.InputHandler.UseJumpInput();
        player.JumpState.ResetAmountOfJumpsLeft();
        Movement?.SetVelocity(playerData.wallJumpVelocity, playerData.wallJumpAngle, wallJumpDirection);
        Movement?.CheckIfShouldFlip(wallJumpDirection); //Flip the player if needed. We only need to do this one throughout one wall jump
        player.JumpState.DecreaseAmountOfJumpsLeft(); //Up to personal preference if wall jump should consume one jump. In this case yes.
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        player.anim.SetFloat("xVelocity", Mathf.Abs(Movement.CurrentVelocity.x));
        player.anim.SetFloat("yVelocity", Movement.CurrentVelocity.y);

        if (Time.time >= startTime + playerData.wallJumpTime)
        {
            isAbilityDone = true; //Inform the ability superstate that the wall jump is finished
        }
    }

    public void DetermineWallJumpDirection(bool isTouchingWall) => wallJumpDirection = isTouchingWall ? -Movement.FacingDirection : Movement.FacingDirection; //If is facing wall, jump away from it, else jump forward
}
