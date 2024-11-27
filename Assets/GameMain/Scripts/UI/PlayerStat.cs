using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerStat : BasePanel
{
    [SerializeField] private Transform hearts;
    [SerializeField] private Player player;

    private void Update()
    {
        for (int i = 0; i < hearts.childCount; i++)
        {
            hearts.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < player.health; i++)
        {
            hearts.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
