using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat_GreateSwordCat : CatBase
{
    protected override void ResetValues()
    {
        base.ResetValues();
        speedWalk = 1f;
        maxHealth = 500;
        attackDamage = 15;
        sortingOrderBase = 3000;
    }
}
