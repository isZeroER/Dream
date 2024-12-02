using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EM_CloudCoffee : EnemyBase
{
    protected override void InitEnemy()
    {
        enemyType = EnemyType.CloudCoffee;
        SetBaseStat(6,1,6);
    }

    protected override void CheckHate()
    {
        if (health < 6 && canHate)
        {
            isHating = true;
            hateGameObject.SetActive(true);
        }
    }

    protected override bool CanAttack()
    {
        starHatingPath = GridManager.Instance.FindPath(currentGrid.position, player.transform.position);
        // Debug.Log(currentGrid.position + " " + player.transform.position+" "+transform.position + " " +starHatingPath[0].position);
        // Debug.Log("判断中。。。"+(starHatingPath[0] == player.currentGrid) +" "+ isHating);
        //最近一格是玩家并且处于仇恨状态
        if (starHatingPath.Count > 0 && starHatingPath[0] == player.currentGrid && isHating)
            return true;
        return false;
    }

    protected override void Attack()
    {
        base.Attack();
    }

    protected override void HatingPatrol()
    {
        base.HatingPatrol();
    }

    protected override void Patrol()
    {
        base.Patrol();
    }

    protected override void Die()
    {
        base.Die();
    }

    // protected override bool WillAttack()
    // {
    //     //如果下一个格子就是主角，那么主角可以闪避
    //     if (starHatingPath[0] == player.currentGrid)
    //         return true;
    //     return false;
    // }
}
