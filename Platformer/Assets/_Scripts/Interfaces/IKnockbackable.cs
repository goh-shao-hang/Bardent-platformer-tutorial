using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKnockBackable
{
    void KnockBack(float strength, Vector2 angle, int direction);
}
