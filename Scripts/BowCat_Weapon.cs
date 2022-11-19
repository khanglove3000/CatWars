using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cat_Enum;
public class BowCat_Weapon : MonoBehaviour
{
    public BowCat bowCat;
    public float weaponSpeed;
    public Transform _posTarget = null;
    private Vector3 _posTempY = new Vector3(0, 0, 0);
    public void BowCatWeaponMovement(Cat_Controller _catTarget, Cat_Shop _catShop, float _catAttackSpeed)
    {
        weaponSpeed = _catAttackSpeed/3;
        if (_catShop != null)
        {
            float _number = Random.Range(-5, -4f);
            _posTarget = _catShop.transform;
            _posTempY = new Vector3(0, _number, 0);

        }
        if (_catTarget != null) _posTarget = _catTarget.transform;
        if (_posTarget == null) 
        {
            Destroy(gameObject);
            return;
        };
        LeanTween.move(gameObject, _posTarget.position + _posTempY, weaponSpeed).setOnComplete(() =>
        {
            bowCat.isHitTheTarget = true;
            Destroy(gameObject);
        });

    }
}
