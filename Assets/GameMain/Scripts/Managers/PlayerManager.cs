using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public Player player;

    private void Start()
    {
        UIManager.Instance.OpenPanel(UIName.PlayerStatPanel);
    }

    public int GetDistance(Vector2 pos)
    {
        int distance = 0;
        distance += (int)Math.Abs(pos.x - player.transform.position.x);
        distance += (int)Math.Abs(pos.y - player.transform.position.y);
        return distance;
    }
}
