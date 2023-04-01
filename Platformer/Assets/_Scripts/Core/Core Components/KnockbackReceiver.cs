using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gamecells.CoreSystem
{
    public class KnockbackReceiver : CoreComponent, IKnockBackable
    {
        [SerializeField] private float maxKnockBackTime = 0.2f;

        private bool isKnockBackActive = false;
        private float knockBackStartTime;

        private CoreComp<Movement> movement;
        private CoreComp<CollisionSenses> collisionSenses;

        public void KnockBack(float strength, Vector2 angle, int direction)
        {
            movement.Comp?.SetVelocity(strength, angle, direction);
            movement.Comp?.AllowSetVelocity(false);
            isKnockBackActive = true;
            knockBackStartTime = Time.time;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            CheckKnockBack();
        }

        private void CheckKnockBack()
        {
            if (isKnockBackActive && ((movement.Comp?.CurrentVelocity.y <= 0.01f && collisionSenses.Comp.Ground) || Time.time >= knockBackStartTime + maxKnockBackTime))
            {
                isKnockBackActive = false;
                movement.Comp.AllowSetVelocity(true);
            }
        }

        protected override void Awake()
        {
            base.Awake();

            movement = new CoreComp<Movement>(core);
            collisionSenses = new CoreComp<CollisionSenses>(core);
        }
    }
}