using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class Memento  {
    //保存
    public void SaveJsonFile()
    {
        PlayerManager playerManager = GameManager.Instance.playerManager;
        string filePath = Application.streamingAssetsPath + "/Json/playerManager.json";
        string saveJsonStr = JsonMapper.ToJson(playerManager);
        StreamWriter sw = new StreamWriter(filePath);
        sw.Write(saveJsonStr);
        sw.Close();
    }
    //读取
    public PlayerManager LoadByJson()
    {
        PlayerManager playerManager = new PlayerManager();
        string filePath = "";
        if (GameManager.Instance.isInitPlayerManager)//如果重置了游戏
        {
            filePath= Application.streamingAssetsPath + "/Json/playerManagerInitData.json";

        }
        else
        {
            filePath= Application.streamingAssetsPath + "/Json/playerManager.json";

        }
        if (File.Exists(filePath))//如果文件存在
        {
            StreamReader sr = new StreamReader(filePath);
            string jsonStr= sr.ReadToEnd();
            sr.Close();
            playerManager = JsonMapper.ToObject<PlayerManager>(jsonStr);
            return playerManager;
        }
        else
        {
            Debug.Log("没有读取到指定文件信息，路径不存在："+filePath);
        }
        return null;
    }
    
}
