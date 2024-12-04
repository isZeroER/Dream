using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    }

    private void Start()
    {
        OpenPanel(UIName.BeginPanel);
    }

    public BasePanel OpenPanel(string name)
    {
        if (panelDic.ContainsKey(name))
        {
            if (!panelDic[name].isOpened)
            {
                panelDic[name].Open();
                if (name == UIName.VolumePanel)
                {
                    panelDic[name].transform.SetAsLastSibling();
                }
                return panelDic[name];
            }
            else
            {
                return panelDic[name];
            }
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

#if UNITY_EDITOR
    static Vector3[] corners = new Vector3[4];
    private void OnDrawGizmos()
    {
        foreach (MaskableGraphic g in GetComponentsInChildren<MaskableGraphic>())
        {
            if (g.raycastTarget)
            {
                RectTransform rectTransform = g.transform as RectTransform;
                rectTransform.GetWorldCorners(corners);
                Gizmos.color = Color.blue;
                for (int i = 0; i < 4; i++)
                {
                    Gizmos.DrawLine(corners[i], corners[(i + 1) % 4]);
                }
            }
        }
    }
#endif
}

public class UIName
{
    public static string BeginPanel = "BeginPanel";
    public static string PlayerStatPanel = "PlayerStatPanel";
    public static string VictoryPanel = "VictoryPanel";
    public static string ScenePanel = "ScenePanel";
    public static string VolumePanel = "VolumePanel";
    public static string DialogPanel = "DialogPanel";
}
