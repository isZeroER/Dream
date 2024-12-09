using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ForBounds : MonoBehaviour
{
    #if UNITY_EDITOR
    [MenuItem("Tools/RefreshBounds")]
    public static void RefreshBounds()
    {
        Tilemap[] initLevels = FindObjectsOfType<Tilemap>();
        foreach (var initLevel in initLevels)
        {
            initLevel.CompressBounds();
            Debug.Log(initLevel.name+" "+ initLevel.size);
        }
    }
    #endif
}
