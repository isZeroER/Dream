using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private List<BasePanel> panels;
    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();
    private Transform UIRoot;
    protected override void Awake()
    {
        base.Awake();
        foreach (var panel in panels)
            panelDic.Add(panel.name, panel);
    }

    public void OpenPanel(string name)
    {
        if(panelDic.TryGetValue(name, out BasePanel panel))
            panel.Open();
        else 
            Debug.Log("No Panel!");
    }

    public void ClosePanel(string name)
    {
        if(panelDic.ContainsKey(name) && panelDic[name].isOpened)
            panelDic[name].Close();
            
    }
}

public class UIName
{
    public static string PlayerStat = "PlayerStat";
    public static string Setting = "Setting";
}
