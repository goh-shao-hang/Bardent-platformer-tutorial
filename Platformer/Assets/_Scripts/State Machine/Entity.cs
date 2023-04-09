using Gamecells.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Assignables

    public Core Core;
    public FiniteStateMachine StateMachine;
    protected Stats stats;
    public Animator anim { get; protected set; }

    #endregion

    #region Unity Callback Functions
    protected virtual void Awake()
    {
        Core = GetComponentInChildren<Core>();
        StateMachine = new FiniteStateMachine();
        stats = Core.GetCoreComponent<Stats>();

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
