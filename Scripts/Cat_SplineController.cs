using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cat_Enum;
public class Cat_SplineController : MonoBehaviour
{
    public int lineNumber;
    public Transform pointA;
    public Transform pointB;

    private float journeyLength;
    private Vector3 _result;
    private float distCovered;
    private float fractionOfJourney;

    private Vector3 _resultFlyingBody;


    public Vector3 GetPositionGo(Cat_Controller _cat, float _speed, CatType catType) 
    {
        distCovered = _speed / 100;
       
        if (catType.ToString() == "Me")
        {
            journeyLength = Vector3.Distance(_cat.transform.position, pointB.position);
            fractionOfJourney = distCovered / journeyLength;
            _result = Vector3.Lerp(_cat.transform.position, pointB.position, fractionOfJourney);

        }
        if (catType.ToString() == "Player")
        {
            journeyLength = Vector3.Distance(_cat.transform.position, pointA.position);
            fractionOfJourney = distCovered / journeyLength;
            _result = Vector3.Lerp(_cat.transform.position, pointA.position, fractionOfJourney);
        }

        return _result;
    }

    public Vector3 GetPositionFlying(Cat_Controller _cat, float speedFlyDeadCat, float distanceCatFlying, CatType catType, Vector3 targetPosFlyDeadCat, float step)
    {
        _resultFlyingBody = Vector3.MoveTowards(_cat.transform.position, targetPosFlyDeadCat, step);
        return _resultFlyingBody;
    }

}
