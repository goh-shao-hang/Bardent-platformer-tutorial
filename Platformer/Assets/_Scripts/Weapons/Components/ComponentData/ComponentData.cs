using System;
using UnityEngine;

namespace Gamecells.Weapons.Components
{
    [Serializable]
    public class ComponentData
    {
        
    }

    public class ComponentData<T> : ComponentData where T : AttackData
    {
        [field: SerializeField] public T[] AttackData { get; private set; } 
    }
}
