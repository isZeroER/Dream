using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public bool hasInput = false;
    private List<IDecision> decisions = new List<IDecision>();
    private void Start()
    {
        //初始化生命和攻击力，确定种类
        health = 3;
        strength = 2;
        characterType = CharacterType.Player;
        currentGrid = GridManager.Instance.GetGridByPos(transform.position);
        
        decisions.Add(new MoveDecision(this, Vector3Int.up));
        decisions.Add(new MoveDecision(this, Vector3Int.down));
        decisions.Add(new MoveDecision(this, Vector3Int.left));
        decisions.Add(new MoveDecision(this, Vector3Int.right));
        decisions.Add(new AttackDecision(this));
        decisions.Add(new DodgeDecision(this));
    }
    
    public override void HandleMethod()
    {
        base.HandleMethod();
        CheckInput();
    }
    
    /// <summary>
    /// 玩家输入。。。待修改为卡牌
    /// </summary>
    public void CheckInput()
    {
        if (hasInput) return;
        foreach (var decision in decisions)
        {
            if (decision.Evaluate())
            {
                decision.Execute();
                break;
            }
        }

        if (hasInput)
        {
            UpdateGridInfo(); 
        }
    }

    public void ClearStat()
    {
        foreach (var decision in decisions)
        {
            decision.ClearStat();
        }
    }

    /// <summary>
    /// 输入之后，格子信息发生改变，当前GridInfo发生改变
    /// </summary>
    private void UpdateGridInfo()
    {
        GridManager.Instance.ChangeGridInfo(currentGrid, CharacterType.None);
        currentGrid = GridManager.Instance.GetGridByPos(transform.position);
        GridManager.Instance.ChangeGridInfo(currentGrid, characterType);
    }
}