using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CatWarEnum;

public class CatBase : MonoBehaviour
{

    [Header("Health")]
    public int currentHealth;
    public int maxHealth;
    public HealthBar healthBarBehaive;
    public Transform hitPoint;
    public bool isDead = false;

    [Header("Movement")]
    public CatBase catBase;
    public float speedWalk;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    public CatType catType;
    IEnumerator actionCat;

    [Header("Attack")]
    public int attackDamage;
    public CatBase catTarget;
    public ShopCat homeTarget;
    //[Header("Unity Stuff")]
    //public Image healthBar;

    [Header("Splines")]
    public Transform startMarker;
    public Transform endMarker;
    public Transform pointAB;
    public float startTime;
    public float journeyLength;

    [Header("SortLayer")]
    public int sortingOrderBase;

    public Renderer myRenderer;


    protected virtual void LateUpdate()
    {
        myRenderer.sortingOrder = (int)(sortingOrderBase + 1);
    }
    private void Start()
    {
        currentHealth = maxHealth;
        healthBarBehaive.SetHealth(currentHealth, maxHealth);
        startTime = Time.time;
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);

    }

    protected virtual void Reset()
    {
        this.ResetValues();
    }

    private void FixedUpdate()
    {
        if (catTarget || homeTarget)
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
        while (true)
        {
            //transform.Translate(_toward * speedWalk * Time.deltaTime);
            //speedWalk = (speedWalk + Time.deltaTime) % 0.1f;
            float distCovered = (Time.time - startTime) * speedWalk;
            float fractionOfJourney = distCovered / journeyLength;
            pointAB.position = Vector3.Lerp(startMarker.position, endMarker.position, fractionOfJourney);
            yield return null;
        }
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
        if (catType == CatType.Me && collision.gameObject.tag == "Player")
        {
            catTarget = collision.gameObject.GetComponent<CatBase>();
            ToAttack();
        }

        if (catType == CatType.Player && collision.gameObject.tag == "Me")
        {
            // cần chỉnh lại
            catTarget = collision.gameObject.GetComponent<CatBase>();
            ToAttack();
            // Tạo class character để cho những class con kế thừa
            // thay đổi tag thành character
        }

        if (catType == CatType.Me && collision.gameObject.tag == "HomePlayer")
        {
            homeTarget = collision.gameObject.GetComponent<ShopCat>();

        }

        if (catType == CatType.Player && collision.gameObject.tag == "HomeMe")
        {
            homeTarget = collision.gameObject.GetComponent<ShopCat>();
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


    protected virtual void ResetValues()
    {
        //For Overide
    }
}
