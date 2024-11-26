using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeDecision : DecisionBase
{
    public DodgeDecision(Player player) : base(player) { }

    public override bool Evaluate()
    {
        //后面从敌人那里获取是否需要闪避
        return Input.GetKeyDown(KeyCode.LeftShift) || CardManager.Instance.isToDodge;
    }

    public override void Execute()
    {
        var adjacentGrids = GridManager.Instance.GetAdjacentGrids(player.transform.position);
        foreach (var grid in adjacentGrids)
        {
            if (grid.characterType != Character.CharacterType.None && grid.gridType == GridType.CanWalk)
            {
                player.transform.position = new Vector3(grid.position.x, grid.position.y, 0);
                Debug.Log("执行闪避逻辑！");
                player.hasInput = true;
                break;
            }
        }
    }
    
    public override void ClearStat()
    {
        CardManager.Instance.isToDodge = false;
    }
}

