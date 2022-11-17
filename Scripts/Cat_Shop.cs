using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cat_Enum;

public class Cat_Shop : MonoBehaviour
{
    [Header("Health")]
    public int currentHealth;
    public int maxHealth = 500;
    public Cat_HealthBar healthBarBehaive;

    public List<Cat_Controller> ListCatPrefabs;
    public Transform spawnPoint;
    public CatType CatType;
    public List<Cat_Controller> listCats;
    public Transform hitPoint;

    private bool attacked = false;

    protected GameObject CatPrefabs;
        
    private void Start()
    {
        listCats = new List<Cat_Controller>();
        currentHealth = maxHealth;
        healthBarBehaive.SetHealth(currentHealth, maxHealth, attacked);
    }
    public void CreateCat(int _index)
    {
        Cat_Controller _cat =  Instantiate(ListCatPrefabs[_index], spawnPoint.position, spawnPoint.rotation);
        _cat.catType = CatType;
        _cat.gameObject.tag = "Cat";
        Cat_IngameManager.instance.SetRandomLine(_cat, _cat.catType);
        /*Gan them spline*/
        listCats.Add(_cat);
        _cat.transform.parent = transform;
        _cat.CatWalk();
    }
  
    public void HomeGetDamage(int amount)
    {
        currentHealth -= amount;
        attacked = true;
        healthBarBehaive.SetHealth(currentHealth, maxHealth, attacked);

        Cat_DamagePopup.Create(hitPoint.position, amount, CatType);
        if (currentHealth <= 0)
        {
            DestroyHome();
        }
    }

    private void DestroyHome()
    {
        Destroy(gameObject);
    }
}
