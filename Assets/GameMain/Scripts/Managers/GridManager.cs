using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : Singleton<GridManager>
{
    [SerializeField] private Tilemap[] wholeTilemaps;
    [SerializeField] private Tilemap forTip;
    [SerializeField] private Tilemap enemyRoute;
    
    [SerializeField] private TileBase highlightTile;
    [SerializeField] private TileBase enemyHighlightTile;
    [SerializeField] private TileBase[] enemyRouteHighlightTile;
    
    public Tilemap currentMap;
    private LevelSet levelSet;
    private int currentTileMapId = 0;
    private BoundsInt mapBounds;
    private Dictionary<int, GridInfo> tileDict = new Dictionary<int, GridInfo>();

    protected override void Awake()
    {
        base.Awake();
        currentMap = wholeTilemaps[currentTileMapId];
        currentMap.gameObject.SetActive(true);
        GetTileMap();
        
        levelSet = currentMap.gameObject.GetComponentInChildren<InitLevel>().levelSet;
        InitTheLevel();
    }

    #region FORABLE

    private void OnEnable()
    {
        EventManager.NextTileMap += NextTileMap;
    }

    private void OnDisable()
    {
        EventManager.NextTileMap -= NextTileMap;
    }

    #endregion

    public List<GridInfo> FindPath(Vector2 startPos, Vector2 endPos)
    {
        AStar aStar = new AStar(this);
        return aStar.FindPath(startPos, endPos);
    }

    #region FORLEVEL

    private void NextTileMap()
    {
        currentMap.gameObject.SetActive(false);
        tileDict.Clear();
        //提示格也全都删除
        forTip.ClearAllTiles();
        //先将所有上一关卡怪物清理
        EnemyManager.Instance.ClearAllEnemies();
        //清理上一关怪物头像和血条
        EventManager.CallUpdateEnemyHealth(null);
        //下一关卡
        currentTileMapId++;
        //如果等于，说明没有下一关，本关结束
        if (currentTileMapId == wholeTilemaps.Length)
        {
            //TODO:胜利界面
            VictoryPanel vp = UIManager.Instance.OpenPanel(UIName.VictoryPanel) as VictoryPanel;
            vp.SetupText("胜利！");
            
            ScenePanel sp = UIManager.Instance.OpenPanel(UIName.ScenePanel) as ScenePanel;
            
            if(SceneMgr.Instance.currentScene == SceneName.Section_0)
                sp.SetupMessage(s1);
            if(SceneMgr.Instance.currentScene == SceneName.Section_1)
                sp.SetupMessage(s2);
            if(SceneMgr.Instance.currentScene == SceneName.Section_2)
                sp.SetupMessage(s3);
            
            return;
        }
        currentMap = wholeTilemaps[currentTileMapId];
        currentMap.gameObject.SetActive(true);
        GetTileMap();
        
        levelSet = currentMap.gameObject.GetComponentInChildren<InitLevel>().levelSet;
        //TODO:后续做成淡入淡出
        InitTheLevel();
    }

    /// <summary>
    /// 关卡设置
    /// </summary>
    /// <param name="levelSet"></param>
    private void InitTheLevel()
    {
        TurnManager.Instance.InitTurnNum();
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
    

    #endregion
    
    #region GetGrid

    private int GetTileIndex(int x, int y, BoundsInt bounds)
    {
        if (x < bounds.xMin || x > bounds.xMax || y < bounds.yMin || y > bounds.yMax) 
            return -1;
        //行数 * 每行个数，加上这行前面的个数
        return (y - bounds.yMin) * bounds.size.x + (x - bounds.xMin);
    }
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

        grids = grids.OrderBy(x => UnityEngine.Random.value).ToList();
        
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

    /// <summary>
    /// 显示敌人路线格子
    /// </summary>
    /// <param name="routePath"></param>
    public void SetEnemyRoute(List<GridInfo> routePath)
    {
        for (int i = 0; i < routePath.Count - 1; i++)  // 遍历到倒数第二个格子
        {
            Vector2 currentPos = routePath[i].position;
            Vector2 nextPos = routePath[i + 1].position;

            // 在当前位置生成箭头
            if (i != routePath.Count - 2)
                PlaceArrowAtPosition(currentPos, nextPos, false);
            else
                PlaceArrowAtPosition(currentPos, nextPos, true);
        }
    }
    // 在指定位置放置箭头，并根据方向调整朝向
    private void PlaceArrowAtPosition(Vector2 currentPos, Vector2 nextPos, bool lastOne)
    {
        // 计算箭头的朝向
        Vector2 direction = nextPos - currentPos;  // 计算当前格子到下一个格子的方向
        
        Vector3Int target = new Vector3Int((int)currentPos.x, (int)currentPos.y, 0);
        
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) // 主要沿水平方向
        {
            if (direction.x > 0)
            {
                if(lastOne)
                    enemyRoute.SetTile(target, enemyRouteHighlightTile[3]); // 右
                else 
                    enemyRoute.SetTile(target, enemyRouteHighlightTile[5]); // 横着
            }
            else
            {
                if(lastOne)
                    enemyRoute.SetTile(target, enemyRouteHighlightTile[2]); // 左
                else
                    enemyRoute.SetTile(target, enemyRouteHighlightTile[5]); // 横着
            }
        }
        else // 主要沿垂直方向
        {
            if (direction.y > 0)
            {
                if(lastOne)
                    enemyRoute.SetTile(target, enemyRouteHighlightTile[0]); // 上
                else 
                    enemyRoute.SetTile(target, enemyRouteHighlightTile[4]); // 竖着
            }
            else
            {
                if(lastOne)
                    enemyRoute.SetTile(target, enemyRouteHighlightTile[1]); // 下
                else 
                    enemyRoute.SetTile(target, enemyRouteHighlightTile[4]); // 竖着
            }
        }
        
    }

    #endregion
    public void ClearTip() => forTip.ClearAllTiles();
    public void ClearHatingRoute() => enemyRoute.ClearAllTiles();
    
    private string s1 =
        "人的梦境常与现实世界的种种经历及深藏的潜意识紧密相连，" +
        "它们绝非毫无意义的虚幻想象，" +
        "而是由个体的思想波澜、丰富情感、深刻记忆以及那未被完全探索的潜意识力量共同交织而成的一个多维而复杂的空间。" +
        "这个空间既蕴藏着无限的潜能与机遇，也暗含着未知的危险与挑战。";

    private string s2 = "睡梦是窥探内心的锁孔，是心灵深处未被完全解读的密码，映射着个体生命的丰富层次与无限可能。";
    private string s3 = "这个梦境世界迎来了它的终结——————一切都要结束了吗————————————————————————————恭喜你睡醒回到现实世界，该上班了";
}