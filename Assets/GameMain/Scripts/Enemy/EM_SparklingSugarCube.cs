using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EM_SparklingSugarCube : EnemyBase
{
    protected override void InitEnemy()
    {
        enemyType = EnemyType.SparklingSugarCube;
        SetBaseStat(2, 1, 2);
    }

    protected override void CheckHate()
    {
        Debug.Log(canHate);
        if(canHate)
            isHating = true;
    }

    protected override bool CanAttack()
    {
        return false;
    }

    protected override void Attack()
    {
        
    }

    protected override void HatingPatrol()
    {
        base.HatingPatrol();
    }

    protected override void Patrol()
    {
        base.Patrol();
    }
}
