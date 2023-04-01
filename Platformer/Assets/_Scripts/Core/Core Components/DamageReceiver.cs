using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamecells.CoreSystem
{
    public class DamageReceiver : CoreComponent, IDamageable
    {
        [SerializeField] private GameObject damageParticles;
        [SerializeField] private float cameraShakeIntensity = 0.15f;

        private CoreComp<Stats> stats;
        private CoreComp<ParticleManager> particleManager;

        public void TakeDamage(float amount)
        {
            Debug.Log($"{core.transform.parent.name} damaged!");
            stats.Comp?.DecreaseHealth(amount);
            particleManager.Comp?.StartParticles(damageParticles);
            CameraManager.CameraShake(cameraShakeIntensity);
        }

        protected override void Awake()
        {
            base.Awake();

            stats = new CoreComp<Stats>(core);
            particleManager = new CoreComp<ParticleManager>(core);
        }
    }
}
