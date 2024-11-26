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
    public Character.CharacterType characterType;
    
    public Sprite currentSprite;
    public Tile tile;
    public Character character;
    public Vector2 position;

    public GridInfo(Tile tile, Vector2 position)
    {
        this.tile = tile;
        this.position = position;
        gridType = GridType.CanWalk;
        characterType = Character.CharacterType.None;
    }

    public void SetSprite(Sprite sprite)
    {
        currentSprite = sprite;
        tile.sprite = sprite;
    }
}
