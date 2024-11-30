using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;


public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    private bool canEncryot = false;
    private string codeWord = "ILOVELXY";
    public FileDataHandler(string dataDirPath, string dataFileName, bool encryData)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.canEncryot = encryData;
    }

    public void Save(GameData gameData)
    {
        //文件的绝对位置
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            //创建Directory路径
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            //将gameData对象转化为json字符串
            string dataToStore = JsonUtility.ToJson(gameData, true);
            //加密
            if(canEncryot)
                dataToStore = EncryptDecrypt(dataToStore);
            
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    //写入文件当中
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        //读取完整文件字符串
                        dataToLoad = reader.ReadToEnd();
                    }

                    //解密
                    if(canEncryot)
                        dataToLoad = EncryptDecrypt(dataToLoad);
                    //将文件内字符串转化为GameData对象
                    loadData = JsonUtility.FromJson<GameData>(dataToLoad);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        return loadData;
    }

    public void Delete()
    {
        string path = Path.Combine(dataDirPath, dataFileName);
        if(File.Exists(path))
            File.Delete(path);
    }

    private string EncryptDecrypt(string data)
    {
        string modifierData = "";
        for (int i = 0; i < data.Length; i++)
        {
            //每个字符都和codeWord里面的字相与，得到的就是编码的
            modifierData += (char)(data[i] ^ codeWord[i % codeWord.Length]);
        }

        return modifierData;
    }
}
