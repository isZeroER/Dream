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
    private bool findPlayer;
    private int enemyBox = 5;

    public EnemyType enemyType;
    
    public override void HandleMethod()
    {
        base.HandleMethod();
        CheckPlayer();
    }

    // public abstract void ();

    protected virtual void Start()
    {
        SetHealth_Strenth(3, 2);

        characterType = CharacterType.Enemy;
        currentGrid = GridManager.Instance.GetGridByPos(transform.position);
        GridManager.Instance.ChangeGridInfo(transform.position, characterType);
    }

    protected void SetHealth_Strenth(int targetHealth, int targetStrength)
    {
        health = targetHealth;
        strength = targetStrength;
    }

    protected virtual void CheckPlayer()
    {
        
    }

    protected override void Die()
    {
        EnemyManager.Instance.RemoveEnemy(this);
        base.Die();
    }
}
