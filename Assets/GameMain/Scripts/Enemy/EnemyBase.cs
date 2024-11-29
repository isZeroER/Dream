using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class EnemyBase : Character
{
    public enum EnemyType
    {
        InactiveEndorphin,
        Glycine,
        Endorphin,
        Acetylcholine,
        EmptyCoffeeCup,
        CloudCoffee,
        SparklingSugarCube,
        SugarCubeBox,
        FlowingColorCoffeeMachine,
        BigOrange,
        AngryBigOrange
    }
    protected Player player;
    private bool findPlayer;
    private int enemyBox = 5;
    public bool canPatrol;
    private bool isHating;
    private int enemyScore;

    public bool mainTurn;

    public EnemyType enemyType;
    //游走路线
    private List<Vector2> pathDir = new List<Vector2>();
    private int currentPath = 0;
    protected override void Start()
    {
        base.Start();
        SetBaseStat(3, 0, 0);

        //确定是敌人
        characterType = CharacterType.Enemy;
        //获取当前瓦片
        currentGrid = GridManager.Instance.GetGridByPos(transform.position);
        GridManager.Instance.ChangeGridInfo(transform.position, characterType);

        player = PlayerManager.Instance.player;
        
        InitEnemy();
    }
    protected void SetBaseStat(int targetHealth, int targetStrength, int targetScore)
    {
        health = targetHealth;
        strength = targetStrength;
        enemyScore = targetScore;
    }

    public void SetupBorn(Vector2 pos, List<Vector2> pathDir, bool canPatrol, bool isHating)
    {
        transform.position = pos;
        this.pathDir = pathDir;
        this.canPatrol = canPatrol;
        this.isHating = isHating;
        UpdateGridInfo();
    }
    
    protected abstract void InitEnemy();
    #region ForAble

    private void OnEnable()
    {
        Player.UpdatePlayerPos += CheckPlayer;
    }

    private void OnDisable()
    {
        Player.UpdatePlayerPos -= CheckPlayer;
    }

    #endregion

    #region EnemyState
    
    public override void HandleMethod()
    {
        base.HandleMethod();
        CheckPlayer();
        // Debug.Log(enemyType + "Handling~" + canPatrol);
        if (findPlayer && isHating && mainTurn)
        {
            //判定是否可以攻击
            if (CanAttack())
            {
                Attack();
            }
            else
            {
                //带仇恨锁敌的寻路
                HatingPatrol();
            }
            
        }
        else
        {
            //可以游行
            if(canPatrol)
                Patrol();
        }

        //回合结束，状态清除
        ClearStat();
    }

    private void ClearStat()
    {
        mainTurn = false;
    }
    protected virtual void CheckPlayer()
    {
        Vector2 playerPos = player.transform.position;
        Vector2 enemyPos = transform.position;
        if(Math.Abs(playerPos.x-enemyPos.x)+Math.Abs(playerPos.y-enemyPos.y) < 2)
        {
            findPlayer = true;
        }
        else
        {
            findPlayer = false;
        }
    }

    protected virtual void Patrol()
    {
        Vector2 theDir = pathDir[currentPath];
        Vector2 targetPos = new Vector2(transform.position.x + theDir.x, transform.position.y + theDir.y);
        //如果目标路径不为空，则不动
        if (GridManager.Instance.GetGridByPos(targetPos)!=null && GridManager.Instance.GetGridByPos(targetPos).characterType != CharacterType.None)
            return;
        transform.DOMove(targetPos, 0.5f).OnComplete(UpdateGridInfo);
        currentPath++;
        if (currentPath == pathDir.Count)
            currentPath = 0;
    }
    
    protected abstract bool CanAttack();
    protected abstract void Attack();
    
    protected abstract void HatingPatrol();

    protected override void Die()
    {
        EnemyManager.Instance.RemoveEnemy(this);
        EventManager.CallSendScore(enemyScore);
        base.Die();
    }
    #endregion
    
}
