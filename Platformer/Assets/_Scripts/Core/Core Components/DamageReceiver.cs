using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamecells.CoreSystem
{
    public class DamageReceiver : CoreComponent, IDamageable
    {
        [SerializeField] private GameObject damageParticles;
        [SerializeField] private float cameraShakeIntensity = 0.15f;

        private Stats stats;
        private ParticleManager particleManager;

        public void TakeDamage(float amount)
        {
            Debug.Log($"{core.transform.parent.name} damaged!");
            stats.Health.Decrease(amount);
            particleManager.StartParticles(damageParticles);
            CameraManager.CameraShake(cameraShakeIntensity);
        }

        protected override void Awake()
        {
            base.Awake();

            stats = core.GetCoreComponent<Stats>();
            particleManager = core.GetCoreComponent<ParticleManager>();
        }
    }
}
