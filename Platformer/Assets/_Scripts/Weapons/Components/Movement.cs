using Gamecells.Weapons.Components;

namespace Gamecells.Weapons.Components
{
    public class Movement : WeaponComponent<MovementData, AttackMovement>
    {
        private CoreSystem.Movement coreMovement;
        private CoreSystem.Movement CoreMovement => coreMovement ??= Core.GetCoreComponent<CoreSystem.Movement>();

        private void HandleStartMovement()
        {
            CoreMovement.SetVelocity(currentAttackData.Velocity, currentAttackData.Direction, CoreMovement.FacingDirection);
        }

        private void HandleStopMovement()
        {
            CoreMovement.SetVelocityZero();
        }

        protected override void Start()
        {
            base.Start();

            animationEventHandler.OnStartMovement += HandleStartMovement;
            animationEventHandler.OnStopMovement += HandleStopMovement;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            animationEventHandler.OnStartMovement -= HandleStartMovement;
            animationEventHandler.OnStopMovement -= HandleStopMovement;
        }
    }
}
