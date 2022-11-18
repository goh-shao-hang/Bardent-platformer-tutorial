using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [Header("Base Data")]
    public D_Enemy enemyData;

    public AnimationToStateMachine atsm { get; private set; }

    [Header("Assignables")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private Transform groundCheck;
    
    public int lastDamageDirection { get; private set; }

    private float currentHealth;
    private float currentStunResistance;
    private float lastDamageTime;
    

    protected bool isStunned;
    protected bool isDead;

    protected override void Awake()
    {
        base.Awake();

        atsm = GetComponent<AnimationToStateMachine>();
    }

    protected override void Start()
    {
        base.Start();

        currentHealth = enemyData.maxHealth;
        currentStunResistance = enemyData.stunResistance;
    }

    protected override void Update()
    {
        base.Update();

        if (Time.time >= lastDamageTime + enemyData.stunRecoveryTime)
        {
            ResetStunResistance();

            anim.SetFloat("yVelocity", Core.Movement.rb.velocity.y);
        }
    }

    public virtual bool CheckPlayerInMinAggroRange()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, enemyData.minAggroDistance, enemyData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInMaxAggroRange()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, enemyData.maxAggroDistance, enemyData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, enemyData.closeRangeActionDistance, enemyData.whatIsPlayer);
    }

    public virtual void TakeDamage(AttackDetails attackDetails)
    {
        lastDamageTime = Time.time;
        currentHealth -= attackDetails.damageAmount;
        currentStunResistance -= attackDetails.stunDamageAmount;

        //Effects
        DamageHop(enemyData.damageHopSpeed);
        Instantiate(enemyData.hitParticles, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));

        if (currentHealth <= 0)
        {
            isDead = true;
            return;
        }

        if (attackDetails.position.x > transform.position.x) //attack came from right, so knockback towards left
            lastDamageDirection = -1;
        else
            lastDamageDirection = 1;

        if (currentStunResistance <= 0)
            isStunned = true;
    }
    public virtual void ResetStunResistance()
    {
        isStunned = false;
        currentStunResistance = enemyData.stunResistance;
    }

    public virtual void DamageHop(float yVelocity)
    {
        Core.Movement.rb.velocity = new Vector2(Core.Movement.rb.velocity.x, yVelocity);
    }

    public virtual void OnDrawGizmos()
    {
        if (Core == null) return;

        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * Core.Movement.FacingDirection * enemyData.wallCheckDistance));
        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * enemyData.ledgeCheckDistance));
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * Core.Movement.FacingDirection * enemyData.closeRangeActionDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * Core.Movement.FacingDirection * enemyData.minAggroDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * Core.Movement.FacingDirection * enemyData.maxAggroDistance), 0.2f);
    }
}
