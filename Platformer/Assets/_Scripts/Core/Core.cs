using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class Core : MonoBehaviour
{
    private Movement movement;
    private CollisionSenses collisionSenses;
    private Combat combat;
    private Stats stats;

    private List<ILogicUpdate> components = new List<ILogicUpdate>();

    public Movement Movement => GenericNotImplementedError.TryGet(movement, transform.parent.name);
    public CollisionSenses CollisionSenses => GenericNotImplementedError.TryGet(collisionSenses, transform.parent.name);
    public Combat Combat => GenericNotImplementedError.TryGet(combat, transform.parent.name);
    public Stats Stats => GenericNotImplementedError.TryGet(stats, transform.parent.name);

    private void Awake()
    {
        movement = GetComponentInChildren<Movement>();
        collisionSenses = GetComponentInChildren<CollisionSenses>();
        combat = GetComponentInChildren<Combat>();
        stats = GetComponentInChildren<Stats>();
    }

    public void LogicUpdate()
    {
        foreach (ILogicUpdate component in components)
        {
            component.LogicUpdate();
        }
    }

    public void AddComponent(ILogicUpdate component)
    {
        if (!components.Contains(component))
        {
            components.Add(component);
        }
    }
}
