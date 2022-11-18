using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utilities.ComponentErrorHandler;

public class CollisionSenses : CoreComponent
{
    [Header("Assignables")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheckHorizontal;
    [SerializeField] private Transform ledgeCheckVertical;
    [SerializeField] private Transform ceilingCheck;

    public Transform GroundCheck => ReturnIfNotNull(groundCheck, core.transform.parent.name);
    public Transform WallCheck => ReturnIfNotNull(wallCheck, core.transform.parent.name);
    public Transform LedgeCheckHorizontal => ReturnIfNotNull(ledgeCheckHorizontal, core.transform.parent.name);
    public Transform LedgeCheckVertical => ReturnIfNotNull(ledgeCheckVertical, core.transform.parent.name);
    public Transform CeilingCheck => ReturnIfNotNull(ceilingCheck, core.transform.parent.name);

    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;

    public float GroundCheckRadius { get => groundCheckRadius; set => groundCheckRadius = value; }
    public float WallCheckDistance { get => wallCheckDistance; set => wallCheckDistance = value; }
    public LayerMask WhatIsGround { get => whatIsGround; set => whatIsGround = value; }

    public bool Ground => Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, whatIsGround);

    public bool WallFront => Physics2D.Raycast(WallCheck.position, Vector2.right * core.Movement.FacingDirection, wallCheckDistance, whatIsGround);

    public bool WallBack => Physics2D.Raycast(WallCheck.position, Vector2.right * -core.Movement.FacingDirection, wallCheckDistance, whatIsGround); //Touching a wall from behind

    public bool LedgeHorizontal => Physics2D.Raycast(LedgeCheckHorizontal.position, Vector2.right * core.Movement.FacingDirection, wallCheckDistance, whatIsGround);

    public bool LedgeVertical => Physics2D.Raycast(LedgeCheckVertical.position, Vector2.down, wallCheckDistance, WhatIsGround);

    public bool Ceiling => Physics2D.OverlapCircle(CeilingCheck.position, groundCheckRadius, whatIsGround);

    
}
