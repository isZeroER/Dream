using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStar
{
    private GridManager gridManager;

    public AStar(GridManager gridManager)
    {
        this.gridManager = gridManager;
    }

    public List<GridInfo> FindPath(Vector2 startPos, Vector2 endPos)
    {
        // 获取起点和终点的格子
        GridInfo startGrid = gridManager.GetGridByPos(startPos);
        GridInfo endGrid = gridManager.GetGridByPos(endPos);

        if (startGrid == null || endGrid == null) return null;

        // 开放列表和封闭列表
        List<AStarNode> openList = new List<AStarNode>();
        HashSet<AStarNode> closedList = new HashSet<AStarNode>();

        // 创建起点的节点
        AStarNode startNode = new AStarNode(startGrid, null, 0, GetHeuristic(startPos, endPos));
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            // 选择具有最小 F 值的节点
            AStarNode currentNode = openList.OrderBy(node => node.FCost).First();
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // 如果到达终点，返回路径
            if (currentNode.GridInfo == endGrid)
            {
                return RetracePath(startNode, currentNode);
            }

            // 遍历当前节点的邻居
            foreach (var neighbor in gridManager.GetAdjacentGrids(currentNode.GridInfo.position))
            {
                // 如果邻居在封闭列表中，跳过;或者neighbor为空，也跳过
                if (closedList.Any(node => node.GridInfo == neighbor) || neighbor == null) continue;

                // 计算 G 和 H 值
                
                float newGCost = currentNode.GCost + GetDistance(currentNode.GridInfo.position, neighbor.position);
                float newHCost = GetHeuristic(neighbor.position, endPos);

                AStarNode neighborNode = new AStarNode(neighbor, currentNode, newGCost, newHCost);

                // 如果邻居不在开放列表中，或者找到更短路径，加入开放列表
                if (openList.All(node => node.GridInfo != neighbor) || newGCost < neighborNode.GCost)
                {
                    openList.Add(neighborNode);
                }
            }
        }

        return null;  // 如果没有路径
    }

    // 计算曼哈顿距离作为启发式函数
    private float GetHeuristic(Vector2 start, Vector2 end)
    {
        return Mathf.Abs(start.x - end.x) + Mathf.Abs(start.y - end.y);
    }

    // 计算两个点之间的距离
    private float GetDistance(Vector2 a, Vector2 b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    // 追溯路径
    private List<GridInfo> RetracePath(AStarNode startNode, AStarNode endNode)
    {
        List<GridInfo> path = new List<GridInfo>();
        AStarNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode.GridInfo);
            currentNode = currentNode.Parent;
        }

        path.Reverse();
        return path;
    }
}
public class AStarNode
{
    public GridInfo GridInfo;  // 当前格子的信息
    public AStarNode Parent;    // 上一个节点
    public float GCost;        // 起点到当前节点的实际成本
    public float HCost;        // 当前节点到终点的启发式成本
    public float FCost => GCost + HCost;  // 总成本 (f = g + h)

    public AStarNode(GridInfo gridInfo, AStarNode parent, float gCost, float hCost)
    {
        GridInfo = gridInfo;
        Parent = parent;
        GCost = gCost;
        HCost = hCost;
    }
}

