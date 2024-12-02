using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerStatPanel : BasePanel
{
    [SerializeField] private Transform hearts;
    [SerializeField] private GameObject stopMenu;
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

    public void VolumeControl()
    {
        
    }
    public void ShowStopMenu()
    {
        stopMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void BackToGame()
    {
        stopMenu.SetActive(false);
        Time.timeScale = 1;
    }
    
    public void ToTheMainMenu()
    {
        Debug.Log("TOMAINMENU");
        SceneMgr.Instance.ChangeToScene(SceneName.StartScene.ToString());
        stopMenu.SetActive(false);
        Close();
    }
    
    public void RePlay()
    {
        SceneMgr.Instance.ChangeToScene(SceneMgr.Instance.currentScene.ToString());
        stopMenu.SetActive(false);
    }
}
