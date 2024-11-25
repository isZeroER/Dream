using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T:Singleton<T>
{
    private static T m_Instatce;

    public static T Instance
    {
        get => m_Instatce;
    }
    protected virtual void Awake()
    {
        if (m_Instatce == null)
            m_Instatce = (T)this;
        else 
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (m_Instatce == this)
            m_Instatce = null;
    }
}
