using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [Header("Base Data")]
    protected PlayerData playerData;

    protected override void Start()
    {
        base.Start();

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
}
