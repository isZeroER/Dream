using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EM_InactiveEndorphin : EnemyBase
{
    protected override void Start()
    {
        base.Start();
        enemyType = EnemyType.InactiveEndorphin;
        SetHealth_Strenth(2, 2);
    }

    protected override bool CanAttack()
    {
        GridInfo[] aroundGrids = GridManager.Instance.GetAdjacentGrids(transform.position);
        
        foreach (var aroundGrid in aroundGrids)
        {
            if (aroundGrid.characterType == CharacterType.Player)
            {
                DoDamage(strength, player);
                return true;
            }
        }
        return false;
    }
}
