using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamecells.Utilities;

public class CombatTestDummy : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject hitParticles;

    private Animator anim;
    private SpriteRenderer sr;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float amount)
    {
        Debug.Log(amount + "damage taken");

        Destroy(Instantiate(hitParticles, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f))), 3f);
        anim.SetTrigger("damage");
        CameraManager.CameraShake(0.15f);
        //Destroy(gameObject);
    }
}
