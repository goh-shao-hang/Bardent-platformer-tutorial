using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamecells.Weapons.Components
{
    public class Knockback : WeaponComponent<KnockbackData, AttackKnockback>
    {
        private ActionHitbox hitbox;

        private CoreSystem.Movement movement;

        private void HandleDetectCollider2D(Collider2D[] colliders)
        {
            foreach (var item in colliders)
            {
                if (item.TryGetComponent(out IKnockBackable knockbackable))
                {
                    knockbackable.KnockBack(currentAttackData.Strength, currentAttackData.Angle, movement.FacingDirection);
                }
            }
        }

        protected override void Start()
        {
            base.Start();

            hitbox = GetComponent<ActionHitbox>();
            hitbox.OnDetectedCollider2D += HandleDetectCollider2D;
            movement = Core.GetCoreComponent<CoreSystem.Movement>();
        }

        protected override void OnDestroy()
        {
            hitbox.OnDetectedCollider2D -= HandleDetectCollider2D;
        }
    }
}
