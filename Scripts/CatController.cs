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
    private float startTime;

    private void Start()
    {
        startTime = Time.time;
        currentHealth = maxHealth;
        healthBarBehaive.SetHealth(currentHealth, maxHealth);
    }

    private void Update()
    {
        Walk();
    }
    public void TakeDamage(int amount)
    {
        ActiveAnimationHit();
        currentHealth -= amount;
        healthBarBehaive.SetHealth(currentHealth, maxHealth);

        //healthBar.fillAmount = currentHealth / maxHealth;
        DamagePopup.Create(hitPoint.position, currentHealth);
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
            spriteRenderer.flipX = true;
            _toward = Vector3.left;
        }
        ActiveAnimationWalk();
        transform.position = spline.GetPositionGo(speedWalk, startTime);
        //while (true)
        //{
        //    transform.Translate(_toward * speedWalk * Time.deltaTime);
        //    //speedWalk = (speedWalk + Time.deltaTime) % 0.1f;
        //    //transform.position = spline.GetPositionGo(speedWalk);
            yield return null;
        //}
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Cat")
        {
            CatController _cat = collision.gameObject.GetComponent<CatController>();
            if (catType != _cat.catType)
            {
                catTarget = _cat;
                //StopRun();
                //ToAttack();
            }
        }
        else if (collision.gameObject.tag == "Home") { 
            ShopCat _shopCat = collision.gameObject.GetComponent<ShopCat>();
            if (catType != _shopCat.CatType)
            {
                homeTarget = _shopCat;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (catTarget == null && homeTarget == null) {
            Walk();
        }
    }
    void ToAttack()
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