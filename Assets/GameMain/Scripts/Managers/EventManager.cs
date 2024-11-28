using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    //更新血条UI
    public static event Action UpdateHealth;

    public static void CallUpdateHealth()
    {
        UpdateHealth?.Invoke();
    }
    
    //玩家得分
    public static event Action<int> SendScore;

    public static void CallSendScore(int score)
    {
        SendScore?.Invoke(score);
    }
    
    //下一关卡
    public static event Action NextTileMap;

    public static void CallNextTileMap()
    {
        NextTileMap?.Invoke();
    }
    
}
