using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour, ISaveManager
{
    public bool isOpened { get; private set; }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    public virtual void Open()
    {
        isOpened = true;
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        isOpened = false;
        gameObject.SetActive(false);
    }

    public void LoadData(GameData data)
    {
        
    }

    public void SaveData(ref GameData data)
    {
        
    }
}
