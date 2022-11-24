using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class Core : MonoBehaviour
{
    private Movement movement;
    private CollisionSenses collisionSenses;
    private Combat combat;

    public Movement Movement => GenericNotImplementedError.TryGet(movement, transform.parent.name);
    public CollisionSenses CollisionSenses => GenericNotImplementedError.TryGet(collisionSenses, transform.parent.name);
    public Combat Combat => GenericNotImplementedError.TryGet(combat, transform.parent.name);

    private void Awake()
    {
        movement = GetComponentInChildren<Movement>();
        collisionSenses = GetComponentInChildren<CollisionSenses>();
        combat = GetComponentInChildren<Combat>();
    }

    public void LogicUpdate()
    {
        Movement.LogicUpdate();
    }
}
