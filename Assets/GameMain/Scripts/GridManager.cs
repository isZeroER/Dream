using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : Singleton<GridManager>
{
    private Dictionary<int, GridInfo> tileDict = new Dictionary<int, GridInfo>();
    // private Dictionary<int, GridInfo> tipDict = new Dictionary<int, GridInfo>();
    
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private Tilemap forTip;
    [SerializeField] private TileBase highlightTile;

    protected override void Awake()
    {
        base.Awake();
        GetTileMap();
    }

    private void GetTileMap()
    {
        BoundsInt bounds = tileMap.cellBounds;

        TileBase[] allTiles = tileMap.GetTilesBlock(bounds);

        for (int x = bounds.xMin; x < bounds.size.x; x++)
        {
            for (int y = bounds.yMin; y < bounds.size.y; y++)
            {
                int index = GetTileIndex(x, y, bounds);

                TileBase tileBase = allTiles[index];

                if (tileBase != null && tileBase is Tile tile)
                {
                    GridInfo newGridInfo = new GridInfo(tile);
                    tileDict[index] = newGridInfo;
                }
            }
        }
    }
    
    private int GetTileIndex(int x, int y, BoundsInt bounds)
    {
        return (y - bounds.yMin) * bounds.size.x + (x - bounds.xMin);
    }


    #region GetGrid

    public GridInfo GetGridByPos(Vector2 pos)
    {
        Vector3Int gridPos = tileMap.WorldToCell(pos);
        int index = gridPos.y * tileMap.cellBounds.size.x + gridPos.x;
        
        return GetGridByInt(index);
    }
    
    public GridInfo GetGridByInt(int index)
    {
        if (tileDict.TryGetValue(index, out GridInfo tile))
        {
            return tile;
        }
        else
        {
            return null;
        }
    }

    #endregion

    public bool CanWalkTo(Vector2 theTarget)
    {
        Vector3Int target = new Vector3Int((int)theTarget.x, (int)theTarget.y, 0);
        GridInfo targetGrid = GetGridByPos(theTarget);
        //判断格子信息，是否为空，是否为可走型，是否有敌人
        if (targetGrid != null && !targetGrid.hasCharacter && targetGrid.gridType == GridType.CanWalk)
        {
            forTip.SetTile(target, highlightTile);
            return true;
        }
        else
            return false;
    }

    public void ChangeGridInfo(Vector2 pos, bool flag)
    {
        GridInfo gridInfo = GetGridByPos(pos);
        ChangeGridInfo(gridInfo, flag);
    }

    public void ChangeGridInfo(GridInfo gridInfo, bool flag)
    {
        //这个格子有角色了
        if(gridInfo!=null)
            gridInfo.hasCharacter = flag;
    }

    public void ShowAround(Vector2 pos)
    {
        CanWalkTo(pos + Vector2.up);
        CanWalkTo(pos + Vector2.down);
        CanWalkTo(pos + Vector2.left);
        CanWalkTo(pos + Vector2.right);
    }
    
    public void ClearTip() => forTip.ClearAllTiles();
}