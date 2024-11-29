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
        [Header("是否回合外移动")]
        public bool isMoveAside;
        [Header("是否仇恨移动")]
        public bool isMoveHating;
    }

    [Header("玩家出生位置")]
    public Vector2 playerBorn;
    [Header("敌人设置")]
    public List<EnemySettings> EnemySettingsList = new List<EnemySettings>();
}
