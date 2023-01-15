using System;
using UnityEngine;

namespace Gamecells.Weapons.Components
{
    [Serializable]
    public class AttackSprites : AttackData
    {
        [field: SerializeField] public Sprite[] Sprites { get; private set; }
    }
}
