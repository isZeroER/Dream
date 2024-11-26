using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [SerializeField] private GameObject heart;
    [SerializeField] private Player player;

    private void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < player.health; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
