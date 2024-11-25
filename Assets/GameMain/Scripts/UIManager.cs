using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private List<BasePanel> panels;
    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();
    private void Start()
    {
        foreach (var panel in panels)
            panelDic.Add(panel.name, panel);
    }

    public void OpenPanel(string name)
    {
        if(panelDic.ContainsKey(name))
            panelDic[name].Open();
        else 
            Debug.Log("No Panel!");
    }

    public void ClosePanel(string name)
    {
        if(panelDic.ContainsKey(name) && panelDic[name].isOpened)
            panelDic[name].Close();
            
    }
}
