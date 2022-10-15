using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public FiniteStateMachine stateMachine;
    [Header("Base Data")]
    public D_Entity entityData;

    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    public GameObject baseGO { get; private set; }
    public AnimationToStateMachine atsm { get; private set; }

    [Header("Assignables")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private Transform groundCheck;
    public int facingDirection { get; private set; }
    public int lastDamageDirection { get; private set; }

    private float currentHealth;
    private float currentStunResistance;
    private float lastDamageTime;
    private Vector2 velocityWorkspace; //used to set values without creating new vector2

    protected bool isStunned;
    protected bool isDead;

    protected virtual void Start()
    {
        baseGO = transform.Find("Base").gameObject;
        rb = baseGO.GetComponent<Rigidbody2D>();
        anim = baseGO.GetComponent<Animator>();
        atsm = baseGO.GetComponent<AnimationToStateMachine>();

        stateMachine = new FiniteStateMachine();

        currentHealth = entityData.maxHealth;
        currentStunResistance = entityData.stunResistance;
        facingDirection = 1;
    }

    protected virtual void Update()
    {
        stateMachine.currentState.LogicUpdate();

        if (Time.time >= lastDamageTime + entityData.stunRecoveryTime)
        {
            ResetStunResistance();

            anim.SetFloat("yVelocity", rb.velocity.y);
        }
    }
    
    protected virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    public virtual void SetVelocity(float velocity)
    {
        velocityWorkspace.Set(facingDirection * velocity, rb.velocity.y);
        rb.velocity = velocityWorkspace;
    }
    
    public virtual void SetVelocity(float velocity, Vector2 angle, int direction) //alternate set velocity function
    {
        angle.Normalize();
        velocityWorkspace.Set(angle.x * velocity * direction, angle.y * velocity); //Direction multiplied only on x axis to flip knockback horizontally only
        rb.velocity = velocityWorkspace;
    }

    public virtual bool CheckWall()
    {
        return Physics2D.Raycast(wallCheck.position, baseGO.transform.right, entityData.wallCheckDistance, entityData.whatIsGround);
    }

    public virtual bool CheckLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.down, entityData.ledgeCheckDistance, entityData.whatIsGround);
    }

    public virtual bool CheckGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, entityData.groundCheckRadius, entityData.whatIsGround);
    }

    public virtual bool CheckPlayerInMinAggroRange()
    {
        return Physics2D.Raycast(playerCheck.position, baseGO.transform.right, entityData.minAggroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInMaxAggroRange()
    {
        return Physics2D.Raycast(playerCheck.position, baseGO.transform.right, entityData.maxAggroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheck.position, baseGO.transform.right, entityData.closeRangeActionDistance, entityData.whatIsPlayer);
    }

    public virtual void TakeDamage(AttackDetails attackDetails)
    {
        lastDamageTime = Time.time;
        currentHealth -= attackDetails.damageAmount;
        currentStunResistance -= attackDetails.stunDamageAmount;

        //Effects
        DamageHop(entityData.damageHopSpeed);
        Instantiate(entityData.hitParticles, baseGO.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));

        if (currentHealth <= 0)
        {
            isDead = true;
            return;
        }

        if (attackDetails.position.x > baseGO.transform.position.x) //attack came from right, so knockback towards left
            lastDamageDirection = -1;
        else
            lastDamageDirection = 1;

        if (currentStunResistance <= 0)
            isStunned = true;
    }
    public virtual void ResetStunResistance()
    {
        isStunned = false;
        currentStunResistance = entityData.stunResistance;
    }

    public virtual void DamageHop(float velocity)
    {
        velocityWorkspace.Set(rb.velocity.x, velocity);
        rb.velocity = velocityWorkspace;
    }
    public virtual void Flip()
    {
        facingDirection *= -1;
        baseGO.transform.Rotate(0f, 180f, 0f);
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.wallCheckDistance));
        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * entityData.ledgeCheckDistance));
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.closeRangeActionDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.minAggroDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.maxAggroDistance), 0.2f);
    }
}
