using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Assignables

    public Core Core;
    public FiniteStateMachine StateMachine;

    public Animator anim { get; protected set; }
    #endregion

    #region Unity Callback Functions
    protected virtual void Awake()
    {
        Core = GetComponentInChildren<Core>();
        StateMachine = new FiniteStateMachine();

        anim = GetComponent<Animator>();
    }

    protected virtual void Start() { }

    protected virtual void Update()
    {
        Core.LogicUpdate();

        StateMachine.CurrentState.LogicUpdate();
    }

    protected virtual void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
    #endregion
}
