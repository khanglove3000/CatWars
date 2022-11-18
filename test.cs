using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Animator anim;
    public float attackSpeed;
    public int duration;
    public float walkSpeed;
    public bool canAttack = true;

    [Button]
    public void PressToAttack()
    {
        attackSpeed = 2f;
        //AttackAnimationEvent();
        StartCoroutine(AutoAttack());
    }

    [Button]
    public void PressToWalk()
    {
        walkSpeed = 1f;
        WalkAnimationEvent();

    }

    [Button]
    public void PressToIdle()
    {
        attackSpeed = -1f;
        walkSpeed = -1f;
        AttackAnimationEvent();
        WalkAnimationEvent();
    }
    public void AttackAnimationEvent()
    {
        anim.SetFloat("Attack", attackSpeed);
        anim.speed = (attackSpeed < 1) ? 1 : attackSpeed;
        float _lengthAnim = (attackSpeed <= 1) ? anim.GetCurrentAnimatorClipInfo(0)[0].clip.length / anim.GetCurrentAnimatorStateInfo(0).speed
                   : (anim.GetCurrentAnimatorClipInfo(0)[0].clip.length / attackSpeed) / anim.GetCurrentAnimatorStateInfo(0).speed;
        float waitTime = (1/attackSpeed)-_lengthAnim;
        StartCoroutine(WaitTime(waitTime));

    }
    public IEnumerator AutoAttack() {
        while (true) {  
            anim.SetFloat("Attack", attackSpeed);
            anim.speed = (attackSpeed < 1) ? 1 : attackSpeed;
            float _lengthAnim = (attackSpeed <= 1) ? anim.GetCurrentAnimatorClipInfo(0)[0].clip.length / anim.GetCurrentAnimatorStateInfo(0).speed
                       : (anim.GetCurrentAnimatorClipInfo(0)[0].clip.length / attackSpeed) / anim.GetCurrentAnimatorStateInfo(0).speed;
            Debug.LogError("_lengthAnim: " + _lengthAnim);
            yield return new WaitForSeconds(_lengthAnim);
            anim.SetFloat("Attack",-1);
            float waitTime = (duration / attackSpeed) - _lengthAnim;
            yield return new WaitForSeconds(waitTime);
            Debug.LogError("waitTime: " + waitTime);
            yield return null;
        }
    }
    IEnumerator WaitTime(float _waitTimer)
    {
        yield return new WaitForSeconds(_waitTimer);
    }

    public void WalkAnimationEvent()
    {
        
    }


}
