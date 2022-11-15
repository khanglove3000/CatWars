using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat_Attack : MonoBehaviour
{
    public CatController catController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Home")
        {
            ShopCat _shopCat = collision.gameObject.GetComponent<ShopCat>();
            if (catController.catType != _shopCat.CatType)
            {
                catController.homeTarget = _shopCat;
                catController.CatAttack();
            }
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (catController.catTarget != null) return;

        if (collision.gameObject.tag != "Cat") return;
        
        CatController _cat = collision.gameObject.GetComponent<CatController>();
        if (catController.catType != _cat.catType)
        {
            catController.catTarget = _cat;
            catController.CatAttack();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        catController.catTarget = null;
        catController.isCatAttacked = false;
        if (catController.CatHealthBar == null) return; 
        catController.CatHealthBar.SetHealth(catController.catCurrentHealth, catController.catMaxHealth, catController.isCatAttacked);

        IEnumerator _actioncat = catController.actionCatFlying;
        if (_actioncat != null)
        {
            StopCoroutine(_actioncat);
            _actioncat = null;
        }
        catController.CatWalk();
    }


}
