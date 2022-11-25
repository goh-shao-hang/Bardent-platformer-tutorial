using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class CollisionSenses : CoreComponent
{
    private Movement movement;

    protected Movement Movement => movement ??= core.GetCoreComponent<Movement>();

    [Header("Assignables")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheckHorizontal;
    [SerializeField] private Transform ledgeCheckVertical;
    [SerializeField] private Transform ceilingCheck;

    public Transform GroundCheck => GenericNotImplementedError.TryGet(groundCheck, core.transform.parent.name);
    public Transform WallCheck => GenericNotImplementedError.TryGet(wallCheck, core.transform.parent.name);
    public Transform LedgeCheckHorizontal => GenericNotImplementedError.TryGet(ledgeCheckHorizontal, core.transform.parent.name);
    public Transform LedgeCheckVertical => GenericNotImplementedError.TryGet(ledgeCheckVertical, core.transform.parent.name);
    public Transform CeilingCheck => GenericNotImplementedError.TryGet(ceilingCheck, core.transform.parent.name);

    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;

    public float GroundCheckRadius { get => groundCheckRadius; set => groundCheckRadius = value; }
    public float WallCheckDistance { get => wallCheckDistance; set => wallCheckDistance = value; }
    public LayerMask WhatIsGround { get => whatIsGround; set => whatIsGround = value; }

    public bool Ground => Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, whatIsGround);

    public bool WallFront => Physics2D.Raycast(WallCheck.position, Vector2.right * Movement.FacingDirection, wallCheckDistance, whatIsGround);

    public bool WallBack => Physics2D.Raycast(WallCheck.position, Vector2.right * -Movement.FacingDirection, wallCheckDistance, whatIsGround); //Touching a wall from behind

    public bool LedgeHorizontal => Physics2D.Raycast(LedgeCheckHorizontal.position, Vector2.right * Movement.FacingDirection, wallCheckDistance, whatIsGround);

    public bool LedgeVertical => Physics2D.Raycast(LedgeCheckVertical.position, Vector2.down, wallCheckDistance, WhatIsGround);

    public bool Ceiling => Physics2D.OverlapCircle(CeilingCheck.position, groundCheckRadius, whatIsGround);   
}
