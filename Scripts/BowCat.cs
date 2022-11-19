using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowCat : Cat_Controller
{
    [Header("Bow Cat Weapon")]
    public BowCat_Weapon bowCatWeapon;
    public Transform pointToShoot;
    public bool isHitTheTarget = false;
    public BowCat_Weapon _bowCatWeapon = null;

    public override IEnumerator WaitForNextCatAttack()
    {
        while (true)
        {
            catAnimator.SetBool("Attack", true);
            _bowCatWeapon = Instantiate(bowCatWeapon, pointToShoot.position, pointToShoot.rotation);
            _bowCatWeapon.BowCatWeaponMovement(catTarget, homeTarget, catAttackSpeed);

            if (catTarget != null)
            {
                if (isHitTheTarget == true)
                {
                    homeTarget = null;
                    catTarget.CatGetDamage(amountDamage);
                }
            }

            if (homeTarget != null)
            {
                if (isHitTheTarget == true) homeTarget.HomeGetDamage(amountDamage);
            }

            catAnimator.speed = (catAttackSpeed < 1) ? 1 : catAttackSpeed;
            float _lengthAnim = (catAttackSpeed <= 1) ? catAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length / catAnimator.GetCurrentAnimatorStateInfo(0).speed
                       : (catAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length / catAttackSpeed) / catAnimator.GetCurrentAnimatorStateInfo(0).speed;
            //Debug.LogError("_lengthAnim: " + _lengthAnim);
            yield return new WaitForSeconds(_lengthAnim);
            catAnimator.SetBool("Attack", false);
            float waitTime = (catDurationAttack / catAttackSpeed) - _lengthAnim;
            yield return new WaitForSeconds(waitTime);
            //Debug.LogError("waitTime: " + waitTime);
            yield return null;

        }
    }
    public override void CatAttack()
    {
        StopCatWalk();
        if (catTarget == null) return;
        if (catTarget.isCatDead == true) catTarget = null;
        if (catTarget) homeTarget = null;
        StartCoroutine(catController.WaitForNextCatAttack());
    }

}
