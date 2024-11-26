using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : Singleton<EnemyManager>
{
    private List<Enemy> enemies = new List<Enemy>();
    private Enemy closestEnemy;
    private void Start()
    {
        GameObject[] enemyGameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemyGameObject in enemyGameObjects)
        {
            enemies.Add(enemyGameObject.GetComponent<Enemy>());
        }
    }

    public void HandleTurn()
    {
        closestEnemy = FindCloseEnemy();
        if (closestEnemy == null)
        {
            //TODO:结束UI
            Debug.Log("游戏结束！！！");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }
        closestEnemy.HandleMethod();

    }

    private Enemy FindCloseEnemy()
    {
        int closeDistance = Int32.MaxValue;
        Enemy closestEnemy = null;
        foreach (var enemy in enemies)
        {
            int distance = PlayerManager.Instance.GetDistance(enemy.transform.position);
            //如果距离更小
            if (distance < closeDistance)
            {
                closeDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
    }
}
