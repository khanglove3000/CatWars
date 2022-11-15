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
    public int catCurrentHealth;
    public int catMaxHealth = 100;
    public HealthBar CatHealthBar;
    public Transform hitPoint;
    public bool isCatDead = false;

    [Header("Movement")]
    public CatController catController;
    public float catSpeed = 1f;
    [SerializeField] Animator catAnimator;
    public SpriteRenderer spriteRenderer;

    public CatType catType;
    public IEnumerator actionCat;
    public IEnumerator actionCatFlying;

    [Header("Attack")]
    public int amountDamage = 5;
    public CatController catTarget = null;
    public ShopCat homeTarget = null;
    public bool isCatAttacked = false;
    public int amountCatAttacked = 0;
    
    [Header("Splines")]
    public SplineController spline;


    [Header("Fly Cat")]
    public float speedFlyDeadCat = 5.0f;
    public float distanceCatFlying = 7.0f;
    public float moveTowers;
    private Vector2 targetPosFlyDeadCat;
    

    private void Start()
    {
        catCurrentHealth = catMaxHealth;
        CatHealthBar.SetHealth(catCurrentHealth, catMaxHealth, isCatAttacked);  
    }

    public void CatGetDamage(CatController _catTarget, int amount)
    {
        if (actionCatFlying != null)
        {
            StopCoroutine(actionCatFlying);
            actionCatFlying = null;
        }
        actionCatFlying = _catTarget.CatFlying();

        _catTarget.ActiveAnimationHit();
        _catTarget.catCurrentHealth -= amount;
        _catTarget.isCatAttacked = true;
        _catTarget.CatHealthBar.SetHealth(_catTarget.catCurrentHealth, _catTarget.catMaxHealth, _catTarget.isCatAttacked);

        DamagePopup.Create(_catTarget.transform.position, amount, catType);
      
        _catTarget.amountCatAttacked++;
        if (_catTarget.amountCatAttacked > 3)
        {
            _catTarget.amountCatAttacked = 0;
            StartCoroutine(actionCatFlying);
        }

        if (_catTarget.catCurrentHealth <= 0) _catTarget.isCatDead = true;
        if (_catTarget.isCatDead)
        {
            _catTarget.CatHealthBar.gameObject.SetActive(false);
            StartCoroutine(_catTarget.CountTimeForDie());
        }
    }

    IEnumerator CountTimeForDie()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    public void CatWalk()
    {
        if (actionCat != null)
        {
            StopCoroutine(actionCat);
            actionCat = null;
        }
        catAnimator.SetBool("Attack", false);
        actionCat = CatMovement();
        StartCoroutine(actionCat);
       
    }

    IEnumerator CatMovement()
    {
        if (catType != CatType.Me)
            transform.rotation = Quaternion.Euler(0, 180, 0);

        ActiveAnimationWalk();

        while (true) {
            transform.position = spline.GetPositionGo(this, catSpeed, catType);
            yield return null;
        }
    }

    public void StopCatWalk()
    {
        if (actionCat != null)
        {
            StopCoroutine(actionCat);
            actionCat = null;
        }
        catAnimator.SetBool("Walk", false);
    }

    public void StopAction()
    {
        catAnimator.SetBool("Attack", false);
        catAnimator.SetBool("Walk", false);
    }
    public void ActiveAnimationWalk()
    {
        catAnimator.SetBool("Walk", true);
    }

    public void ActiveAnimationAttack()
    {
        if (actionCat != null)
        {
            StopCoroutine(actionCat);
            actionCat = null;
        }

        if(catTarget || homeTarget) catAnimator.SetBool("Attack", true);
    }

    public void ActiveAnimationHit()
    {
        if (actionCat != null)
        {
            StopCoroutine(actionCat);
            actionCat = null;
        }
        catAnimator.SetBool("Hit", true);
        StartCoroutine(CountForStopHit());
    }

    IEnumerator CountForStopHit()
    {
        yield return new WaitForSeconds(1f);
        catAnimator.SetBool("Hit", false);
    }

    public void CatAttack()
    {
        StopCatWalk();
        if (catTarget)
        {
            ActiveAnimationAttack();
            CatGetDamage(catTarget, amountDamage);
        }

        if (homeTarget)
        {
            ActiveAnimationAttack();
            homeTarget.TakeDamageHome(amountDamage);
        }
    }

    IEnumerator CatFlying()
    {
       
        if (catType.ToString() == "Me") moveTowers = -distanceCatFlying;
        if (catType.ToString() == "Player") moveTowers = distanceCatFlying;
       
        targetPosFlyDeadCat = new Vector2(transform.position.x + moveTowers, transform.position.y);
        while (true)
        {
            float step = speedFlyDeadCat * Time.deltaTime;

            transform.position = Vector2.MoveTowards(transform.position, targetPosFlyDeadCat, step);
            yield return null;
        }
    }

 
  
}