using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public FiniteStateMachine StateMachine;

    public Rigidbody2D rb { get; protected set; }
    public Animator anim { get; protected set; }

    protected virtual void Awake()
    {
        StateMachine = new FiniteStateMachine();
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
    }

    protected virtual void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
}
