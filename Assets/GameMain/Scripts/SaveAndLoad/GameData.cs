using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class GameData
{
    public int volume;
    public SerializableDictionary<string, bool> sectionPass;
    public GameData()
    {
        volume = 0;
        sectionPass = new SerializableDictionary<string, bool>();
    }
}
