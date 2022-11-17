using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cat_Enum;

public class Cat_Controller : MonoBehaviour
{
    [Header("Health")]
    public int catCurrentHealth;
    public int catMaxHealth = 100;
    public Cat_HealthBar CatHealthBar;
    public bool isCatDead = false;

    [Header("Movement")]
    public Cat_Controller catController;
    public float catSpeed = 1f;
    [SerializeField] Animator catAnimator;
    public SpriteRenderer spriteRenderer;

    public CatType catType;
    public IEnumerator actionCat;
    public IEnumerator actionCatFlying;

    [Header("Attack")]
    public int amountDamage = 5;
    public Cat_Controller catTarget = null;
    public Cat_Shop homeTarget = null;
    public bool isCatAttacked = false;
    public int amountCatAttacked = 0;
    public float catAttackSpeed;
    
    [Header("Splines")]
    public Cat_SplineController spline;


    [Header("Fly Cat")]
    public float speedFlyDeadCat = 10f;
    public float distanceCatFlying = 5f;
    public float moveTowers;
    public Vector3 targetPosFlyDeadCat;
    public bool isFlyingBody = false;

    public Color tmp;

    private void Start()
    {
        catCurrentHealth = catMaxHealth;
        CatHealthBar.SetHealth(catCurrentHealth, catMaxHealth, isCatAttacked);  
    }
    [Button]
    public void PressStop()
    {
        StopCatWalk();
    }

    public void CatGetDamage(Cat_Controller _catTarget, int amount)
    {
        if (_catTarget.isCatDead == true) return;
        // Dùng để kích hoạt flying body
        _catTarget.amountCatAttacked++;
        if (_catTarget.amountCatAttacked > 5)
        {
            _catTarget.amountCatAttacked = 0;
            _catTarget.CatFlyingBody(_catTarget.isCatDead);
        }

        // dùng để nhận damage, trừ máu, hiện thanh máu, hiện chỉ số damage
  
        _catTarget.catCurrentHealth -= amount;
        _catTarget.isCatAttacked = true;
        _catTarget.CatHealthBar.SetHealth(_catTarget.catCurrentHealth, _catTarget.catMaxHealth, _catTarget.isCatAttacked);
        Cat_DamagePopup.Create(_catTarget.transform.position, amount, catType);
      
        // khi cat dead thi bị đẩy lùi, và die
        if (_catTarget.catCurrentHealth <= 0)
        {
          
            _catTarget.isCatDead = true;
            _catTarget.CatHealthBar.gameObject.SetActive(false);
            _catTarget.CatFlyingBody(_catTarget.isCatDead);
        }
    }

    public void CatDead(Cat_Controller _catTarget)
    {
        Destroy(_catTarget.gameObject, 0.7f);
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
    public virtual void CatAttack()
    {
        StopCatWalk();

        if (homeTarget)
        {
            ActiveAnimationAttack();
            homeTarget.HomeGetDamage(amountDamage);
        }

        if (catTarget == null) return;
        if (catTarget.isCatDead == true) catTarget = null;
        if (catTarget)
        {
            ActiveAnimationAttack();
            CatGetDamage(catTarget, amountDamage);
        }

    }

    public void CatFlyingBody(bool _isCatDead)
    {

        if (catType.ToString() == "Me") moveTowers = -distanceCatFlying;
        if (catType.ToString() == "Player") moveTowers = distanceCatFlying;
        targetPosFlyDeadCat = new Vector3(transform.position.x + moveTowers, transform.position.y);
        this.StopCatWalk();
        float _temp = this.catSpeed;
        this.catSpeed = 0;
        LeanTween.move(gameObject, targetPosFlyDeadCat, 0.2f).setOnComplete(()=> {
            
            if (!_isCatDead)
            {
                this.catSpeed = _temp;
                this.CatWalk();
            }

            if (_isCatDead)
            {
                this.StopCatWalk();
                tmp = spriteRenderer.color;
                LeanTween.value(gameObject, 1, 0, 0.5f).setOnUpdate((float val) => {
                    tmp = new Color(tmp.r, tmp.g, tmp.b, val);
                    spriteRenderer.color = tmp;
                });
                CatDead(this);
            }
        });
    }
}