using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EM_Glycine : EnemyBase
{
    protected override void InitEnemy()
    {
        enemyType = EnemyType.Glycine;
        SetBaseStat(4, 0, 4);
    }

    protected override bool CanAttack()
    {
        return true;
    }

    protected override void Attack()
    {
        
    }

    protected override void Patrol()
    {
        
    }

    protected override void HatingPatrol()
    {
        
    }
}
