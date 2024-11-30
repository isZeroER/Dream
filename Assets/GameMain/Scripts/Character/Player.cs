using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Player : Character
{
    public bool hasInput = false;
    private List<IDecision> decisions = new List<IDecision>();
    public static event Action UpdatePlayerPos;
    public int currentScore { get; private set; }
    public ThinkBluePrint thinkBluePrint;

    //玩家是否在移动
    public bool isMoving;
    //是否可以闪避 以及 闪避是否冷却
    public bool canDodge = false;
    public bool isDodgeIgnore = false;
    public EnemyBase toDodge;
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

    #region ForAble
    
    private void OnEnable()
    {
        EventManager.SendScore += AddScore;
    }

    private void OnDisable()
    {
        EventManager.SendScore -= AddScore;
    }
    #endregion

    public void SetupBluePrint(ThinkBluePrint thinkBluePrint)
    {
        this.thinkBluePrint = thinkBluePrint;
    }

    /// <summary>
    /// 初始化Player的关卡位置
    /// </summary>
    /// <param name="thePos"></param>
    public void SetupBornPos(Vector2 thePos)
    {
        transform.position = thePos;
        UpdateGridInfo();
    }
    
    private void AddScore(int score)
    {
        currentScore += score;
    }

    public override void HandleMethod()
    {
        CheckInput();
    }

    protected override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        //更新生命值
        EventManager.CallUpdateHealth();
    }
    
    private void CheckInput()
    {
        //如果现在有输入 或者 玩家正在移动，就不进行回合
        if (hasInput || isMoving) return;
        foreach (var decision in decisions)
        {
            if (decision.Evaluate())
            {
                decision.Execute();
                break;
            }
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
    public override void UpdateGridInfo()
    {
        base.UpdateGridInfo();
        UpdatePlayerPos?.Invoke();
    }

    public void BeMove(Vector2 targetPos)
    {
        isMoving = true;
        transform.DOMove(targetPos, .49f).OnComplete(() =>
        {
            isMoving = false;
            UpdateGridInfo();
        });
    }
}
