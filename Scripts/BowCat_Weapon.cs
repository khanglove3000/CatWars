using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cat_Enum;
public class BowCat_Weapon : MonoBehaviour
{
    public BowCat bowCat;
    public float weaponSpeed = 1f;
    public Transform _posTarget = null;
    public void BowCatWeaponMovement(Cat_Controller _catTarget, Cat_Shop _catShop)
    {
        if (_catShop != null) _posTarget = _catShop.transform;
        if (_catTarget != null) _posTarget = _catTarget.transform;
        LeanTween.move(gameObject, _posTarget.position, weaponSpeed).setOnComplete(() => 
        { 
            bowCat.isHitTheTarget = true;
            Destroy(gameObject);
        });
        
    }

}
