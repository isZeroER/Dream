using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDecision : DecisionBase
{
    private Vector2 direction;
    private Vector2 baseDirection;
    private bool isMovingWithMouse;
    private GridInfo targetGrid;
    public MoveDecision(Player player, Vector2 direction) : base(player)
    {
        this.direction = direction;
        baseDirection = direction;
    }

    public override bool Evaluate()
    {
        Vector2 targetPos = (Vector2)player.transform.position + direction;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
            
            //如果点击到了
            if (hit.collider != null && hit.collider.CompareTag("TipGrid"))
            {
                //获取hit的位置
                Vector2 hitWorldPos = hit.point;
                GridInfo mouseGrid = GridManager.Instance.GetGridByPos(hitWorldPos);

                //判断是否为空，并且是可走的
                if (mouseGrid != null && IsValidMouseTarget(mouseGrid.position))
                {
                    isMovingWithMouse = true;
                    //获取这一格的方向
                    direction = mouseGrid.position - (Vector2)player.transform.position;
                    return true;
                }
            }
        }
        
        return GridManager.Instance.CanWalkTo(targetPos) && (Input.GetKeyDown(GetKeyForDirection()));
    }

    public override void Execute()
    {
        player.transform.Translate(direction.x, direction.y, 0);
        player.hasInput = true;
    }
    
    /// <summary>
    /// 判断是否为玩家四周的格子
    /// </summary>
    /// <param name="gridPos"></param>
    /// <returns></returns>
    private bool IsValidMouseTarget(Vector2 gridPos)
    {
        // 确保鼠标点击的位置在玩家四周一个格子范围内
        Vector2 playerGridPos = player.transform.position;
        Vector2 offset = gridPos - playerGridPos;

        if (Mathf.Abs(offset.x) + Mathf.Abs(offset.y) != 1) // 只允许四个方向的一个格子范围
            return false;

        
        return GridManager.Instance.CanWalkTo(gridPos);
    }

    private KeyCode GetKeyForDirection()
    {
        if (direction == Vector2.up) return KeyCode.W;
        if (direction == Vector2.down) return KeyCode.S;
        if (direction == Vector2.left) return KeyCode.A;
        if (direction == Vector2.right) return KeyCode.D;
        return KeyCode.None;
    }
    public override void ClearStat()
    {
        CardManager.Instance.isToMove = false;
        isMovingWithMouse = false;
        direction = baseDirection;
    }
}
