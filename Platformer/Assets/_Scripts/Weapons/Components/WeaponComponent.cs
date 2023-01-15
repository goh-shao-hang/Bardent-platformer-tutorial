using Gamecells.CoreSystem;
using UnityEngine;

namespace Gamecells.Weapons.Components
{
    public abstract class WeaponComponent : MonoBehaviour
    {
        protected Weapon weapon;
        protected AnimationEventHandler eventHandler;
        //TODO: Fix this when finishing weapon data
        //protected AnimationEventHandler EventHandler => weapon.EventHandler;
        protected Core Core => weapon.Core;

        protected bool isAttackActive;

        protected virtual void Awake()
        {
            weapon = GetComponent<Weapon>();

            eventHandler = GetComponentInChildren<AnimationEventHandler>();
        }

        protected virtual void Start()
        {
            
        }

        protected virtual void HandleEnter()
        {
            isAttackActive = true;
        }

        protected virtual void HandleExit()
        {
            isAttackActive = false;
        }

        protected virtual void OnEnable()
        {
            weapon.OnEnter += HandleEnter;
            weapon.OnExit += HandleExit;
        }

        protected virtual void OnDisable()
        {
            weapon.OnEnter -= HandleEnter;
            weapon.OnExit -= HandleExit;
        }
    }

    public abstract class WeaponComponent<T1, T2> : WeaponComponent where T1 : ComponentData<T2> where T2 : AttackData
    {
        protected T1 componentData;
        protected T2 currentAttackData;

        protected override void Awake()
        {
            base.Awake();

            componentData = weapon.Data.GetData<T1>();
        }

        protected override void HandleEnter()
        {
            base.HandleEnter();

            currentAttackData = componentData.AttackData[weapon.CurrentAttackCounter];
        }
    }
}
