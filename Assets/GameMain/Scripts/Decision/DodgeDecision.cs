using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class DodgeDecision : DecisionBase
{
    public DodgeDecision(Player player) : base(player) { }

    public override bool Evaluate()
    {
        //后面从敌人那里获取是否需要闪避
        return CheckCanDodge() && Input.GetKeyDown(KeyCode.LeftShift);
    }

    public override void Execute()
    {
        player.isMoving = true;
        //玩家与怪物位置调换
        Vector2 playerToPos = player.toDodge.currentGrid.position;
        //更新位置信息
        player.toDodge.transform.DOMove(player.transform.position, .5f).OnComplete(()=>
        {
            player.toDodge.UpdateGridInfo();
            player.UpdateGridInfo();
        });
        player.transform.DOMove(playerToPos, .49f).OnComplete(() =>
        {
            player.isMoving = false;
            player.hasInput = true;
            IgnoreDodge();
        });
    }

    private bool CheckCanDodge()
    {
        Debug.Log(player.canDodge+" "+player.isDodgeIgnore);
        //先判断是否冷却
        if (player.isDodgeIgnore)
            return false;
        //判断是否需要闪避（有怪物将要攻击）
        if (player.canDodge)
            return true;
        return false;
    }
    
    public override void ClearStat()
    {
        player.UpdateGridInfo();
        //回合结束，使得不能闪避
        player.canDodge = false;
    }

    private async void IgnoreDodge()
    {
        //0.1秒后使得不能动弹
        await Task.Delay(TimeSpan.FromSeconds(.1f));
        //一个回合不能动弹
        player.isDodgeIgnore = true;
    }
}

