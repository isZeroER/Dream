using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : Singleton<EnemyManager>
{
    //现有所有敌人
    public List<EnemyBase> currentExistsEnemies = new List<EnemyBase>();
    //可以回合外自移动的敌人
    private List<EnemyBase> canPatrolEnemies = new List<EnemyBase>();
    //最近的敌人
    private EnemyBase _closestEnemyBase;
    //角色根物体
    private Transform characterRoot;

    private Transform CharacterRoot
    {
        get
        {
            if (characterRoot == null)
                characterRoot = GameObject.Find("Characters").transform;
            return characterRoot;
        }
    }
    //敌人预制体SO
    public EnemiesPrefabs enemiesPrefabs;
    //敌人预制体字典
    private Dictionary<string, GameObject> emPrefabDict = new Dictionary<string, GameObject>();
    protected override void Awake()
    {
        base.Awake();
        foreach (var enemyPrefab in enemiesPrefabs.enemies)
        {
            emPrefabDict[enemyPrefab.name] = enemyPrefab;
        }
        RefreshEnemies();
    }

    /// <summary>
    /// 根据EnemySetting来生成敌人
    /// </summary>
    /// <param name="enemySetting"></param>
    public void GenerateEnemy(LevelSet.EnemySettings enemySetting)
    {
        EnemyBase newEnemy = Instantiate(emPrefabDict[enemySetting.enemyType.ToString()], CharacterRoot).GetComponent<EnemyBase>();
        newEnemy.SetupBorn(enemySetting.bornPoint, enemySetting.directions, enemySetting.isMoveAside, enemySetting.isMoveHating, enemySetting.needClear);
    }

    public void ClearAllEnemies()
    {
        foreach (var enemy in currentExistsEnemies)
        {
            Destroy(enemy.gameObject);
        }
        currentExistsEnemies.Clear();
        canPatrolEnemies.Clear();
    }

    private void RefreshEnemies()
    {
        currentExistsEnemies.Clear();
        GameObject[] enemyGameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemyGameObject in enemyGameObjects)
        {
            // Debug.Log(enemyGameObject.name);
            currentExistsEnemies.Add(enemyGameObject.GetComponent<EnemyBase>());
        }
    }

    public void HandleTurn()
    {
        RefreshEnemies();
        _closestEnemyBase = FindCloseEnemy();
        if (_closestEnemyBase == null)
        {
            //TODO:清楚完才可以结束
            return;
        }

        // _closestEnemyBase.mainTurn = true;
        
        //只有最近的怪物可以触发回合
        // _closestEnemyBase.HandleMethod();

        //直接所有怪物进行决策
        foreach (var existsEnemy in currentExistsEnemies)
        {
            if (existsEnemy != null)
                existsEnemy.HandleMethod();
        }
    }

    /// <summary>
    /// 这里获取最近的敌人
    /// 同时获取所有含自由行动的敌人，在每个回合调用
    /// </summary>
    /// <returns></returns>
    private EnemyBase FindCloseEnemy()
    {
        int closeDistance = Int32.MaxValue;
        EnemyBase closestEnemyBase = null;
        foreach (var enemy in currentExistsEnemies)
        {
            if (enemy.canPatrol)
                canPatrolEnemies.Add(enemy);
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
        if (canPatrolEnemies.Contains(enemyBase))
            canPatrolEnemies.Remove(enemyBase);
        currentExistsEnemies.Remove(enemyBase);
    }
}
