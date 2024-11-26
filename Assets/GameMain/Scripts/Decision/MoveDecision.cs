using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDecision : DecisionBase
{
    private Vector3Int direction;
    private bool isMovingWithMouse;
    private GridInfo targetGrid;
    public MoveDecision(Player player, Vector3Int direction) : base(player)
    {
        this.direction = direction;
    }

    public override bool Evaluate()
    {
        Vector2 pos = Vector2.zero;
        Vector2 targetPos = player.transform.position + direction;
        if (Input.GetMouseButtonDown(0))
        {
            //TODO:把点击到的坐标转换为格子并且移动
            isMovingWithMouse = true;
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetGrid = GridManager.Instance.GetGridByPos(pos);
        }
        return GridManager.Instance.CanWalkTo(targetPos) && (Input.GetKeyDown(GetKeyForDirection()) || CardManager.Instance.isToMove);
    }

    public override void Execute()
    {
        if (isMovingWithMouse)
        {
            player.transform.position = targetGrid.position;
            isMovingWithMouse = true;
            return;
        }
        
        player.transform.Translate(direction.x, direction.y, 0);
        player.hasInput = true;
    }

    private KeyCode GetKeyForDirection()
    {
        if (direction == Vector3Int.up) return KeyCode.W;
        if (direction == Vector3Int.down) return KeyCode.S;
        if (direction == Vector3Int.left) return KeyCode.A;
        if (direction == Vector3Int.right) return KeyCode.D;
        return KeyCode.None;
    }
    public override void ClearStat()
    {
        CardManager.Instance.isToMove = false;
        isMovingWithMouse = false;
    }
}
