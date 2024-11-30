using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EM_Acetylcholine : EnemyBase
{
    protected override void InitEnemy()
    {
        enemyType = EnemyType.Acetylcholine;       
        SetBaseStat(12, 0, 3);
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

    protected override void Patrol()
    {
        base.Patrol();
    }

    protected override void HatingPatrol()
    {
        base.HatingPatrol();
    }
}
