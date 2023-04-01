using UnityEngine;

namespace Gamecells.Weapons.Components
{
    public class ActionHitboxData : ComponentData<AttackActionHitbox>
    {
        [field: SerializeField] public LayerMask DetectableLayers { get; private set; }

        protected override void SetComponentDependency()
        {
            this.ComponentDependency = typeof(ActionHitbox);
        }
    }
}
