using Gamecells.CoreSystem;
using System;
using UnityEngine;

namespace Gamecells.Weapons.Components
{
    public class ActionHitbox : WeaponComponent<ActionHitboxData, AttackActionHitbox>
    {
        public event Action<Collider2D[]> OnDetectedCollider2D;

        private CoreComp<CoreSystem.Movement> movement;
        private Vector2 offset;
        private Collider2D[] detectedColliders;

        protected override void Start()
        {
            base.Start();

            movement = new CoreComp<CoreSystem.Movement>(Core);
            animationEventHandler.OnAttackAction += HandleAttackAction;
        }

        private void HandleAttackAction()
        {
            offset.Set(
                transform.position.x + currentAttackData.Hitbox.center.x * movement.Comp.FacingDirection,
                transform.position.y + currentAttackData.Hitbox.center.y
                );

            detectedColliders = Physics2D.OverlapBoxAll(offset, currentAttackData.Hitbox.size, 0f, componentData.DetectableLayers);
            if (detectedColliders.Length > 0)
            {
                OnDetectedCollider2D?.Invoke(detectedColliders);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            animationEventHandler.OnAttackAction -= HandleAttackAction;
        }

        private void OnDrawGizmosSelected()
        {
            if (componentData == null) return;

            foreach (var item in componentData.AttackData)
            {
                if (!item.Debug) continue;

                Gizmos.DrawWireCube(transform.position + (Vector3)item.Hitbox.center, item.Hitbox.size);
            }
        }
    }
}