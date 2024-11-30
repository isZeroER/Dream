using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    public static SaveManager instance;

    [SerializeField] private bool encryptData;
    private string fileDirPath;
    private string fileName = "Data.txt";
    
    private GameData _gameData;
    private List<ISaveManager> _saveManagers;
    private FileDataHandler _dataHandler;

    protected override void Awake()
    {
        base.Awake();
        fileDirPath = Application.persistentDataPath;
        Debug.Log(fileDirPath);
        _dataHandler = new FileDataHandler(fileDirPath, fileName, encryptData);
    }
    

    [ContextMenu("Delete Save Data")]
    public void DeleteSaveData()
    {
        _dataHandler = new FileDataHandler(fileDirPath, fileName, encryptData);
        _dataHandler.Delete();
    }
    
    private void Start()
    {
        //用于对每个模块进行存储管理
        _saveManagers = FindAllSaveManagers();
        //游戏开始时读取存档
        LoadGame();
    }

    public bool HasSaveData()
    {
        try
        {
            if (_dataHandler.Load() != null)
                return true;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }

        return false;
    }

    public void NewGame() => _gameData = new GameData();
    public void LoadGame()
    {
        _gameData = _dataHandler.Load();

        //如果没有，就新建游戏
        if (_gameData == null)
            NewGame();

        foreach (var saveManager in _saveManagers)
            saveManager.LoadData(_gameData);
    }

    public void SaveGame()
    {
        foreach (var saveManager in _saveManagers)
            saveManager.SaveData(ref _gameData);
        
        _dataHandler.Save(_gameData);
    }

    private void OnApplicationQuit() => SaveGame();
    
    private List<ISaveManager> FindAllSaveManagers()
    {
        //使用接口的方法，可以使用所有的接口
        IEnumerable<ISaveManager> saveMgr = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveManager>();
        return new List<ISaveManager>(saveMgr);
    }
}
