using UnityEngine;

namespace Gamecells.Weapons.Components
{
    public class ActionHitboxData : ComponentData<AttackActionHitbox>
    {
        [field: SerializeField] public LayerMask DetectableLayers { get; private set; }
    }
}
