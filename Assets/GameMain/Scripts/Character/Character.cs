using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //角色决策行为
    protected enum CharacterAction
    {
        Move,
        Attack,
        Dodge,
        Idle
    }

    //角色种类
    public enum CharacterType
    {
        None,
        Player,
        Enemy
    }
    
    protected CharacterAction action = CharacterAction.Idle;
    protected CharacterType characterType;
    
    //当前位置
    public GridInfo currentGrid;
    public int health;
    public int strength;

    public virtual void HandleMethod()
    {
        
    }
    #region Damage
    /// <summary>
    /// 对目标角色造成伤害
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="victim， 收到伤害的对象"></param>
    public void DoDamage(int damage, Character victim)
    {
        victim.TakeDamage(damage);
    }
    /// <summary>
    /// 受到的伤害
    /// </summary>
    /// <param name="damage, 受到的伤害值"></param>
    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
            Die();
    }
    #endregion

    protected virtual void Die()
    {
        Debug.Log("死了");
        currentGrid.characterType = CharacterType.None;
        Destroy(gameObject);
    }
}
