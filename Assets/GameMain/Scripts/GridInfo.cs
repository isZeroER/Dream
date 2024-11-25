using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public enum GridType
{
    CanWalk,
    Obstacle
}

public class GridInfo
{
    public GridType gridType;
    public bool hasCharacter;
    public Sprite characterSprite;
    public Tile tile;

    public GridInfo(Tile tile)
    {
        this.tile = tile;
        gridType = GridType.CanWalk;
    }
}
