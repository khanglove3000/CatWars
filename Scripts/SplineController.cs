using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineController : MonoBehaviour
{
    public int lineNumber;
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;

    private float journeyLength;
    private void Start()
    {
       
        journeyLength = Vector3.Distance(pointA.position, pointB.position);
    }
    public Vector3 GetPositionGo(float _speed, float startTime) {
        float distCovered = (Time.time - startTime) * _speed;
        float fractionOfJourney = distCovered / journeyLength;
        Debug.Log(distCovered);
       Vector3 _result = Vector3.Lerp(pointA.position, pointB.position, fractionOfJourney);
        return _result;
    }
}
