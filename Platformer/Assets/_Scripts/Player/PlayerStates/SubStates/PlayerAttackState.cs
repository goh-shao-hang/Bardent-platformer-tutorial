using Gamecells.Weapons;
using System.Collections;
using System.Collections.Generic;

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
    }

    public override void Enter()
    {
        base.Enter();

        weapon.Enter();
    }
}