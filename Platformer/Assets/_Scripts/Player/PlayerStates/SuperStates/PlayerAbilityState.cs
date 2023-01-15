using Gamecells.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    private Movement movement;
    private CollisionSenses collisionSenses;

    protected Movement Movement => movement ??= core.GetCoreComponent<Movement>();
    private CollisionSenses CollisionSenses => collisionSenses ??= core.GetCoreComponent<CollisionSenses>();

    protected bool isAbilityDone;

    private bool isGrounded;

    public PlayerAbilityState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, PlayerData playerData) : base(entity, stateMachine, animBoolName, playerData)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if (CollisionSenses)
            isGrounded = CollisionSenses.Ground;
    }

    public override void Enter()
    {
        base.Enter();

        isAbilityDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAbilityDone)
        {
            if (isGrounded && Movement?.CurrentVelocity.y < 0.01f)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else
            {
                stateMachine.ChangeState(player.InAirState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
