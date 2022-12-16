using Gamecells.Weapons.Components.ComponentData.AttackData;
using UnityEngine;

namespace Gamecells.Weapons.Components.ComponentData
{
    public class WeaponSpriteData : ComponentData
    {
        [field: SerializeField] public AttackSprites[] AttackData { get; private set; }
    }
}
