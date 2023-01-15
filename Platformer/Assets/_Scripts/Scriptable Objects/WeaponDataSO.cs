using Gamecells.Weapons.Components;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gamecells.Weapons
{
    [CreateAssetMenu(fileName = "newWeaponData", menuName = "Data/Weapon Data/Basic Weapon Data", order = 0)]
    public class WeaponDataSO : ScriptableObject
    {
        [field: SerializeField] public int NumberOfAttacks { get; private set; } = 3;

        [field: SerializeReference] public List<ComponentData> ComponentData { get; private set; }

        public T GetData<T>()
        {
            return ComponentData.OfType<T>().FirstOrDefault();
        }

        public void AddData(ComponentData data)
        {
            if (ComponentData.FirstOrDefault(t => t.GetType() == data.GetType()) == null) //If a component data with the same type already exists in the list, don't add a new one
            {
                ComponentData.Add(data);
            }
            else
            {
                Debug.LogWarning($"A component data of type {data.GetType().Name} already exists.");
            }
        }
    }
}
