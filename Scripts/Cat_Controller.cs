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
    public float catWalkSpeed = 1f;
    public Animator catAnimator;

    public SpriteRenderer spriteRenderer;

    public CatType catType;
    public IEnumerator actionCat;

    [Header("Attack")]
    public int amountDamage = 5;
    public Cat_Controller catTarget = null;
    public Cat_Shop homeTarget = null;
    public bool isCatAttacked = false;
    public int amountCatAttacked = 0;
    public float catAttackSpeed = 1f;
    public int catDurationAttack = 1;

    [Header("Splines")]
    public Cat_SplineController spline;

    [Header("Fly Cat")]
    public float speedFlyDeadCat = 10f;
    public float distanceCatFlying = 5f;
    public float moveTowers;
    public Vector3 targetPosFlyDeadCat;
    public bool isFlyingBody = false;

    [Header("Change body color")]
    public Color tmp;

    [Button]
    public void PressToStop()
    {
        StopCatWalk();
    }
    [Button]
    public void PressToAttack()
    {
       StartCoroutine(AutoAttack());
    }

    private void Start()
    {
        catCurrentHealth = catMaxHealth;
        CatHealthBar.SetHealth(catCurrentHealth, catMaxHealth, isCatAttacked);  
    }
    public void CatGetDamage(int amount)
    {
        if (isCatDead == true) return;
        // Dùng để kích hoạt flying body
        amountCatAttacked++;
        if (amountCatAttacked > 5)
        {
            amountCatAttacked = 0;
            CatFlyingBody(isCatDead);
        }

        // dùng để nhận damage, trừ máu, hiện thanh máu, hiện chỉ số damage
  
        catCurrentHealth -= amount;
        isCatAttacked = true;
        CatHealthBar.SetHealth(catCurrentHealth,catMaxHealth, isCatAttacked);
        Cat_DamagePopup.Create(transform.position, amount, catType);
      
        // khi cat dead thi bị đẩy lùi, và die
        if (catCurrentHealth <= 0)
        {
          
            isCatDead = true;
            CatHealthBar.gameObject.SetActive(false);
            CatFlyingBody(isCatDead);
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
        if (catWalkSpeed == 0) catWalkSpeed = 1f;
        catAnimator.SetBool("Attack", false);
        catAnimator.SetFloat("Walk", catWalkSpeed);
        actionCat = CatMovement();
        StartCoroutine(actionCat);
    }
    IEnumerator CatMovement()
    {
        if (catType != CatType.Me)
            transform.rotation = Quaternion.Euler(0, 180, 0);
       
        while (true) {
            transform.position = spline.GetPositionGo(this, catWalkSpeed, catType);
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
        catWalkSpeed = 0;
        catAnimator.SetFloat("Walk", catWalkSpeed);
    } 

    public IEnumerator AutoAttack()
    {
        while (true)
        {
            //catAnimator.SetFloat("Attack", catAttackSpeed);
            catAnimator.SetBool("Attack", true);
            catAnimator.speed = (catAttackSpeed < 1) ? 1 : catAttackSpeed;
            float _lengthAnim = (catAttackSpeed <= 1) ? catAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length / catAnimator.GetCurrentAnimatorStateInfo(0).speed
                       : (catAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length / catAttackSpeed) / catAnimator.GetCurrentAnimatorStateInfo(0).speed;
            //Debug.LogError("_lengthAnim: " + _lengthAnim);
            yield return new WaitForSeconds(_lengthAnim);
            catAnimator.SetFloat("Attack", -1);
            float waitTime = (catDurationAttack / catAttackSpeed) - _lengthAnim;
            yield return new WaitForSeconds(waitTime);
            //Debug.LogError("waitTime: " + waitTime);
            yield return null;

        }
    }

    // Cat to attack
    public virtual void CatAttack()
    {
        StopCatWalk();
        if (homeTarget)
        {
            catAnimator.SetBool("Attack", true);
            homeTarget.HomeGetDamage(amountDamage);
        }
        if (catTarget == null) return;
        if (catTarget.isCatDead == true) catTarget = null;
        if (catTarget)
        {
            catAnimator.SetBool("Attack", true);
            catTarget.CatGetDamage(amountDamage);
        }
    }
    // Cat flying body when cat die or attacked > 3
    public void CatFlyingBody(bool _isCatDead)
    {

        if (catType.ToString() == "Me") moveTowers = -distanceCatFlying;
        if (catType.ToString() == "Player") moveTowers = distanceCatFlying;
        targetPosFlyDeadCat = new Vector3(transform.position.x + moveTowers, transform.position.y);
        this.StopCatWalk();
        float _temp = this.catWalkSpeed;
        this.catWalkSpeed = 0;
        LeanTween.move(gameObject, targetPosFlyDeadCat, 0.2f).setOnComplete(()=> {
            
            if (!_isCatDead)
            {
                this.catWalkSpeed = _temp;
                this.CatWalk();
                Debug.Log("Cat flying body - Cat walk");
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