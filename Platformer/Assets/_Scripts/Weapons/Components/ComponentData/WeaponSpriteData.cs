using Gamecells.Weapons.Components;
using UnityEngine;

namespace Gamecells.Weapons.Components
{
    public class WeaponSpriteData : ComponentData<AttackSprites>
    {
        protected override void SetComponentDependency()
        {
            this.ComponentDependency = typeof(WeaponSprite);
        }
    }
}
