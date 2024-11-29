using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
