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

    private void Start()
    {
        listCats = new List<CatController>();
        currentHealth = maxHealth;
        healthBarBehaive.SetHealth(currentHealth, maxHealth);
    }
    public void CreateCat(int _index)
    {
        CatController _cat =  Instantiate(ListCatPrefabs[_index], spawnPoint.position, spawnPoint.rotation);
        _cat.catType = CatType;
        _cat.gameObject.tag = CatType.ToString();
        Cat_IngameManager.instance.SetRandomLine(_cat);
        /*Gan them spline*/
        listCats.Add(_cat);
    }

    public void TakeDamageHome(int amount)
    {

        currentHealth -= amount;
        healthBarBehaive.SetHealth(currentHealth, maxHealth);

        //healthBar.fillAmount = currentHealth / maxHealth;

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
