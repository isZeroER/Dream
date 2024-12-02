using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EM_Serotonin : EnemyBase
{
    private Tilemap lockStone;
    private List<Vector2> aroundPos = new List<Vector2>();
    protected override void InitEnemy()
    {
        enemyType = EnemyType.Serotonin;
        SetBaseStat(12, 1, 45);
        lockStone = GameObject.FindGameObjectWithTag("LockStone").GetComponent<Tilemap>();
        GetAroundGrid();
    }

    protected override void CheckHate()
    {
        Debug.Log(canHate);
        if (canHate)
        {
            isHating = true;
            hateGameObject.SetActive(true);
        }
    }

    protected override bool CanAttack() 
    {
        if (TurnManager.Instance.currentTurnNum % 2 == 0)
            return true;
        return false;
    }

    protected override void Attack()
    {
        // 获取 Tilemap 的 cellBounds
        BoundsInt bounds = GridManager.Instance.currentMap.cellBounds;

        // 随机选择一个有效的格子位置
        int x = Random.Range(bounds.xMin, bounds.xMax);
        int y = Random.Range(bounds.yMin, bounds.yMax);

        // 生成格子坐标
        Vector3Int randomPosition = new Vector3Int(x, y, 0);

        // 确保格子上有 Tile（如果需要）
        while (!GridManager.Instance.currentMap.HasTile(randomPosition) &&
               GridManager.Instance.GetGridByPos(new Vector2(randomPosition.x, randomPosition.y)).characterType != CharacterType.Enemy)
        {
            x = Random.Range(bounds.xMin, bounds.xMax);
            y = Random.Range(bounds.yMin, bounds.yMax);
            randomPosition = new Vector3Int(x, y, 0);
        }

        Vector2 targetPos = new Vector2(randomPosition.x, randomPosition.y);
        starHatingPath = GridManager.Instance.FindPath(currentGrid.position, targetPos);
        MoveAttack(targetPos);
    }
    
    private void MoveAttack(Vector2 targetPos)
    {
        float speed = 0.05f;
        // 更新最终位置信息
        UpdateGridInfoNow(targetPos);
    
        int currentNum = 0;
        int pathCount = starHatingPath.Count;  // 获取路径的总长度
    
        // 使用协程来处理逐步移动
        StartCoroutine(MoveAlongPath(currentNum, pathCount, speed));
    }

    private IEnumerator MoveAlongPath(int currentNum, int pathCount, float speed)
    {
        while (currentNum < pathCount)
        {
            // 获取当前目标位置
            Vector2 targetPosition = starHatingPath[currentNum].position;
            Vector2 currentPosition = transform.position;
            // 执行移动到目标位置
            transform.DOMove(targetPosition, speed).OnComplete(() =>
            {
                //如果当前格子为玩家格子
                if (starHatingPath[currentNum] == player.currentGrid)
                {
                    if (aroundPos.Contains(player.currentGrid.position))
                    {
                        //玩家要移动
                        Vector2 dir = player.currentGrid.position - currentPosition;
                        dir = Random.Range(0, 2) == 0 ? Vector2.Perpendicular(dir) : -Vector2.Perpendicular(dir);
                        player.BeMove(player.currentGrid.position + dir);
                    }
                    DoDamage(strength, player);
                }
                // 每次移动完成后，更新 currentNum，指向下一个格子
                currentNum++;
            });

            // 等待当前 DOMove 动画完成再继续执行下一步
            yield return new WaitForSeconds(speed);
        }
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
                }
            }
        }
    }
}
