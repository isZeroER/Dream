using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//存储和加载接口
public interface ISaveManager
{
    void LoadData(GameData data);
    //ref为指针
    void SaveData(ref GameData data);
}
