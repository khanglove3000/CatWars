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

    public override void CatAttack()
    {

        StopCatWalk();
        if (homeTarget)
        {
            catAnimator.SetBool("Attack", true);
            _bowCatWeapon = Instantiate(bowCatWeapon, pointToShoot.position, pointToShoot.rotation);
            _bowCatWeapon.BowCatWeaponMovement(catTarget, homeTarget);
            if (isHitTheTarget == true)
            {
                homeTarget.HomeGetDamage(amountDamage);
            }
        }

        if (catTarget == null) return;
        if (catTarget.isCatDead == true) catTarget = null;
        catAnimator.SetBool("Attack", true);
        _bowCatWeapon = Instantiate(bowCatWeapon, pointToShoot.position, pointToShoot.rotation);
        _bowCatWeapon.BowCatWeaponMovement(catTarget, homeTarget);
        if (isHitTheTarget == true)
        {
            catTarget.CatGetDamage(amountDamage);
        }
        
    }
}
