using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cat_Enum;
public class Cat_IngameManager : MonoBehaviour
{
    public List<Cat_SplineController> listSplines;

    public static Cat_IngameManager instance;

    //private Cat_SplineController_spline;
    private void Awake()
    {
        instance = this;
    }
    public void SetRandomLine(Cat_Controller _cat, CatType catType)
    {
        int _number = Random.Range(0, 4);
        Cat_SplineController _spline = listSplines[_number];
        int _sortOrder = (_number == 0) ? Random.Range(0, 1000) : Random.Range(_number * 1000, (_number + 1) * 1000);
        _cat.spline = _spline;
        _cat.spriteRenderer.sortingOrder = _sortOrder;
    }
}
