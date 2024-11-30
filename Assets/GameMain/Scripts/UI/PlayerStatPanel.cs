using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerStatPanel : BasePanel
{
    [SerializeField] private Transform hearts;

    private Player player;
    private Player Player
    {
        get
        {
            player = PlayerManager.Instance.player;
            return player;
        }
    }
    

    private void OnEnable()
    {
        EventManager.UpdateHealth += UpdatePlayerHealth;
        
        UpdatePlayerHealth();
    }

    private void OnDisable()
    {
        EventManager.UpdateHealth -= UpdatePlayerHealth;
    }

    private void UpdatePlayerHealth()
    {
        for (int i = 0; i < hearts.childCount; i++)
        {
            hearts.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < Player.health; i++)
        {
            hearts.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
