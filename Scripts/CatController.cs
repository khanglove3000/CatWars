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
    [SerializeField] float catSpeed = 1f;
    [SerializeField] Animator catAnimator;
    public SpriteRenderer spriteRenderer;

    public CatType catType;
    IEnumerator actionCat;

    [Header("Attack")]
    public int amountDamage = 5;
    public CatController catTarget = null;
    public ShopCat homeTarget = null;


    [Header("Splines")]
    public SplineController spline;

    [SerializeField] protected Transform positionFlyDeadCat;

    private bool isCatAttacked = false;

    private void Start()
    {
        catCurrentHealth = catMaxHealth;
        CatHealthBar.SetHealth(catCurrentHealth, catMaxHealth, isCatAttacked);
    }

    public void CatGetDamage(int amount)
    {
        ActiveAnimationHit();
        catCurrentHealth -= amount;
        isCatAttacked = true;
        CatHealthBar.SetHealth(catCurrentHealth, catMaxHealth, isCatAttacked);

        DamagePopup.Create(hitPoint.position, amount);
        
        if (catCurrentHealth <= 0) isCatDead = true;
        if(isCatDead)
        {
            CatHealthBar.gameObject.SetActive(false);
            FlyingCatDead();
            StartCoroutine(CountTimeForDie());
        }
    }

    IEnumerator CountTimeForDie()
    {
        yield return new WaitForSeconds(0.5f);
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
        //transform.position = spline.GetPositionGo(this, speedWalk, catType);
        //Vector2 _vt2 = new Vector2();
        while (true) {
            //if (catType.ToString() == "Me")
            //{
            //    _vt2 = Vector2.MoveTowards(transform.position, spline.pointB.position, speedWalk / 100);

            //}
            //if (catType.ToString() == "Player")
            //{
            //    _vt2 = Vector2.MoveTowards(transform.position, spline.pointA.position, speedWalk / 100);
            //}
            //transform.position = new Vector2(_vt2.x, transform.position.y);

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
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (catTarget == null) {
            CatWalk();
        }
    }
    public void CatAttack()
    {
        StopCatWalk();
        if (catTarget)
        {
            ActiveAnimationAttack();
            CatGetDamage(amountDamage);
        }

        if (homeTarget)
        {
            ActiveAnimationAttack();
            homeTarget.TakeDamageHome(amountDamage);
        }
    }

    public void FlyingCatDead()
    {
        LeanTween.moveX(catController.gameObject, positionFlyDeadCat.position.x, 0.2f).setEase(LeanTweenType.easeInQuad);
        //idTweenMainContainer = LeanTween.moveX(catController.gameObject, positionFlyDeadBody.position.x, 0.2f).setEase(LeanTweenType.easeInBack).setOnComplete(() => {}).id;

    }
}