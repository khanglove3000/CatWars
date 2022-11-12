using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CatWarEnum;

public class ShopCat : MonoBehaviour
{
    [Header("Health")]
    public int currentHealth;
    public int maxHealth = 500;
    public HealthBar healthBarBehaive;

    public List<CatController> ListCatPrefabs;
    public Transform spawnPoint;
    public CatType CatType;
    public List<CatController> listCats;
    public Transform hitPoint;

    private bool attacked = false;

    protected GameObject CatPrefabs;
        
    private void Start()
    {
        listCats = new List<CatController>();
        currentHealth = maxHealth;
        healthBarBehaive.SetHealth(currentHealth, maxHealth, attacked);
    }
    public void CreateCat(int _index)
    {
        CatController _cat =  Instantiate(ListCatPrefabs[_index], spawnPoint.position, spawnPoint.rotation);
        _cat.catType = CatType;
        _cat.gameObject.tag = "Cat";
        Cat_IngameManager.instance.SetRandomLine(_cat, _cat.catType);
        /*Gan them spline*/
        listCats.Add(_cat);
        _cat.transform.parent = transform;
        _cat.CatWalk();
    }
  
    public void TakeDamageHome(int amount)
    {
        currentHealth -= amount;
        attacked = true;
        healthBarBehaive.SetHealth(currentHealth, maxHealth, attacked);

        DamagePopup.Create(hitPoint.position, amount);
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
