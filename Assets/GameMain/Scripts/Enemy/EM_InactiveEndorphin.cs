using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EM_InactiveEndorphin : EnemyBase
{
    protected override void Start()
    {
        base.Start();
        enemyType = EnemyType.InactiveEndorphin;
    }
}
