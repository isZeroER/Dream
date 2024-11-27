using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : Singleton<EnemyManager>
{
    private List<EnemyBase> enemies = new List<EnemyBase>();
    private EnemyBase _closestEnemyBase;
    private void Start()
    {
        GameObject[] enemyGameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemyGameObject in enemyGameObjects)
        {
            enemies.Add(enemyGameObject.GetComponent<EnemyBase>());
        }
    }

    public void HandleTurn()
    {
        _closestEnemyBase = FindCloseEnemy();
        if (_closestEnemyBase == null)
        {
            //TODO:结束UI
            Debug.Log("游戏结束！！！");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }
        _closestEnemyBase.HandleMethod();

    }

    private EnemyBase FindCloseEnemy()
    {
        int closeDistance = Int32.MaxValue;
        EnemyBase closestEnemyBase = null;
        foreach (var enemy in enemies)
        {
            int distance = PlayerManager.Instance.GetDistance(enemy.transform.position);
            //如果距离更小
            if (distance < closeDistance)
            {
                closeDistance = distance;
                closestEnemyBase = enemy;
            }
        }

        return closestEnemyBase;
    }

    public void RemoveEnemy(EnemyBase enemyBase)
    {
        enemies.Remove(enemyBase);
    }
}
