using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CoreComponent
{
    public Rigidbody2D RB { get; private set; }
    public int FacingDirection { get; private set; } = 1;
    public Vector2 CurrentVelocity { get; private set; }

    public bool CanSetVelocity { get; private set; } = true;

    private Vector2 vector2Workspace;

    protected override void Awake()
    {
        base.Awake();

        RB = GetComponentInParent<Rigidbody2D>();
    }

    public void LogicUpdate()
    {
        CurrentVelocity = RB.velocity;
    }

    public void SetVelocityZero()
    {
        vector2Workspace = Vector2.zero;
        SetFinalVelocity();
    }

    public virtual void SetVelocityX(float xVelocity)
    {
        vector2Workspace.Set(xVelocity, CurrentVelocity.y);
        SetFinalVelocity();
    }

    public virtual void SetVelocityY(float yVelocity)
    {
        vector2Workspace.Set(CurrentVelocity.x, yVelocity);
        SetFinalVelocity();
    }

    public virtual void SetVelocity(float velocity, Vector2 angle, int direction) //set velocity towards a specific angle. Note that you should always input a positive angle and only use direction to determine if we should flip that angle in the x axis (based on facing direction or attack direction etc.)
    {
        angle.Normalize();
        vector2Workspace.Set(angle.x * velocity * direction, angle.y * velocity); //Direction multiplied only on x axis to flip knockback horizontally only
        SetFinalVelocity();
    }

    public virtual void SetVelocity(float velocity, Vector2 direction) //Alternate set velocity that simply set velocity towards a direction
    {
        vector2Workspace = direction * velocity;
        RB.velocity = vector2Workspace;
        SetFinalVelocity();
    }

    private void SetFinalVelocity()
    {
        if (!CanSetVelocity) return;

        RB.velocity = vector2Workspace;
        CurrentVelocity = vector2Workspace;
    }

    public void AllowSetVelocity(bool value)
    {
        CanSetVelocity = value;
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
        RB.transform.Rotate(0, 180f, 0f);
    }
}
