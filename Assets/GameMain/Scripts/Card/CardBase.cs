using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardBase : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    private Image image;
    private void Start()
    {
        image = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color=Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color=Color.white;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        //卡牌点击后，可以在地图上有一些点击
    }
}
