using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamecells.Weapons.Components
{
    public class PoiseDamage : WeaponComponent<PoiseDamageData, AttackPoiseDamage>
    {
        private ActionHitbox _hitbox;
        
        private void HandleDetectCollider2D(Collider2D[] colliders)
        {
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out IPoiseDamageable poiseDamageable))
                {
                    poiseDamageable.DamagePoise(currentAttackData.Amount);
                }
            }
        }

        protected override void Start()
        {
            base.Start();

            _hitbox = GetComponent<ActionHitbox>();
            _hitbox.OnDetectedCollider2D += HandleDetectCollider2D;
        }

        protected override void OnDestroy()
        {
            _hitbox.OnDetectedCollider2D -= HandleDetectCollider2D;
        }
    }
}
