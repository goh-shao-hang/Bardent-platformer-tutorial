using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CoreComponent
{
    public Rigidbody2D rb { get; private set; }
    public int FacingDirection { get; private set; } = 1;
    public Vector2 CurrentVelocity { get; private set; }

    private Vector2 vector2Workspace;

    protected override void Awake()
    {
        base.Awake();

        rb = GetComponentInParent<Rigidbody2D>();
    }

    public void LogicUpdate()
    {
        CurrentVelocity = rb.velocity;
    }

    public void SetVelocityZero()
    {
        rb.velocity = Vector2.zero;
        CurrentVelocity = Vector2.zero;
    }

    public virtual void SetVelocityX(float xVelocity)
    {
        vector2Workspace.Set(xVelocity, CurrentVelocity.y);
        rb.velocity = vector2Workspace;
        CurrentVelocity = vector2Workspace;
    }

    public virtual void SetVelocityY(float yVelocity)
    {
        vector2Workspace.Set(CurrentVelocity.x, yVelocity);
        rb.velocity = vector2Workspace;
        CurrentVelocity = vector2Workspace;
    }

    public virtual void SetVelocity(float velocity, Vector2 angle, int direction) //set velocity towards a specific angle. Note that you should always input a positive angle and only use direction to determine if we should flip that angle in the x axis (based on facing direction or attack direction etc.)
    {
        angle.Normalize();
        vector2Workspace.Set(angle.x * velocity * direction, angle.y * velocity); //Direction multiplied only on x axis to flip knockback horizontally only
        rb.velocity = vector2Workspace;
        CurrentVelocity = vector2Workspace;
    }

    public virtual void SetVelocity(float velocity, Vector2 direction) //Alternate set velocity that simply set velocity towards a direction
    {
        rb.velocity = direction * velocity;
        CurrentVelocity = rb.velocity;
    }

    public void CheckIfShouldFlip(int xInput)
    {
        if (xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }

    public void Flip()
    {
        FacingDirection *= -1;
        rb.transform.Rotate(0, 180f, 0f);
    }
}
