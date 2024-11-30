using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryPanel : BasePanel
{
    public void RePlay()
    {
        SceneMgr.Instance.ChangeToScene(SceneMgr.Instance.currentScene.ToString());
        Close();
    }

    public void ToTheMainMenu()
    {
        SceneMgr.Instance.ChangeToScene(SceneName.StartScene.ToString());
        Close();
    }

    public void NextSection()
    {
        int a = (int) SceneMgr.Instance.currentScene;
        if (a == 4)
        {
            Debug.Log("没有下一关了！");
            return;
        }
        SceneMgr.Instance.ChangeToScene(((SceneName)a).ToString());
        Close();
    }
}
