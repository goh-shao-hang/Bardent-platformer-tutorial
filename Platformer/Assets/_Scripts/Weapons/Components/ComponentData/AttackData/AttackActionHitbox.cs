using System;
using UnityEngine;

namespace Gamecells.Weapons.Components
{
    [Serializable]
    public class AttackActionHitbox : AttackData
    {
        public bool Debug;

        [field: SerializeField] public Rect Hitbox { get; private set; }
    }
}
