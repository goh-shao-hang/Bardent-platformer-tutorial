namespace Gamecells.Weapons.Components
{
    public class KnockbackData : ComponentData<AttackKnockback>
    {
        protected override void SetComponentDependency()
        {
            this.ComponentDependency = typeof(Knockback);
        }
    }
}
