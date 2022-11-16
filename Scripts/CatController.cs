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
    public float speedFlyDeadCat = 10f;
    public float distanceCatFlying = 0.5f;
    public float moveTowers;
    private Vector3 targetPosFlyDeadCat;
    public bool isFlyingBody = false;

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
    //[Button]
    //public void PressDamage()
    //{
    //    Test_CatGetDamage();
    //}
    //public void Test_CatGetDamage()
    //{
    //    if (isCatDead == true) return;
    //    // Dùng để kích hoạt flying body
    //    amountCatAttacked++;
    //    if (amountCatAttacked > 3)
    //    {
    //        amountCatAttacked = 0;
    //        CatFlyingBody(isCatDead);
    //    }

    //    catCurrentHealth -= 1   ;
    //    isCatAttacked = true;
    //    CatHealthBar.SetHealth(catCurrentHealth, catMaxHealth, isCatAttacked);
    //    DamagePopup.Create(transform.position, 50, catType);

    //    // khi cat dead thi bị đẩy lùi, và die
    //    if (catCurrentHealth <= 0)
    //    {
    //        isCatDead = true;
    //        CatHealthBar.gameObject.SetActive(false);
    //        CatFlyingBody(isCatDead);
    //    }
    //}
    public void CatGetDamage(CatController _catTarget, int amount)
    {
        if (_catTarget.isCatDead == true) return;
        // Dùng để kích hoạt flying body
        _catTarget.amountCatAttacked++;
        if (_catTarget.amountCatAttacked > 3)
        {
            _catTarget.amountCatAttacked = 0;
            _catTarget.CatFlyingBody(isCatDead);
        }

        // dùng để nhận damage, trừ máu, hiện thanh máu, hiện chỉ số damage
  
        _catTarget.catCurrentHealth -= amount;
        _catTarget.isCatAttacked = true;
        _catTarget.CatHealthBar.SetHealth(_catTarget.catCurrentHealth, _catTarget.catMaxHealth, _catTarget.isCatAttacked);
        DamagePopup.Create(_catTarget.transform.position, amount, catType);
      
        // khi cat dead thi bị đẩy lùi, và die
        if (_catTarget.catCurrentHealth <= 0)
        {
            _catTarget.isCatDead = true;
            _catTarget.CatHealthBar.gameObject.SetActive(false);
            _catTarget.CatFlyingBody(isCatDead);
        }
    }

    public void CatDead(CatController _catTarget)
    {
        Color tmp = spriteRenderer.color;
        LeanTween.value(gameObject, 1, 0, 1f).setOnUpdate((float val) => {
            tmp = new Color(tmp.r, tmp.g, tmp.b, val);
            spriteRenderer.color = tmp;
        });

        Destroy(_catTarget.gameObject);
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

    //public void CatFlyingBody(bool _isCatDead)
    //{
    //    if (actionCat != null)
    //    {
    //        StopCoroutine(actionCat);
    //        actionCat = null;
    //    }
    //    actionCat = IECatFlying(_isCatDead);
    //    StartCoroutine(actionCat);
    //}

    public void CatFlyingBody(bool _isCatDead)
    {
        if (catType.ToString() == "Me") moveTowers = -distanceCatFlying;
        if (catType.ToString() == "Player") moveTowers = distanceCatFlying;
        targetPosFlyDeadCat = new Vector3(transform.position.x+moveTowers, transform.position.y);
        StopCatWalk();
        LeanTween.move(gameObject, targetPosFlyDeadCat, 1.0f).setOnComplete(()=> {
            if (!_isCatDead)
            {
                CatWalk();
            }

            if (_isCatDead)
            {
                StopCatWalk();
                CatDead(catTarget);
            }
        });
     
        //targetPosFlyDeadCat = new Vector3(moveTowers, transform.position.y);
        /**/
        //while (true)
        //{
        //    float step = speedFlyDeadCat * Time.deltaTime;

        //    transform.position = spline.GetPositionFlying(this, speedFlyDeadCat, distanceCatFlying, catType, targetPosFlyDeadCat, step);

        //    if(transform.position == targetPosFlyDeadCat && !_isCatDead)
        //    {
        //        CatWalk();
        //        Debug.Log("CatWalk()");
        //    }

        //    if (transform.position == targetPosFlyDeadCat && _isCatDead)
        //    {
        //        StopCatWalk();
        //       /* SetColorCatDead()*/;

        //        ChangeCatBodyColor();
        //        Debug.Log("StartCoroutine");

        //    }
        //    yield return null;
        //}
    }

    protected void ChangeCatBodyColor()
    {
        Color tmp = spriteRenderer.color;
        LeanTween.value(gameObject, 1, 0, 1f).setOnUpdate((float val) => {
            tmp = new Color(tmp.r,tmp.g,tmp.b,val);
            spriteRenderer.color = tmp;
        });
        CatDead(this);
    }



}