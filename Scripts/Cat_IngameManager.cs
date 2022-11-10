using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat_IngameManager : MonoBehaviour
{
    public List<SplineController> listSplines;
    public static Cat_IngameManager instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {

    }
    public void SetRandomLine(CatController _cat)
    {
        int _number = Random.Range(0, 4);
        SplineController _spline = listSplines[_number];
        int _sortOrder = (_number == 0) ? Random.Range(0, 1000) : Random.Range(_number * 1000, (_number + 1) * 1000);
        _cat.spline = _spline;
        _cat.spriteRenderer.sortingOrder = _sortOrder;
    }
}
