using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Gamecells.Utilities;

public class Core : MonoBehaviour
{
    private readonly List<CoreComponent> CoreComponents = new List<CoreComponent>();

    private void Awake()
    {

    }

    public void LogicUpdate()
    {
        foreach (CoreComponent component in CoreComponents)
        {
            component.LogicUpdate();
        }
    }

    public void AddComponent(CoreComponent component)
    {
        if (!CoreComponents.Contains(component))
        {
            CoreComponents.Add(component);
        }
    }

    public T GetCoreComponent<T>() where T: CoreComponent
    {
        T component = CoreComponents.OfType<T>().FirstOrDefault();
    
        if (component) return component;

        component = GetComponentInChildren<T>(); //Manually set the reference for the core if the core does not have a reference yet due to the referred component not being awake

        if (component) return component;
        
        Debug.LogWarning($"{typeof(T)} not found on {transform.parent.name}");

        return null;
    }

    public T GetCoreComponent<T>(ref T value) where T: CoreComponent //Alternate verision that immediately assigns the retrived component to the reference passed in
    {
        value = GetCoreComponent<T>();
        return value;
    }
}
