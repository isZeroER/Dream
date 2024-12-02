using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EM_Melatonin : EnemyBase
{
    private Tilemap lockStone;
    private List<Vector2> aroundPos = new List<Vector2>();
    protected override void InitEnemy()
    {
        enemyType = EnemyType.Melatonin;
        SetBaseStat(2, 1, 30);
        
        lockStone = GameObject.FindGameObjectWithTag("LockStone").GetComponent<Tilemap>();
        GetAroundGrid();
    }

    private void GetAroundGrid()
    {
        // 获取 Tilemap 的 cellBounds（边界矩形）
        BoundsInt bounds = lockStone.cellBounds;

        // 遍历 Tilemap 中的所有格子
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                // 获取每个格子的世界坐标
                Vector3Int cellPosition = new Vector3Int(x, y, 0);

                // 如果该格子上有 Tile，就打印其世界位置
                if (lockStone.HasTile(cellPosition))
                {
                    //这是锁格
                    Vector2 worldPosition = lockStone.CellToWorld(cellPosition);
                    aroundPos.Add(worldPosition);
                    aroundPos.Add(worldPosition + Vector2.up);
                    aroundPos.Add(worldPosition + Vector2.down);
                    aroundPos.Add(worldPosition + Vector2.left);
                    aroundPos.Add(worldPosition + Vector2.right);
                }
            }
        }
    }

    protected override void CheckHate()
    {
        Debug.Log(canHate);
        isHating = true;
    }

    protected override bool CanAttack()
    {
        //玩家是否在格子上 且 当前为双数回合
        Debug.Log(aroundPos.Contains(player.currentGrid.position) +" "+ (TurnManager.Instance.currentTurnNum));
        if (aroundPos.Contains(player.currentGrid.position) && TurnManager.Instance.currentTurnNum %2 == 0)
            return true;
        return false;
    }

    protected override void Attack()
    {
        //远程攻击
        Debug.Log("攻击");
        DoDamage(strength, player);
    }

    protected override void HatingPatrol()
    {
        
    }
}
