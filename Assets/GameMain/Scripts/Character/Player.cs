using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public bool hasInput = false;
    private List<IDecision> decisions = new List<IDecision>();
    public static event Action UpdatePlayerPos;
    public int currentScore { get; private set; }
    [Space] 
    [SerializeField] private GameObject tryTest;
    protected override void Start()
    {
        base.Start();
        //初始化生命和攻击力，确定种类
        health = 3;
        strength = 2;
        characterType = CharacterType.Player;
        currentGrid = GridManager.Instance.GetGridByPos(transform.position);
        
        decisions.Add(new MoveDecision(this, Vector2.up));
        decisions.Add(new MoveDecision(this, Vector2.down));
        decisions.Add(new MoveDecision(this, Vector2.left));
        decisions.Add(new MoveDecision(this, Vector2.right));
        decisions.Add(new AttackDecision(this));
        decisions.Add(new DodgeDecision(this));
    }


    private void OnEnable()
    {
        EventManager.SendScore += AddScore;
    }

    private void OnDisable()
    {
        EventManager.SendScore -= AddScore;
    }

    private void AddScore(int score)
    {
        currentScore += score;
    }

    public override void HandleMethod()
    {
        base.HandleMethod();
        CheckInput();
    }

    protected override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        //更新生命值
        EventManager.CallUpdateHealth();
    }

    /// <summary>
    /// 玩家输入。。。待修改为卡牌
    /// </summary>
    private void CheckInput()
    {
        //TODO:一个关卡胜利
        // if ((Vector2)transform.position == new Vector2(1, 1))
        // {
        //     tryTest.SetActive(true);
        //     return;
        // }
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
        //这里
        currentGrid = GridManager.Instance.GetGridByPos(transform.position);
        GridManager.Instance.ChangeGridInfo(currentGrid, characterType);
        
        UpdatePlayerPos?.Invoke();
    }
}
