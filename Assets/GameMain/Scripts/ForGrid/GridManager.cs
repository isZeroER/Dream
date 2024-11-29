using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : Singleton<GridManager>
{
    [SerializeField] private Tilemap[] wholeTilemaps;
    [SerializeField] private Tilemap forTip;
    [SerializeField] private TileBase highlightTile;
    [SerializeField] private TileBase enemyHighlightTile;
    
    private Tilemap currentMap;
    private int currentTileMapId = 0;
    private BoundsInt mapBounds;
    private Dictionary<int, GridInfo> tileDict = new Dictionary<int, GridInfo>();

    protected override void Awake()
    {
        base.Awake();
        currentMap = wholeTilemaps[currentTileMapId];
        currentMap.gameObject.SetActive(true);
        GetTileMap();
        InitLevel initLevel = currentMap.gameObject.GetComponentInChildren<InitLevel>();
        InitTheLevel(initLevel.levelSet);
    }


    private void OnEnable()
    {
        EventManager.NextTileMap += NextTileMap;
    }

    private void OnDisable()
    {
        EventManager.NextTileMap -= NextTileMap;
    }

    private void NextTileMap()
    {
        currentMap.gameObject.SetActive(false);
        tileDict.Clear();
        //先将所有上一关卡怪物清理
        EnemyManager.Instance.ClearAllEnemies();
        //下一关卡
        currentTileMapId++;
        Debug.Log(currentTileMapId);
        //如果等于，说明没有下一关，本关结束
        if (currentTileMapId == wholeTilemaps.Length)
        {
            Debug.Log("本章完结！");
            return;
        }
        currentMap = wholeTilemaps[currentTileMapId];
        currentMap.gameObject.SetActive(true);
        GetTileMap();
        
        InitLevel initLevel = currentMap.gameObject.GetComponentInChildren<InitLevel>();
        InitTheLevel(initLevel.levelSet);
    }

    /// <summary>
    /// 关卡设置
    /// </summary>
    /// <param name="levelSet"></param>
    private void InitTheLevel(LevelSet levelSet)
    {
        //设置玩家地点
        PlayerManager.Instance.player.SetupBornPos(levelSet.playerBorn);
        foreach (var enemySetting in levelSet.EnemySettingsList)
        {
            EnemyManager.Instance.GenerateEnemy(enemySetting);
        }
    }

    /// <summary>
    /// 初始化格子
    /// </summary>
    private void GetTileMap()
    {
        mapBounds = currentMap.cellBounds;

        //获取包围盒的数组
        TileBase[] allTiles = currentMap.GetTilesBlock(mapBounds);

        for (int x = mapBounds.xMin; x < mapBounds.size.x + mapBounds.xMin; x++)
        {
            for (int y = mapBounds.yMin; y < mapBounds.size.y + mapBounds.yMin; y++)
            {
                int index = GetTileIndex(x, y, mapBounds);

                TileBase tileBase = allTiles[index];

                if (tileBase != null && tileBase is Tile tile)
                {
                    GridInfo newGridInfo = new GridInfo(tile, new Vector2(x, y));
                    tileDict[index] = newGridInfo;
                }
            }
        }
    }
    
    private int GetTileIndex(int x, int y, BoundsInt bounds)
    {
        if (x < bounds.xMin || x > bounds.xMax || y < bounds.yMin || y > bounds.yMax) 
            return -1;
        //行数 * 每行个数，加上这行前面的个数
        return (y - bounds.yMin) * bounds.size.x + (x - bounds.xMin);
    }


    #region GetGrid

    public GridInfo GetGridByPos(Vector2 pos)
    {
        Vector2 realPos = new Vector2((int)Math.Round(pos.x), (int)Math.Round(pos.y));
        Vector3Int gridPos = currentMap.WorldToCell(realPos);
        int index = GetTileIndex(gridPos.x, gridPos.y, mapBounds);
        
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
    
    /// <summary>
    /// 获取此格子四周 有敌人的格子
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public GridInfo[] GetAdjacentGrids(Vector2 pos)
    {
        List<GridInfo> grids = new List<GridInfo>();
        GridInfo up = GetGridByPos(pos + Vector2.up);
        GridInfo down = GetGridByPos(pos + Vector2.down);
        GridInfo left = GetGridByPos(pos + Vector2.left);
        GridInfo right = GetGridByPos(pos + Vector2.right);
        grids.Add(up);
        grids.Add(down);
        grids.Add(left);
        grids.Add(right);

        return grids.ToArray();
    }

    #endregion

    #region 改变格子信息
    
    public void ChangeGridInfo(Vector2 pos, Character.CharacterType state)
    {
        GridInfo gridInfo = GetGridByPos(pos);
        ChangeGridInfo(gridInfo, state);
    }

    public void ChangeGridInfo(GridInfo gridInfo, Character.CharacterType state)
    {
        //这个格子有角色了
        if (gridInfo != null)
            gridInfo.characterType = state;
        else
            Debug.LogWarning("GridInfo is null");
    }

    #endregion

    #region 高光显示周围格子

    /// <summary>
    /// 判断目标格子是否可以行走，顺便把敌人显示了
    /// </summary>
    /// <param name="theTarget"></param>
    /// <returns></returns>
    public bool WalkToCheck_EnemyCheck(Vector2 theTarget)
    {
        Vector3Int target = new Vector3Int((int)theTarget.x, (int)theTarget.y, 0);
        GridInfo targetGrid = GetGridByPos(theTarget);
        //判断格子信息，是否为空，是否为可走型，是否有敌人
        if (targetGrid != null && targetGrid.characterType == Character.CharacterType.None && targetGrid.gridType == GridType.CanWalk)
        {
            forTip.SetTile(target, highlightTile);
            return true;
        }
        else
        {
            if (targetGrid == null)
                return false;
            CheckisEnemy(targetGrid, new Vector2(target.x, target.y));
            return false;
        }
    }

    public bool CheckisEnemy(Vector2 theTarget)
    {
        return CheckisEnemy(GetGridByPos(theTarget), theTarget);
    }
    private bool CheckisEnemy(GridInfo targetGrid, Vector2 theTarget)
    {
        Vector3Int target = new Vector3Int((int)theTarget.x, (int)theTarget.y, 0);
        if (targetGrid == null)
            return false;
        if (targetGrid.characterType == Character.CharacterType.Enemy)
        {
            forTip.SetTile(target, enemyHighlightTile);
            return true;
        }
        return false;
    }

    #endregion
    public void ClearTip() => forTip.ClearAllTiles();
}