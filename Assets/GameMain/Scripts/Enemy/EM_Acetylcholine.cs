using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EM_Acetylcholine : EnemyBase
{
    protected override void InitEnemy()
    {
        SetBaseStat(12, 0, 3);
        enemyType = EnemyType.Acetylcholine;       
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
