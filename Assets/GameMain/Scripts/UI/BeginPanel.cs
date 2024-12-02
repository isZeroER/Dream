using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BeginPanel : BasePanel
{
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject sectionSelect;
    public void PlaySection(int sceneName)
    {
        if(sceneName == 0)
            AudioManager.Instance.PlayBGM(6);
        if(sceneName == 1)
            AudioManager.Instance.PlayBGM(4);
        if(sceneName == 2)
            AudioManager.Instance.PlayBGM(1);
        SceneMgr.Instance.ChangeToScene("Section_" + sceneName);
        Close();
    }

    public void NewGame()
    {
        SceneMgr.Instance.ChangeToScene(SceneName.Section_0.ToString());
        Close();
    }

    public void ChooseSection()
    {
        options.SetActive(false);
        sectionSelect.SetActive(true);
    }

    public void BackToMenu()
    {
        options.SetActive(true);
        sectionSelect.SetActive(false);
    }

    public void About()
    {
        
    }
    
    public void Setting()
    {
        
    }
}
