using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EM_EmptyCoffeeCup : EnemyBase
{
    protected override void InitEnemy()
    {
        enemyType = EnemyType.EmptyCoffeeCup;
        SetBaseStat(6, 0, 4);
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
