using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : Singleton<EnemyManager>
{
    private List<EnemyBase> enemies = new List<EnemyBase>();
    private EnemyBase _closestEnemyBase;

    private List<EnemyBase> patrolEnemies = new List<EnemyBase>();
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
            //TODO:结算
            // Debug.Log("游戏结束！！！");
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }
        
        //只有最近的怪物可以触发回合
        _closestEnemyBase.HandleMethod();

    }

    /// <summary>
    /// 这里获取最近的敌人
    /// TODO：后面可以获取所有含自由行动的敌人，在每个回合调用
    /// </summary>
    /// <returns></returns>
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
