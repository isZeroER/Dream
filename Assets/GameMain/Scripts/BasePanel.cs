using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    public bool isOpened { get; private set; }

    public virtual void Open()
    {
        isOpened = true;
    }

    public virtual void Close()
    {
        isOpened = false;
    }
}
