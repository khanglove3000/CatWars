using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cat_Enum;
public class Cat_Zeus : Cat_Controller
{
    public Animator thunderAnimator;

    public virtual IEnumerator WaitForNextCatAttack()
    {
        while (true)
        {
            catAnimator.SetBool("Attack", true);
            thunderAnimator.SetBool("Thunder", true);
            //Debug.Log("Zeus Cat Attack");

            if (catTarget != null)
            {
                homeTarget = null;
                catTarget.CatGetDamage(amountDamage);
            }

            if (homeTarget != null)
            {
                homeTarget.HomeGetDamage(amountDamage);
            }

            catAnimator.speed = (catAttackSpeed < 1) ? 1 : catAttackSpeed;
            float _lengthAnim = (catAttackSpeed <= 1) ? catAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length / catAnimator.GetCurrentAnimatorStateInfo(0).speed
                       : (catAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length / catAttackSpeed) / catAnimator.GetCurrentAnimatorStateInfo(0).speed;
            //Debug.LogError("_lengthAnim: " + _lengthAnim);
            yield return new WaitForSeconds(_lengthAnim);
            catAnimator.SetBool("Attack", false);
            
            float waitTime = (catDurationAttack / catAttackSpeed) - _lengthAnim;
            yield return new WaitForSeconds(waitTime);
            thunderAnimator.SetBool("Thunder", false);
            //Debug.LogError("waitTime: " + waitTime);
            yield return null;

        }
    }

    public virtual void CatAttack()
    {
        StopCatWalk();
        if (catTarget == null) return;
        if (catTarget.isCatDead == true) catTarget = null;
        if (catTarget) homeTarget = null;
        Debug.Log("Zeus Cat Attack");
        StartCoroutine(catController.WaitForNextCatAttack());
        
    }
}
