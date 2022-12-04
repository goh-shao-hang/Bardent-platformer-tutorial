using System;
using UnityEngine;

public class Death : CoreComponent
{
    [SerializeField] private GameObject[] deathParticles;

    private Stats stats;
    private Stats Stats => stats ??= core.GetCoreComponent<Stats>();
        
    private ParticleManager particleManager;
    private ParticleManager ParticleManager => particleManager ??= core.GetCoreComponent<ParticleManager>();
    
    public virtual void Die()
    {
        foreach (GameObject particle in deathParticles)
        {
            ParticleManager.StartParticles(particle);
        }
        
        core.transform.parent.gameObject.SetActive(false);
    }

    private void OnEnable() => Stats.OnHealthZero += Die;

    private void OnDisable() => Stats.OnHealthZero -= Die;
}
