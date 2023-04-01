using Gamecells.Weapons.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Gamecells.Weapons
{
    [CustomEditor(typeof(WeaponDataSO))] //Whenever the WeaponDataSO monobehavior class is inspected, run the following
    public class WeaponDataSOEditor : Editor
    {
        private static List<Type> dataComponentTypes = new List<Type>();
        //Type is a class that represents any type such as floats, integers and classes.
        //This allows us to store the classes we want as types instead of having to store instances of the classes.

        private WeaponDataSO dataSO;

        private bool showForceUpdateButtons;
        private bool showAddComponentButtons;

        private void OnEnable()
        {
            dataSO = (WeaponDataSO)target; //target is the object being inspected.
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Set Number Of Attacks"))
            {
                foreach (ComponentData item in dataSO.ComponentData)
                {
                    item.InitializeAttackData(dataSO.NumberOfAttacks);
                }
            }

            showAddComponentButtons = EditorGUILayout.Foldout(showAddComponentButtons, "Add Components");

            if (showAddComponentButtons)
            {
                foreach (Type dataComponentType in dataComponentTypes)
                {
                    if (GUILayout.Button(dataComponentType.Name))
                    {
                        var comp = Activator.CreateInstance(dataComponentType) as ComponentData; //Extract information from a specific type and create an instance(Object) out of it.
                        if (comp == null) return;

                        comp.InitializeAttackData(dataSO.NumberOfAttacks);
                        dataSO.AddData(comp);
                    }
                }
            }

            showForceUpdateButtons = EditorGUILayout.Foldout(showForceUpdateButtons, "Force Update Buttons");

            if (showForceUpdateButtons)
            {
                if (GUILayout.Button("Force Update Component Names"))
                {
                    foreach (ComponentData item in dataSO.ComponentData)
                    {
                        item.SetComponentName();
                    }
                }

                if (GUILayout.Button("Force Update Attack Names"))
                {
                    foreach (ComponentData item in dataSO.ComponentData)
                    {
                        item.SetAttackDataNames();
                    }
                }
            }

        }

        [DidReloadScripts] //Calls method marked with this attribute when scripts are recompiled. The function called must be static.
        private static void OnRecompile()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies(); //Get all assemblies in the current applciation domain. This var effectively contains EVERYTHING in the application so use with caution.
            var types = assemblies.SelectMany(assembly => assembly.GetTypes()); //Loop through each assembly and get all types. SelectMany retrieve all types in an assemblies in a IEnumerable sequence and flattens all sequences into one when all assemblies are looked thorugh. 
            var filteredTypes = types.Where(type => type.IsSubclassOf(typeof(ComponentData)) && !type.ContainsGenericParameters && type.IsClass); //Lastly, loop through all existing types and retrieve those that inherit from ComponentData. This gives us all component data types we need to make a dynamic list.

            //Note that the process above is extremely slow and is ok only because we are not using it during runtime and only during compilation.

            dataComponentTypes = filteredTypes.ToList(); //ToList converts an IEnumberable sequence to a list.
        }
    }
}
