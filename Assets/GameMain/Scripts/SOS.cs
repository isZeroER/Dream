using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Think BluePrint", menuName = "Think BluePrint/Player BluePrint")]
public class ThinkBluePrint:ScriptableObject
{
    public List<Vector2> points = new List<Vector2>();
    public virtual void Skill(){}
}

[CreateAssetMenu(fileName = "LevelSet", menuName = "LevelSet")]
public class LevelSet : ScriptableObject
{
    [Serializable]
    public class EnemySettings
    {
        [Header("敌人种类")]
        public EnemyBase.EnemyType enemyType;
        [Header("敌人出生点")]
        public Vector2 bornPoint;
        [Header("基本路线")]
        public List<Vector2> directions = new List<Vector2>();
        [Header("是否循环路线")]
        public bool isLoop;
    }

    [Header("玩家出生位置")]
    public Vector2 playerBorn;
    [Header("敌人设置")]
    public List<EnemySettings> EnemySettingsList = new List<EnemySettings>();
}

[CreateAssetMenu(fileName = "EnemiesPrefabs", menuName = "EnemiesPrefabs")]
public class EnemiesPrefabs:ScriptableObject
{
    public List<GameObject> enemies = new List<GameObject>();
}