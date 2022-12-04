using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    #region Assignables

    [Header("Base Data")]
    [SerializeField] private PlayerData playerData;

    [Header("Assignables")]
    [SerializeField] private Transform dashDirectionIndicator;

    public PlayerInputHandler InputHandler { get; private set; }
    public BoxCollider2D MovementCollider { get; private set; }
    public Transform DashDirectionIndicator => dashDirectionIndicator;

    #endregion

    #region States
    
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallGrabState WallGrabState { get; private set; }
    public PlayerWallClimbState WallClimbState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerLedgeClimbState LedgeClimbState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerCrouchIdleState CrouchIdleState { get; private set; }
    public PlayerCrouchMoveState CrouchMoveState { get; private set; }
    public PlayerAttackState PrimaryAttackState { get; private set; }
    public PlayerAttackState SecondaryAttackState { get; private set; }

    #endregion

    #region Unity Callback Functions
    protected override void Awake()
    {
        base.Awake();

        IdleState = new PlayerIdleState(this, StateMachine, "idle", playerData);
        MoveState = new PlayerMoveState(this, StateMachine, "move", playerData);
        JumpState = new PlayerJumpState(this, StateMachine, "inAir", playerData); //inAir as animBoolName to share same blend tree with InAirState
        InAirState = new PlayerInAirState(this, StateMachine, "inAir", playerData);
        LandState = new PlayerLandState(this, StateMachine, "land", playerData);
        WallSlideState = new PlayerWallSlideState(this, StateMachine, "wallSlide", playerData);
        WallGrabState = new PlayerWallGrabState(this, StateMachine, "wallGrab", playerData);
        WallClimbState = new PlayerWallClimbState(this, StateMachine, "wallClimb", playerData);
        WallJumpState = new PlayerWallJumpState(this, StateMachine, "inAir", playerData);
        LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, "ledgeClimbState", playerData);
        DashState = new PlayerDashState(this, StateMachine, "inAir", playerData);
        CrouchIdleState = new PlayerCrouchIdleState(this, StateMachine, "crouchIdle", playerData);
        CrouchMoveState = new PlayerCrouchMoveState(this, StateMachine, "crouchMove", playerData);
        PrimaryAttackState = new PlayerAttackState(this, StateMachine, "attack", playerData);
        SecondaryAttackState = new PlayerAttackState(this, StateMachine, "attack", playerData);
    }

    protected override void Start()
    {
        base.Start();

        InputHandler = GetComponent<PlayerInputHandler>();
        MovementCollider = GetComponent<BoxCollider2D>();

        StateMachine.Inititalize(IdleState);
    }
    #endregion

    #region Other Functions

    public void SetColliderHeight(float height)
    {
        Vector2 center = MovementCollider.offset;
        Vector2 newColliderSize = new Vector2(MovementCollider.size.x, height);

        center.y += (height - MovementCollider.size.y) / 2; //Shift the offset so that the bottom of the collider remains on the ground. Every 1 unit the collider is shrinked, the offset needs to be lowered by 0.5 unit. The minus sign handles if the collider should be shrinked or growed.

        MovementCollider.size = newColliderSize;
        MovementCollider.offset = center;
    }

    private void AnimationTrigger() => ((PlayerState)StateMachine.CurrentState).AnimationTrigger(); //Casting required to call this function since it is exclusive to the child class of State (PlayerState)

    private void AnimationFinishTrigger() => ((PlayerState)StateMachine.CurrentState).AnimationFinishTrigger();

    #endregion
}
