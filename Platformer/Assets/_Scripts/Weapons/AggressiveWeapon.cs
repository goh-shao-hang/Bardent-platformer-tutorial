using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AggressiveWeapon : Weapon
{
    private Movement movement;

    protected Movement Movement => movement ??= core.GetCoreComponent<Movement>();

    protected SO_AggressiveWeaponData aggressiveWeaponData;

    private List<IDamageable> detectedDamageables = new List<IDamageable>();
    private List<IKnockbackable> detectedKnockbackables = new List<IKnockbackable>();

    protected override void Awake()
    {
        base.Awake();

        if (weaponData.GetType() == typeof(SO_AggressiveWeaponData))
        {
            aggressiveWeaponData = (SO_AggressiveWeaponData)weaponData;
        }
        else
        {
            Debug.LogError("Wrong data for the weapon!");
        }
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        CheckMeleeAttack();
    } 

    private void CheckMeleeAttack()
    {
        WeaponAttackDetails details = aggressiveWeaponData.AttackDetails[attackCounter];

        foreach (IDamageable damageable in detectedDamageables.ToList()) //ToList() creates a copy of the list to loop through so that when we modify the original list (eg. removing a dead enemy), the list used to perform this execution is unaffected and prevents any errors.
        {
            damageable.TakeDamage(details.damageAmount);
        }

        foreach (IKnockbackable knockbackable in detectedKnockbackables.ToList())
        {
            knockbackable.Knockback(details.knockbackStrength, details.knockbackAngle, Movement.FacingDirection);
        }
    }

    public void AddToDetected(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamageable damageable))
        {
            detectedDamageables.Add(damageable);
        }

        if (collision.TryGetComponent(out IKnockbackable knockbackable))
        {
            detectedKnockbackables.Add(knockbackable);
        }
    }

    public void RemoveFromDetected(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamageable damageable))
        {
            detectedDamageables.Remove(damageable);
        }

        if (collision.TryGetComponent(out IKnockbackable knockbackable))
        {
            detectedKnockbackables.Remove(knockbackable);
        }
    }
}
