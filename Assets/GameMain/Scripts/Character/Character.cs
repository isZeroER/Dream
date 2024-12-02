using System;
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

    //TODO:后续改为动画
    private SpriteRenderer sr;

    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    public virtual void HandleMethod()
    {
        
    }
    #region Damage
    /// <summary>
    /// 对目标角色造成伤害
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="victim， 收到伤害的对象"></param>
    public virtual void DoDamage(int damage, Character victim)
    {
        victim.TakeDamage(damage);
    }
    /// <summary>
    /// 受到的伤害
    /// </summary>
    /// <param name="damage, 受到的伤害值"></param>
    protected virtual void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
            Die();
        StartCoroutine(CoTakeDamage(damage));
    }

    IEnumerator CoTakeDamage(int damage)
    {
        float blinkFor = 1f;
        float timer = 0;
        while (timer<blinkFor)
        {
            timer += .4f;
            sr.color = damage > 0 ? Color.red : Color.green;
            yield return new WaitForSeconds(.2f);
            sr.color = Color.white;
            yield return new WaitForSeconds(.2f);
        }
    }
    
    #endregion

    public virtual void UpdateGridInfo()
    {
        GridManager.Instance.ChangeGridInfo(currentGrid, CharacterType.None);
        //这里
        currentGrid = GridManager.Instance.GetGridByPos(transform.position);
        GridManager.Instance.ChangeGridInfo(currentGrid, characterType);
    }

    public virtual void UpdateGridInfoNow(Vector2 targetPos)
    {
        GridManager.Instance.ChangeGridInfo(currentGrid, CharacterType.None);
        currentGrid = GridManager.Instance.GetGridByPos(targetPos);
        GridManager.Instance.ChangeGridInfo(targetPos, characterType);
    }
    protected virtual void Die()
    {
        Debug.Log(characterType + "死了");
        currentGrid.characterType = CharacterType.None;
        Invoke(nameof(DestroyCharacter), 1f);
    }

    private void DestroyCharacter() => Destroy(gameObject);
}
