using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDecision : DecisionBase
{
    private Vector3Int direction;

    public MoveDecision(Player player, Vector3Int direction) : base(player)
    {
        this.direction = direction;
    }

    public override bool Evaluate()
    {
        Vector2 targetPos = player.transform.position + direction;
        return GridManager.Instance.CanWalkTo(targetPos) && (Input.GetKeyDown(GetKeyForDirection()) || CardManager.Instance.isToMove);
    }

    public override void Execute()
    {
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
    }
}
