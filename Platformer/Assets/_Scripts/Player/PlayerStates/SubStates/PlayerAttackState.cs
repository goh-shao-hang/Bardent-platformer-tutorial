using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{
    private Weapon weapon;

    private int xInput;
    private bool shouldCheckFlip;
    private float desiredVelocity;
    private bool setVelocity;

    public PlayerAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, PlayerData playerData) : base(entity, stateMachine, animBoolName, playerData)
    {
    }

    public override void Enter()
    {
        base.Enter();

        setVelocity = false;

        weapon.EnterWeapon();
    }

    public override void Exit()
    {
        base.Exit();

        weapon.ExitWeapon();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormalizedInputX;

        if (shouldCheckFlip)
            core.Movement.CheckIfShouldFlip(xInput);

        if (setVelocity)
        {
            core.Movement.SetVelocityX(desiredVelocity * core.Movement.FacingDirection);
        }
    }

    public void SetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
        weapon.InitializeWeapon(this);
    }

    public void SetPlayerVelocity(float velocity)
    {
        core.Movement.SetVelocityX(velocity * core.Movement.FacingDirection);
        desiredVelocity = velocity;
        setVelocity = true;
    }

    public void SetFlipCheck(bool value)
    {
        shouldCheckFlip = value;
    }

    #region Animation Triggers

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAbilityDone = true;
    }

    #endregion
}
