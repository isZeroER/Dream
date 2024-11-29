using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EM_InactiveEndorphin : EnemyBase
{
    protected override void InitEnemy()
    {
        enemyType = EnemyType.InactiveEndorphin;
        //血量和攻击力
        SetBaseStat(6,0, 6);
    }

    protected override bool CanAttack()
    {
        // GridInfo[] aroundGrids = GridManager.Instance.GetAdjacentGrids(transform.position);
        //
        // foreach (var aroundGrid in aroundGrids)
        // {
        //     if (aroundGrid.characterType == CharacterType.Player)
        //     {
        //         DoDamage(strength, player);
        //         return true;
        //     }
        // }
        // return false;
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
        
    }
}
