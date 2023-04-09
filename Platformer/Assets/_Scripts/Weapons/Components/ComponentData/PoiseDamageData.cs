using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamecells.Weapons.Components
{
    public class PoiseDamageData : ComponentData<AttackPoiseDamage>
    {
        protected override void SetComponentDependency()
        {
            this.ComponentDependency = typeof(PoiseDamage);
        }
    }
}
