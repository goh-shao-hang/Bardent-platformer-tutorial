using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamecells.Weapons.Components
{
    public class DamageData : ComponentData<AttackDamage>
    {
        protected override void SetComponentDependency()
        {
            this.ComponentDependency = typeof(Damage);
        }
    }
}
