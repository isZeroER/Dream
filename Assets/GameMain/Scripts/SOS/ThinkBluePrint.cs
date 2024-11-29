using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Think BluePrint", menuName = "Think BluePrint/Player BluePrint")]
public class ThinkBluePrint:ScriptableObject
{
    public List<Vector2> points = new List<Vector2>();
    public virtual void Skill(){}
}
