using System;
using UnityEngine;

namespace Gamecells.Weapons.Components.ComponentData.AttackData
{
    [Serializable]
    public class AttackSprites
    {
        [field: SerializeField] public Sprite[] Sprites { get; private set; }
    }
}
