using Gamecells.Weapons.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamecells.Weapons.Components
{
    public class MovementData : ComponentData<AttackMovement>
    {
        protected override void SetComponentDependency()
        {
            this.ComponentDependency = typeof(Movement);
        }
    }
}