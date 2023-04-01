using System;
using UnityEngine;

namespace Gamecells.Weapons.Components
{
    [Serializable]
    public abstract class ComponentData
    {
        [SerializeField, HideInInspector] private string name;

        public Type ComponentDependency { get; protected set; }

        public ComponentData()
        {
            SetComponentName();
            SetComponentDependency();
        }

        public void SetComponentName() => name = GetType().Name;

        protected abstract void SetComponentDependency();

        public virtual void SetAttackDataNames() { }

        public virtual void InitializeAttackData(int numberOfAttacks) { }
    }

    public abstract class ComponentData<T> : ComponentData where T : AttackData
    {
        [SerializeField] private T[] attackData;
        public T[] AttackData { get => attackData; private set => attackData = value; } 

        public override void SetAttackDataNames()
        {
            base.SetAttackDataNames();

            for (int i = 0; i < AttackData.Length; i++)
            {
                AttackData[i].SetAttackName(i + 1);
            }
        }

        public override void InitializeAttackData(int numberOfAttacks)
        {
            base.InitializeAttackData(numberOfAttacks);

            int oldLength = attackData != null ? AttackData.Length : 0;

            if (oldLength == numberOfAttacks) return;

            Array.Resize(ref attackData, numberOfAttacks);

            if (oldLength < numberOfAttacks)
            {
                for (int i = oldLength; i < attackData.Length; i++)
                {
                    var newObj = Activator.CreateInstance(typeof(T)) as T;
                    attackData[i] = newObj;
                }
            }

            SetAttackDataNames();
        }
    }
}
