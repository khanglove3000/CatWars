using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowards : MonoBehaviour
{
    public float speedFlyDeadCat = 10.0f;
    public Vector2 targetPosFlyDeadCat;

    [NaughtyAttributes.Button]
    public void PressToCuBeRun()
    {
        StartCoroutine(CatMovement());
    }
    IEnumerator CatMovement()
    {
        targetPosFlyDeadCat = new Vector2(transform.position.x + 10f, transform.position.y);
        while (true)
        {
            float step = speedFlyDeadCat * Time.deltaTime;
          
            transform.position = Vector2.MoveTowards(transform.position, targetPosFlyDeadCat, step);
            yield return null;
        }
    }

}
