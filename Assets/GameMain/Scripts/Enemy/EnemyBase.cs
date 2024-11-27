using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : Character
{
    protected Player player;
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
    private bool findPlayer;
    private int enemyBox = 5;

    public EnemyType enemyType;
    protected virtual void Start()
    {
        SetHealth_Strenth(3, 2);

        //确定是敌人
        characterType = CharacterType.Enemy;
        //获取当前瓦片
        currentGrid = GridManager.Instance.GetGridByPos(transform.position);
        GridManager.Instance.ChangeGridInfo(transform.position, characterType);

        player = PlayerManager.Instance.player;
    }
    protected void SetHealth_Strenth(int targetHealth, int targetStrength)
    {
        health = targetHealth;
        strength = targetStrength;
    }
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
            if (CanAttack())
            {
                Attack();
            }
            else
            {
                HatingPatrol();
            }
            
        }
        else
        {
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
    protected virtual bool CanAttack()
    {
        return true;
    }
    protected virtual void Attack()
    {
        
    }

    protected virtual void Patrol()
    {
        
    }

    protected virtual void HatingPatrol()
    {
        
    }

    protected override void Die()
    {
        EnemyManager.Instance.RemoveEnemy(this);
        base.Die();
    }
    #endregion
    
}
