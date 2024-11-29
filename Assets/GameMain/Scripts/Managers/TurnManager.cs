using System;
using System.Collections;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    private bool isPlayerTurn = true;
    //是否在回合中
    private bool turning;
    private Player player;

    private void Start()
    {
        player = PlayerManager.Instance.player;
    }

    private void Update()
    {
        //判断是哪一方回合
        if (!turning)
        {
            if (isPlayerTurn)
            {
                turning = true;
                StartCoroutine(PlayerInput());
            }
            else
            {
                turning = true;
                EnemyTurn();
            }
        }
    }

    private IEnumerator PlayerInput()
    {
        while (!player.hasInput)
        {
            Debug.Log("玩家回合，等待玩家输入...");
            player.HandleMethod();
            yield return null;
        }
        //刷新玩家状态
        player.ClearStat();

        //提示map里面都清空然后更新状态
        GridManager.Instance.ClearTip();
        GridManager.Instance.ChangeGridInfo(player.transform.position, Character.CharacterType.Player);

        EndTurn();
    }

    private void EnemyTurn()
    {
        // 敌人回合逻辑，假设敌人回合是自动进行的
        Debug.Log("敌人回合，自动执行...");

        // 模拟敌人行动
        StartCoroutine(EnemyAction());
    }

    private IEnumerator EnemyAction()
    {
        yield return new WaitForSeconds(.51f);
        EnemyManager.Instance.HandleTurn();
        yield return new WaitForSeconds(.51f);
        player.hasInput = false;
        EndTurn();
    }

    public void EndTurn()
    {
        // 切换回合
        isPlayerTurn = !isPlayerTurn;
        turning = false;
    }
}