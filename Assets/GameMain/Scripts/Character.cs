using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected enum CharacterAction
    {
        Move,
        Attack,
        Dodge,
        Idle
    }
    
    protected CharacterAction action = CharacterAction.Idle;
    
    //当前位置
    public GridInfo currentGrid;
    public int health;
    public int strength;

    public virtual void HandleMethod()
    {
        
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
    }
}
