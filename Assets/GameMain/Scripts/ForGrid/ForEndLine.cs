using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForEndLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("终点");
        if(CheckEnemy())
            StartCoroutine(CoNextLevel());
    }

    IEnumerator CoNextLevel()
    {
        yield return new WaitForSeconds(1f);
        SceneMgr.Instance.FadeInOutWithCall(EventManager.CallNextTileMap);
    }

    private bool CheckEnemy()
    {
        foreach (var enemy in EnemyManager.Instance.currentExistsEnemies)
        {
            if (enemy.needClear)
                return false;
        }

        return true;
    }
}
