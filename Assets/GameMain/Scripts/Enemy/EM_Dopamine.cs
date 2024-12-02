using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EM_Dopamine : EnemyBase
{
    protected override void InitEnemy()
    {
        enemyType = EnemyType.Dopamine;
        SetBaseStat(8,1,12);
    }

    protected override void CheckHate()
    {
        if (health < 8 && canHate)
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
        DoDamage(strength, player);
        health++;
        Vector2 dir = player.currentGrid.position - currentGrid.position;
        if (GridManager.Instance.GetGridByPos(player.currentGrid.position+dir) == null)
        {
            dir = Random.Range(0, 2) == 0 ? Vector2.Perpendicular(dir) : -Vector2.Perpendicular(dir);
        }

        if (GridManager.Instance.GetGridByPos(player.currentGrid.position+dir) == null)
        {
            dir.x *= -1;
            dir.y *= -1;
            if (GridManager.Instance.GetGridByPos(player.currentGrid.position + dir) == null)
            {
                DoDamage(strength, player);
                return;
            }
        }
        transform.DOMove(player.transform.position, .5f).OnComplete(UpdateGridInfo);
        player.BeMove(player.currentGrid.position + dir);
    }
}
