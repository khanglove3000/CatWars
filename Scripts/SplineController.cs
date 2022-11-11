using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CatWarEnum;
public class SplineController : MonoBehaviour
{
    public int lineNumber;
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;

    private float journeyLength;
    private Vector3 _result;
    private float distCovered;
    private float fractionOfJourney;

    public Vector3 GetPositionGo(CatController _cat, float _speed, CatType catType) 
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
}
