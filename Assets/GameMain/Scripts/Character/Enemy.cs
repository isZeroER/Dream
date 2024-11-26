using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    private bool findPlayer;
    private int enemyBox = 5;
    public override void HandleMethod()
    {
        base.HandleMethod();
        CheckPlayer();
    }

    private void Start()
    {
        characterType = CharacterType.Enemy;
        currentGrid = GridManager.Instance.GetGridByPos(transform.position);
        GridManager.Instance.ChangeGridInfo(transform.position, characterType);
    }

    private void CheckPlayer()
    {
        // DoDamage();
    }

    protected override void Die()
    {
        EnemyManager.Instance.RemoveEnemy(this);
        base.Die();
    }
}
