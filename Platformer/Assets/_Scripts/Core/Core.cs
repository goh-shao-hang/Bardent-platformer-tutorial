using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    private Movement movement;
    private CollisionSenses collisionSenses;

    public Movement Movement
    {
        get
        {
            if (movement)
            {
                return movement;
            }
            else
            {
                Debug.LogError("No Movement Core Component on" + transform.parent.name);
                return null;
            }
        }

        private set { movement = value; }
    }

    public CollisionSenses CollisionSenses
    {
        get
        {
            if (collisionSenses)
            {
                return collisionSenses;
            }
            else
            {
                Debug.LogError("No Collision Senses Core Component on" + transform.parent.name);
                return null;
            }
        }

        private set { collisionSenses = value; }
    }

    private void Awake()
    {
        Movement = GetComponentInChildren<Movement>();
        CollisionSenses = GetComponentInChildren<CollisionSenses>();
    }

    public void LogicUpdate()
    {
        Movement.LogicUpdate();
    }
}
