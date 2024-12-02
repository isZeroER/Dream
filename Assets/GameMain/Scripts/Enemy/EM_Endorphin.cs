using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EM_Endorphin : EnemyBase
{
    protected override void InitEnemy()
    {
        enemyType = EnemyType.CloudCoffee;
        SetBaseStat(6,1,5);
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
        Debug.Log(currentGrid.position + " " + player.transform.position+" "+transform.position + " " +starHatingPath[0].position);
        // Debug.Log("判断中。。。"+(starHatingPath[0] == player.currentGrid) +" "+ isHating);
        //最近一格是玩家并且处于仇恨状态
        if (starHatingPath.Count > 0 && starHatingPath[0] == player.currentGrid && isHating)
            return true;
        return false;
    }
}
