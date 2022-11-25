using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class Combat : CoreComponent, IDamageable, IKnockbackable
{
    [SerializeField] private float maxKnockbackTime = 0.2f;

    private bool isKnockbackActive = false;
    private float knockbackStartTime;

    public void TakeDamage(float amount)
    {
        Debug.Log($"{core.transform.parent.name} damaged!");
        core.Stats.DecreaseHealth(amount);
    }

    public void Knockback(float strength, Vector2 angle, int direction)
    {
        core.Movement.SetVelocity(strength, angle, direction);
        core.Movement.AllowSetVelocity(false);
        StartCoroutine(CheckKnockback());
    }

    IEnumerator CheckKnockback()
    {
        isKnockbackActive = true;

        while (!(isKnockbackActive && (core.Movement.CurrentVelocity.y <= 0.01f && core.CollisionSenses.Ground) || Time.time >= knockbackStartTime + maxKnockbackTime))
        {
            yield return null;
        }

        isKnockbackActive = false;
        core.Movement.AllowSetVelocity(true);
    }
}
