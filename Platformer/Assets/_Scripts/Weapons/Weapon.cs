using System;
using UnityEngine;
using Gamecells.Utilities;

namespace Gamecells.Weapons
{
    public class Weapon : MonoBehaviour
    {
        //References
        [field: SerializeField] public WeaponDataSO Data { get; private set; }
        public GameObject BaseGameObject { get; private set; }
        public GameObject WeaponSpriteGameObject { get; private set; }

        private Animator anim;
        private AnimationEventHandler eventHandler;
        private Timer attackCounterResetTimer;

        //Fields
        public event Action OnEnter;
        public event Action OnExit;
        
        private static readonly int activeHash = Animator.StringToHash("active");
        private static readonly int counterHash = Animator.StringToHash("counter");

        [SerializeField] private float attackCounterResetCooldown = 2f;
        
        private int currentAttackCounter;

        public int CurrentAttackCounter
        {
            get => currentAttackCounter;
            private set => currentAttackCounter = value >= Data.NumberOfAttacks ? 0 : value;
        }

        private void Awake()
        {
            BaseGameObject = transform.GetChild(0).gameObject;
            WeaponSpriteGameObject = transform.GetChild(1).gameObject;
            anim = BaseGameObject.GetComponent<Animator>();
            eventHandler = BaseGameObject.GetComponent<AnimationEventHandler>();
            attackCounterResetTimer = new Timer(attackCounterResetCooldown);
        }

        private void Update()
        {
            attackCounterResetTimer.Tick();
        }

        public void Enter()
        {
            print($"{transform.name} entered.");

            anim.SetBool(activeHash, true);
            anim.SetInteger(counterHash, CurrentAttackCounter);

            attackCounterResetTimer.StopTimer();

            OnEnter?.Invoke();
        }

        private void Exit()
        {
            anim.SetBool(activeHash, false);
            CurrentAttackCounter++;

            attackCounterResetTimer.StartTimer();

            OnExit?.Invoke();
        }

        private void OnEnable()
        {
            eventHandler.OnFinish += Exit;
            attackCounterResetTimer.OnTimerDone += ResetAttackCounter;
        }

        private void OnDisable()
        {
            eventHandler.OnFinish -= Exit;
            attackCounterResetTimer.OnTimerDone -= ResetAttackCounter;
        }

        private void ResetAttackCounter() => CurrentAttackCounter = 0;
    } 
}

