using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VictoryPanel : BasePanel
{
    [SerializeField]
    private TextMeshProUGUI messageText;
    public void RePlay()
    {
        SceneMgr.Instance.ChangeToScene(SceneMgr.Instance.currentScene.ToString());
        Invoke(nameof(Close), 1f);
    }

    public void ToTheMainMenu()
    {
        SceneMgr.Instance.ChangeToScene(SceneName.StartScene.ToString());
        Invoke(nameof(Close), 1f);
    }

    public void NextSection()
    {
        int a = (int) SceneMgr.Instance.currentScene;
        a++;
        if (a == 3)
        {
            SetupText("没有下一章啦！");
            return;
        }
        
        SceneMgr.Instance.ChangeToScene(((SceneName)a).ToString());
        Invoke(nameof(Close), 1f);
    }

    public void SetupText(string word)
    {
        messageText.text = word;
    }
}
