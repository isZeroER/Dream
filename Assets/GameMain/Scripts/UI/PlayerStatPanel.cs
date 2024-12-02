using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerStatPanel : BasePanel
{
    [SerializeField] private Transform hearts;
    [SerializeField] private GameObject stopMenu;
    [SerializeField] private GameObject heartPrefab;
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
        //先删除所有爱心
        // 遍历所有子物体并销毁它们
        foreach (Transform child in hearts)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < Player.health; i++)
        {
            Instantiate(heartPrefab, hearts);
        }
    }

    public void VolumeControl()
    {
        
    }
    public void ShowStopMenu()
    {
        stopMenu.SetActive(true);
        // Time.timeScale = 0;
    }

    public void BackToGame()
    {
        stopMenu.SetActive(false);
        // Time.timeScale = 1;
    }
    
    public void ToTheMainMenu()
    {
        SceneMgr.Instance.ChangeToScene(SceneName.StartScene.ToString());
        stopMenu.SetActive(false);
        Invoke(nameof(Close), 1f);
    }
    
    public void RePlay()
    {
        SceneMgr.Instance.ChangeToScene(SceneMgr.Instance.currentScene.ToString());
        stopMenu.SetActive(false);
    }
}
