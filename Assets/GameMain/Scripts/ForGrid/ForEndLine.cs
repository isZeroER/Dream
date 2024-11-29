using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForEndLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(CoNextLevel());
    }

    IEnumerator CoNextLevel()
    {
        yield return new WaitForSeconds(1f);
        EventManager.CallNextTileMap();
    }
}
