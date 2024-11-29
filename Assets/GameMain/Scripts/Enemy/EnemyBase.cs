using System;
using System.Collections;
using System.Collections.Generic;
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
    protected bool canPatrol;
    private int enemyScore;

    public EnemyType enemyType;
    //游走路线
    private List<Vector2> pathDir = new List<Vector2>();
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

    public void SetupBorn(Vector2 pos, List<Vector2> pathDir)
    {
        transform.position = pos;
        this.pathDir = pathDir;
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
        if (findPlayer)
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

    protected abstract bool CanAttack();
    protected abstract void Attack();
    protected abstract void Patrol();
    protected abstract void HatingPatrol();

    protected override void Die()
    {
        EnemyManager.Instance.RemoveEnemy(this);
        EventManager.CallSendScore(enemyScore);
        base.Die();
    }
    #endregion
    
}
