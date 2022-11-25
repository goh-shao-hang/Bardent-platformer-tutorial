using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Utilities;

public class Core : MonoBehaviour
{
    private readonly List<CoreComponent> coreComponents = new List<CoreComponent>();

    private void Awake()
    {

    }

    public void LogicUpdate()
    {
        foreach (CoreComponent component in coreComponents)
        {
            component.LogicUpdate();
        }
    }

    public void AddComponent(CoreComponent component)
    {
        if (!coreComponents.Contains(component))
        {
            coreComponents.Add(component);
        }
    }

    public T GetCoreComponent<T>() where T: CoreComponent
    {
        T component = coreComponents.OfType<T>().FirstOrDefault();

        if (component == null)
        {
            Debug.LogWarning($"{typeof(T)} not found on {transform.parent.name}");
        }
        return component;
    }

    public T GetCoreComponent<T>(ref T value) where T: CoreComponent //Alternate verision that immediately assigns the retrived component to the reference passed in
    {
        value = GetCoreComponent<T>();
        return value;
    }
}
