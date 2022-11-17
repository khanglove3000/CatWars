using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Cat_Enum;
public class Cat_DamagePopup : MonoBehaviour
{
    public TextMeshPro textMesh;
    public float disappearTimer;
    public Color textColor;
    public Vector3 moveVector;

    public static Cat_DamagePopup Create(Vector3 positionCatTarget, int damageAmount, CatType catType)
    {    
        Transform damagePopupTransform = Instantiate(GameAssets.i.pfDamagePopup, positionCatTarget, Quaternion.identity);
        Cat_DamagePopup damagePopup = damagePopupTransform.GetComponent<Cat_DamagePopup>();
        damagePopup.SetUp(damageAmount, catType);
        return damagePopup;
    }


    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void SetUp(int damageAmount, CatType catType)
    {
        textMesh.SetText(damageAmount.ToString());
        
        textColor = textMesh.color;
        disappearTimer = 1f;
        int _randomMoveVectorX = Random.Range(5, 15);
        int _randomMoveVectorY = Random.Range(5, 15);
        if (catType.ToString() == "Me") moveVector = new Vector3(_randomMoveVectorX, _randomMoveVectorY);
        if (catType.ToString() == "Player") moveVector = new Vector3(-_randomMoveVectorX, _randomMoveVectorY);
    }

    private void Update()
    { 
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 5f * Time.deltaTime;
        disappearTimer -= Time.deltaTime;
        if(disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            //textMesh.color = textColor;
            if(textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
