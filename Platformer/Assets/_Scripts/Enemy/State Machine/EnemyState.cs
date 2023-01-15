using Gamecells.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : State
{
    private Movement movement;
    private CollisionSenses collisionSenses;

    protected Movement Movement => movement ??= core.GetCoreComponent<Movement>();
    protected CollisionSenses CollisionSenses => collisionSenses ??= core.GetCoreComponent<CollisionSenses>();

    protected Enemy enemy;

    public EnemyState(Entity entity, FiniteStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
        enemy = entity as Enemy;
    }
}
