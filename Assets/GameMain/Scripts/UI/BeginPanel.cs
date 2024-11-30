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
        SceneMgr.Instance.ChangeToScene("Section_" + sceneName);
    }

    public void NewGame()
    {
        
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
