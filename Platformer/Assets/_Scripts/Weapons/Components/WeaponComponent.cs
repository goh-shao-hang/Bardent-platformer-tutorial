using Gamecells.CoreSystem;
using UnityEngine;

namespace Gamecells.Weapons.Components
{
    public abstract class WeaponComponent : MonoBehaviour
    {
        protected Weapon weapon;
        protected AnimationEventHandler animationEventHandler;
        //TODO: Fix this when finishing weapon data
        //protected AnimationEventHandler EventHandler => weapon.EventHandler;
        protected Core Core => weapon.Core;

        protected bool isAttackActive;

        public virtual void Init()
        {

        }

        protected virtual void Awake()
        {
            weapon = GetComponent<Weapon>();
            animationEventHandler = GetComponentInChildren<AnimationEventHandler>();
        }

        protected virtual void Start()
        {
            //Subscribing in Start instead of OnEnable because OnEnable is called before Start, but WeaponGenerator currently initializes weapon on Start
            //Thus, subscribing in OnEnable can cause sequencing issues
            weapon.OnEnter += HandleEnter;
            weapon.OnExit += HandleExit;
        }

        protected virtual void HandleEnter()
        {
            isAttackActive = true;
        }

        protected virtual void HandleExit()
        {
            isAttackActive = false;
        }

        protected virtual void OnDestroy()
        {
            weapon.OnEnter -= HandleEnter;
            weapon.OnExit -= HandleExit;
        }
    }

    public abstract class WeaponComponent<T1, T2> : WeaponComponent where T1 : ComponentData<T2> where T2 : AttackData
    {
        protected T1 componentData;
        protected T2 currentAttackData;

        public override void Init()
        {
            base.Init();

            componentData = weapon.WeaponData.GetData<T1>();
        }

        protected override void HandleEnter()
        {
            base.HandleEnter();

            currentAttackData = componentData.AttackData[weapon.CurrentAttackCounter];
        }
    }
}
