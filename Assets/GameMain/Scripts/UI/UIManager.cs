using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private List<GameObject> panelPrefabs = new List<GameObject>();
    private Dictionary<string, GameObject> panelPrefabsDict=new Dictionary<string, GameObject>();
    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    private Transform uiRoot;
    private Transform UIRoot
    {
        get
        {
            if (uiRoot == null)
                uiRoot = GameObject.Find("UICanvas").transform.GetChild(0);
            return uiRoot;
        }
    }
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        foreach (var panelPrefab in panelPrefabs)
            panelPrefabsDict[panelPrefab.name] = panelPrefab;
        
        OpenPanel(UIName.BeginPanel);
    }

    public BasePanel OpenPanel(string name)
    {
        if (panelDic.ContainsKey(name))
        {
            if (!panelDic[name].isOpened)
            {
                panelDic[name].Open();
                return panelDic[name];
            }
            else
                return panelDic[name];
        }
        
        if(panelPrefabsDict.TryGetValue(name, out GameObject panel))
        {
            BasePanel newPanelOpen = Instantiate(panel, UIRoot).GetComponent<BasePanel>();
            panelDic[name] = newPanelOpen;
            panelDic[name].Open();
            return newPanelOpen;
        }
        else
        {
            Debug.Log("No Panel!");
            return null;
        }
    }

    public void ClosePanel(string name)
    {
        if(panelDic.ContainsKey(name) && panelDic[name].isOpened)
            panelDic[name].Close();
    }
}

public class UIName
{
    public static string BeginPanel = "BeginPanel";
    public static string PlayerStatPanel = "PlayerStatPanel";
    public static string VictoryPanel = "VictoryPanel";
    public static string ScenePanel = "ScenePanel";
    public static string DialogPanel = "DialogPanel";
}
