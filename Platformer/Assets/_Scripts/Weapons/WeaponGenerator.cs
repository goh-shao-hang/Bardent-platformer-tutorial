using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamecells.Weapons.Components;
using System;
using System.Linq;

namespace Gamecells.Weapons
{
    public class WeaponGenerator : MonoBehaviour
    {
        [SerializeField] private Weapon weapon;
        [SerializeField] private WeaponDataSO weaponData;

        private List<WeaponComponent> componentsAlreadyOnWeapon = new List<WeaponComponent>();
        private List<WeaponComponent> componentsToAdd = new List<WeaponComponent>();
        private List<Type> componentDependencies = new List<Type>(); //Components needed by new weapon

        private void Start()
        {
            GenerateWeapon(weaponData);
        }

        [ContextMenu("Test Generate")]
        private void TestGeneration()
        {
            GenerateWeapon(weaponData);
        }

        public void GenerateWeapon(WeaponDataSO newWeaponData)
        {
            weapon.SetWeaponData(newWeaponData);

            componentsAlreadyOnWeapon.Clear();
            componentsToAdd.Clear();
            componentDependencies.Clear();

            componentsAlreadyOnWeapon = GetComponents<WeaponComponent>().ToList();
            componentDependencies = newWeaponData.GetAllDependencies();

            foreach (var dependency in componentDependencies)
            {
                //If needed component already exists, continue
                if (componentsToAdd.FirstOrDefault(component => component.GetType() == dependency))
                {
                    continue;
                }

                var weaponComponent = componentsAlreadyOnWeapon.FirstOrDefault(component => component.GetType() == dependency);
                    
                if (weaponComponent == null) //If this dependency is not on the weapon yet, add it
                {
                    weaponComponent = gameObject.AddComponent(dependency) as WeaponComponent;
                }

                weaponComponent.Init(); //Reset weapon data even if the weapon component is the same since data can be different based on weapon
                componentsToAdd.Add(weaponComponent);
            }

            var componentsToRemove = componentsAlreadyOnWeapon.Except(componentsToAdd);

            foreach (var weaponComponent in componentsToRemove) //Remove duplicate components
            {
                Destroy(weaponComponent);
            }
        }
    }
}
