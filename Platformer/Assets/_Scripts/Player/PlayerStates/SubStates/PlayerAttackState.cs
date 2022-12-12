using System.Collections;
using System.Collections.Generic;
using Gamecells.Weapons;
using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{
    private Weapon weapon;

    public PlayerAttackState
    (
        Entity entity, 
        FiniteStateMachine stateMachine, 
        string animBoolName, 
        PlayerData playerData, 
        Weapon weapon
    )
        : base(entity, stateMachine, animBoolName, playerData)
    {
        this.weapon = weapon;
        weapon.OnExit += ExitHandler;
    }

    public override void Enter()
    {
        base.Enter();

        weapon.Enter();
    }

    private void ExitHandler()
    {
        AnimationFinishTrigger();
        isAbilityDone = true;
    }
}