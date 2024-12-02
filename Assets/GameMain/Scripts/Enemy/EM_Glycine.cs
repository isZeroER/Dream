using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EM_Glycine : EnemyBase
{
    protected override void InitEnemy()
    {
        enemyType = EnemyType.Glycine;
        SetBaseStat(4, 1,3);
    }

    protected override void CheckHate()
    {
        
    }

    protected override bool CanAttack()
    {
        return false;
    }

    protected override void Attack()
    {
        base.Attack();
    }

    protected override void Patrol()
    {
        base.Patrol();
    }

    protected override void HatingPatrol()
    {
        base.HatingPatrol();
    }
}
