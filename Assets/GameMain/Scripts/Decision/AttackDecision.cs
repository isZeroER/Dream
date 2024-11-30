using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackDecision : DecisionBase
{
    public AttackDecision(Player player) : base(player) { }
    private Character toAttack;
    private bool waitingForChoose;
    private GridInfo[] bluePrintGrids;
    public override bool Evaluate()
    {
        if (waitingForChoose)
            return true;
        if(Input.GetKeyDown(KeyCode.Space) || CardManager.Instance.isToAttack)
            waitingForChoose = true;
        return CheckTarget(player.transform.position) && (CardManager.Instance.isToAttack || Input.GetKeyDown(KeyCode.Space));
    }

    public override void Execute()
    {
        //正在等待获取...
        if (waitingForChoose)
        {
            ChooseTarget();
        }
    }

    /// <summary>
    /// 点击，则获取到所有敌人，并且判断是否可攻击
    /// </summary>
    private void ChooseTarget()
    {
        RaycastHit2D[] hits = CastRayToSelectTarget();
        if (hits == null)
        {
            Debug.Log("没有选择目标，继续等待...");
            return;
        }
        foreach (var hit in hits)
        {
            
            //判断是否为蓝图攻击范围内敌人
            if (hit.collider.CompareTag("Enemy") && bluePrintGrids.Contains(hit.collider.GetComponent<Character>().currentGrid))
            {
                
                toAttack = hit.collider.GetComponent<Character>();
                // Debug.Log("获取到了目标" + toAttack.name);
                waitingForChoose = false;
                player.DoDamage(player.strength, toAttack);
                toAttack = null;
                player.hasInput = true;
                return;
            }
        }
    }
    
    // 发射射线来选择目标
    private RaycastHit2D[] CastRayToSelectTarget()
    {
        if (!Input.GetMouseButtonDown(0)) 
            return null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // 发射一条射线，检测鼠标点击位置是否有敌人
        return Physics2D.RaycastAll(ray.origin, ray.direction);
    }

    /// <summary>
    /// 判断是否存在要攻击的enemy
    /// </summary>
    /// <param name="pos"></param>
    private bool CheckTarget(Vector2 pos)
    {
        //获取蓝图相应的格子
        bluePrintGrids = BluePrintTargets(pos);
        int enemyNum = 0;
        foreach (GridInfo grid in bluePrintGrids)
        {
            if(grid == null) 
                continue;
            if (grid.characterType == Character.CharacterType.Enemy)
            {
                enemyNum++;
            }
        }

        if (enemyNum == 0)
            return false;
        else
            return true;
    }

    /// <summary>
    /// 获取当前位置根据蓝图的周围格子,如果有敌人就显示
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private GridInfo[] BluePrintTargets(Vector2 pos)
    {
        if(player.thinkBluePrint==null)
            return GridManager.Instance.GetAdjacentGrids(pos);
        //通过蓝图获取攻击范围
        List<GridInfo> grids = new List<GridInfo>();
        foreach (var point in player.thinkBluePrint.points)
        {
            Vector2 gridPos = (Vector2)player.transform.position + point;
            grids.Add(GridManager.Instance.GetGridByPos(gridPos));
            GridManager.Instance.CheckisEnemy(gridPos);
        }
        return grids.ToArray();
    }
    
    public override void ClearStat()
    {
        toAttack = null;
        waitingForChoose = false;
        CardManager.Instance.isToAttack = false;
        
        //一个回合的闪避解除
        player.isDodgeIgnore = false;
    }
}

