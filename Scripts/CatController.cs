using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static CatWarEnum;

public class CatController : MonoBehaviour
{
    [Header("Health")]
    public int currentHealth;
    public int maxHealth = 100;
    public HealthBar healthBarBehaive;
    public Transform hitPoint;
    public bool isDead = false;

    [Header("Movement")]
    public CatController catController;
    [SerializeField] float speedWalk = 1f;
    [SerializeField] Animator animator;
    public SpriteRenderer spriteRenderer;

    public CatType catType;
    IEnumerator actionCat;

    [Header("Attack")]
    public int attackDamage = 5;
    public CatController catTarget;
    public ShopCat homeTarget;


    [Header("Splines")]
    public SplineController spline;


    private bool attacked = false;
    private void Start()
    {
        currentHealth = maxHealth;
        
        healthBarBehaive.SetHealth(currentHealth, maxHealth, attacked);

    }

    private void Update()
    {
        if(catTarget || homeTarget)
        {
            StopRun();
           
        }
        else
        {
            Walk(); 
        }
       
    }
    public void TakeDamage(int amount)
    {
        ActiveAnimationHit();
        currentHealth -= amount;
        attacked = true;
        healthBarBehaive.SetHealth(currentHealth, maxHealth, attacked);

        DamagePopup.Create(hitPoint.position, amount);
        
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }


    private void Die()
    {
        isDead = true;
        Destroy(gameObject);
    }
    public void Walk()
    {
        if (actionCat != null)
        {
            StopCoroutine(actionCat);
            actionCat = null;
        }
        actionCat = Movement();
        StartCoroutine(actionCat);
    }

    IEnumerator Movement()
    {
        Vector3 _toward = new Vector3();
        if (catType == CatType.Me)
        {
            _toward = Vector3.right;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            _toward = Vector3.left;
        }
        ActiveAnimationWalk();
        transform.position = spline.GetPositionGo(this, speedWalk, catType);
        yield return null;

    }

    public void StopRun()
    {
        if (actionCat != null)
        {
            StopCoroutine(actionCat);
            actionCat = null;
        }
        animator.SetFloat("Run", -1);

    }
    public void ActiveAnimationWalk()
    {
        if (actionCat != null)
        {
            StopCoroutine(actionCat);
            actionCat = null;
        }
        animator.SetFloat("Run", 1);
    }

    public void ActiveAnimationAttack()
    {
        if (actionCat != null)
        {
            StopCoroutine(actionCat);
            actionCat = null;
        }

        animator.SetTrigger("Attack");
        //animator.SetInteger("AttackByInt", 1);
    }

    public void ActiveAnimationHit()
    {
        if (actionCat != null)
        {
            StopCoroutine(actionCat);
            actionCat = null;
        }
        animator.SetTrigger("Hit");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (catTarget == null && homeTarget == null) {
            Walk();
           
        }
    }
    public void ToAttack()
    {
        if (catTarget)
        {
            ActiveAnimationAttack();
            TakeDamage(attackDamage);
        }

        if (homeTarget)
        {
            ActiveAnimationAttack();
            homeTarget.TakeDamageHome(attackDamage);
        }
    }


}