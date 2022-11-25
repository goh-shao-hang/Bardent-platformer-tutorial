using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeState : EnemyState
{
    protected D_DodgeState stateData;

    protected bool performCloseRangeAction;
    protected bool isPlayerInMaxAggroRange;
    protected bool isGrounded;
    protected bool isDodgeOver;

    public DodgeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DodgeState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if (CollisionSenses)
        {
            performCloseRangeAction = enemy.CheckPlayerInCloseRangeAction();
            isPlayerInMaxAggroRange = enemy.CheckPlayerInMaxAggroRange();
            isGrounded = CollisionSenses.Ground;
        }
    }

    public override void Enter()
    {
        base.Enter();

        isDodgeOver = false;
        Movement?.SetVelocity(stateData.dodgeSpeed, stateData.dodgeAngle, -Movement.FacingDirection); 
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= startTime + stateData.dodgeTime && isGrounded)
        {
            isDodgeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
