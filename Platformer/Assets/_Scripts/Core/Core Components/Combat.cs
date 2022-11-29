using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class Combat : CoreComponent, IDamageable, IKnockbackable
{
    public event Action<float> OnDamageTaken;

    private Movement movement;
    private CollisionSenses collisionSenses;
    private Stats stats;
    private ParticleManager particleManager;

    private Movement Movement => movement ??= core.GetCoreComponent<Movement>();
    private CollisionSenses CollisionSenses => collisionSenses ??= core.GetCoreComponent<CollisionSenses>();
    private Stats Stats => stats ??= core.GetCoreComponent<Stats>();
    private ParticleManager ParticleManager => particleManager ??= core.GetCoreComponent<ParticleManager>();

    [SerializeField] private GameObject damageParticles;
    
    [SerializeField] private float cameraShakeIntensity = 0.15f;
    [SerializeField] private float maxKnockbackTime = 0.2f;

    private bool isKnockbackActive = false;
    private float knockbackStartTime;

    public void TakeDamage(float amount)
    {
        Debug.Log($"{core.transform.parent.name} damaged!");
        Stats?.DecreaseHealth(amount);
        ParticleManager?.StartParticles(damageParticles);
        OnDamageTaken?.Invoke(cameraShakeIntensity);
    }

    public void Knockback(float strength, Vector2 angle, int direction)
    {
        Movement?.SetVelocity(strength, angle, direction);
        Movement?.AllowSetVelocity(false);
        StartCoroutine(CheckKnockback());
        CameraManager.CameraShake(cameraShakeIntensity);
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
