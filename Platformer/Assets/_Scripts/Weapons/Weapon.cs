using System;
using UnityEngine;
using Gamecells.Utilities;

namespace Gamecells.Weapons
{
    public class Weapon : MonoBehaviour
    {
        private Animator anim;
        private GameObject baseGO;
        private AnimationEventHandler eventHandler;
        private Timer attackCounterResetTimer;

        public event Action OnExit;
        
        private static readonly int activeHash = Animator.StringToHash("active");
        private static readonly int counterHash = Animator.StringToHash("counter");

        [SerializeField] private int numberOfAttacks = 3;
        [SerializeField] private float attackCounterResetCooldown = 2f;
        
        private int currentAttackCounter;

        public int CurrentAttackCounter
        {
            get => currentAttackCounter;
            private set => currentAttackCounter = value >= numberOfAttacks ? 0 : value;
        }

        private void Awake()
        {
            baseGO = transform.GetChild(0).gameObject;
            anim = baseGO.GetComponent<Animator>();
            eventHandler = baseGO.GetComponent<AnimationEventHandler>();
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

