using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{
    public PlayerAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, PlayerData playerData)
        : base(entity, stateMachine, animBoolName, playerData)
    {
    }
}