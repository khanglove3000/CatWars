using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat_Attack : MonoBehaviour
{
    public CatController catController;
 
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Cat")
        {
            CatController _cat = collision.gameObject.GetComponent<CatController>();
            if (catController.catType != _cat.catType)
            {
                catController.catTarget = _cat;
                catController.CatAttack();
            }
            if(catController.catTarget == null)
            {
                catController.CatWalk();
            }
        }
        else if (collision.gameObject.tag == "Home")
        {
            ShopCat _shopCat = collision.gameObject.GetComponent<ShopCat>();
            if (catController.catType != _shopCat.CatType)
            {
                catController.homeTarget = _shopCat;
            }
        }
    }

 

}
