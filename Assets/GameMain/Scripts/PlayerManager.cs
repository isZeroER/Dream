using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    private Player player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    public int GetDistance(Vector2 pos)
    {
        int distance = 0;
        distance += (int)Math.Abs(pos.x - player.transform.position.x);
        distance += (int)Math.Abs(pos.y - player.transform.position.y);
        return distance;
    }
}
