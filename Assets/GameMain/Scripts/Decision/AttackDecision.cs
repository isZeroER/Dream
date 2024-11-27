using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackDecision : DecisionBase
{
    public AttackDecision(Player player) : base(player) { }
    private Character toAttack;
    private bool waitingForChoose;
    public override bool Evaluate()
    {
        if (waitingForChoose)
            return true;
        if(Input.GetKeyDown(KeyCode.J) || CardManager.Instance.isToAttack)
            waitingForChoose = true;
        return CheckTarget(player.transform.position) && (CardManager.Instance.isToAttack || Input.GetKeyDown(KeyCode.J));
    }

    public override void Execute()
    {
        //正在等待获取...
        if (waitingForChoose)
        {
            ChooseTarget();
        }
    }

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
            if (hit.collider.CompareTag("Enemy"))
            {
                toAttack = hit.collider.GetComponent<Character>();
                waitingForChoose = false;
                Debug.Log("获取到了目标" + toAttack.name);
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
    /// 获取到要攻击的enemy
    /// </summary>
    /// <param name="pos"></param>
    private bool CheckTarget(Vector2 pos)
    {
        GridInfo[] grids = GridManager.Instance.GetAdjacentGrids(pos);
        int enemyNum = 0;
        foreach (GridInfo grid in grids)
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
    
    public override void ClearStat()
    {
        toAttack = null;
        waitingForChoose = false;
        CardManager.Instance.isToAttack = false;
    }
}

