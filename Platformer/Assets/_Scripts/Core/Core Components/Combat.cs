using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class Combat : CoreComponent, IDamageable, IKnockbackable
{
    private Movement movement;
    private CollisionSenses collisionSenses;
    private Stats stats;

    private Movement Movement => movement ??= core.GetCoreComponent<Movement>();
    private CollisionSenses CollisionSenses => collisionSenses ??= core.GetCoreComponent<CollisionSenses>();
    private Stats Stats => stats ??= core.GetCoreComponent<Stats>();
    
    [SerializeField] private float maxKnockbackTime = 0.2f;

    private bool isKnockbackActive = false;
    private float knockbackStartTime;

    public void TakeDamage(float amount)
    {
        Debug.Log($"{core.transform.parent.name} damaged!");
        Stats?.DecreaseHealth(amount);
    }

    public void Knockback(float strength, Vector2 angle, int direction)
    {
        Movement?.SetVelocity(strength, angle, direction);
        Movement?.AllowSetVelocity(false);
        StartCoroutine(CheckKnockback());
    }

    IEnumerator CheckKnockback()
    {
        isKnockbackActive = true;

        while (!(isKnockbackActive && (Movement?.CurrentVelocity.y <= 0.01f && CollisionSenses.Ground) || Time.time >= knockbackStartTime + maxKnockbackTime))
        {
            yield return null;
        }

        isKnockbackActive = false;
        Movement.AllowSetVelocity(true);
    }
}
