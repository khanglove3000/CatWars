using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat_AttackRange : MonoBehaviour
{
    public Cat_Controller catController;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Home")
        {
            Cat_Shop _shopCat = collision.gameObject.GetComponent<Cat_Shop>();
            if (catController.catType != _shopCat.CatType)
            {
                catController.homeTarget = _shopCat;
                catController.CatAttack();
            }
        }

    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (catController.catTarget != null) return;

        if (collision.gameObject.tag != "Cat") return;

        Cat_Controller _cat = collision.gameObject.GetComponent<Cat_Controller>();
        if (catController.catType != _cat.catType)
        {
            catController.catTarget = _cat;
            catController.CatAttack();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        

        catController.catTarget = null;
        catController.isCatAttacked = false;
        if (catController.CatHealthBar == null) return;
        StartCoroutine(WaitForOffHealthBar());
        catController.CatHealthBar.SetHealth(catController.catCurrentHealth, catController.catMaxHealth, catController.isCatAttacked);
        catController.CatWalk();
        Debug.Log("Cat Range Attack - Cat Walk");
        
    }

    protected IEnumerator WaitForOffHealthBar()
    {
        yield return new WaitForSeconds(1f);
    }


}
