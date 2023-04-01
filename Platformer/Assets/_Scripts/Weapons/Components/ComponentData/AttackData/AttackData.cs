using System;
using UnityEngine;

namespace Gamecells.Weapons.Components
{
    [Serializable]
    public class AttackData
    {
        [SerializeField, HideInInspector] private string name;

        public AttackData()
        {

        }

        public void SetAttackName(int i) => name = $"Attack {i}";
    }
}
