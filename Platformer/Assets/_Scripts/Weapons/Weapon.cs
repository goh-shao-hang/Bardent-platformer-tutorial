using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected SO_WeaponData weaponData;

    protected PlayerAttackState state;
    protected Animator baseAnimator;
    protected Animator weaponAnimator;

    private int attackHash = Animator.StringToHash("attack");
    private int attackCounterHash = Animator.StringToHash("attackCounter");

    protected int attackCounter;

    protected virtual void Awake()
    {
        Animator[] animators = GetComponentsInChildren<Animator>();
        baseAnimator = animators[0];
        weaponAnimator = animators[1];

        gameObject.SetActive(false);
    }

    public virtual void EnterWeapon()
    {
        gameObject.SetActive(true);

        baseAnimator.SetBool(attackHash, true);
        weaponAnimator.SetBool(attackHash, true);

        baseAnimator.SetInteger(attackCounterHash, attackCounter);
        weaponAnimator.SetInteger(attackCounterHash, attackCounter);
    }

    public virtual void ExitWeapon()
    {
        baseAnimator.SetBool(attackHash, false);
        weaponAnimator.SetBool(attackHash, false);

        attackCounter = (attackCounter + 1) % weaponData.amountOfAttacks;

        gameObject.SetActive(true);
    }

    public void InitializeWeapon(PlayerAttackState state)
    {
        this.state = state;
    }

    #region Animation Triggers

    public virtual void AnimationStartMovementTrigger()
    {
        state.SetPlayerVelocity(weaponData.movementSpeed[attackCounter]);
    }

    public virtual void AnimationStopMovementTrigger()
    {
        state.SetPlayerVelocity(0f);
    }

    public virtual void AnimationTurnOffFlipTrigger()
    {
        state.SetFlipCheck(false);
    }

    public virtual void AnimationTurnOnFlipTrigger()
    {
        state.SetFlipCheck(true);
    }

    public virtual void AnimationFinishTrigger()
    {
        state.AnimationFinishTrigger();
    }

    public virtual void AnimationActionTrigger()
    {

    }

    #endregion
}
