using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemiesPrefabs", menuName = "EnemiesPrefabs")]
public class EnemiesPrefabs:ScriptableObject
{
    public List<GameObject> enemies = new List<GameObject>();
}