using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerStatPanel : BasePanel
{
    [Header("ForPlayer")]
    [SerializeField] private Transform hearts;
    [SerializeField] private GameObject heartPrefab;
    
    [Header("ForEnemy")]
    [SerializeField] private Transform enemyHearts;
    [SerializeField] private GameObject enemyHeartPrefab;
    [SerializeField] private Image enemyHead;
    
    [Header("Information")]
    [SerializeField] private TextMeshProUGUI turnText;
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
        EventManager.UpdateEnemyHealth += UpdateEnemyHealth;
        EventManager.UpdateTurnNum += UpdateTurnNum;
        UpdatePlayerHealth();
    }

    private void OnDisable()
    {
        EventManager.UpdateHealth -= UpdatePlayerHealth;
        EventManager.UpdateEnemyHealth -= UpdateEnemyHealth;
        EventManager.UpdateTurnNum -= UpdateTurnNum;
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

    private void UpdateEnemyHealth(EnemyBase enemy)
    {
        if (!enemy)
        {
            enemyHead.color = new Color(1, 1, 1, 0);
            foreach (Transform child in enemyHearts)
            {
                Destroy(child.gameObject);
            }
            return;
        }
        enemyHead.sprite = enemy.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        enemyHead.color=Color.white;
        foreach (Transform child in enemyHearts)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < enemy.health; i++)
        {
            Instantiate(enemyHeartPrefab, enemyHearts);
        }
    }

    private void UpdateTurnNum(int num)
    {
        turnText.text = $"第 {num} 回合";
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
        //将敌人的状态栏更新
        EventManager.CallUpdateEnemyHealth(null);
        EventManager.CallUpdateTurnNum(1);
        Player.SetPlayerRePlay();
        stopMenu.SetActive(false);
    }
}
