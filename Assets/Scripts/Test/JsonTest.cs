using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class JsonTest : MonoBehaviour {

    AppOfTrigger appOfTrigger=new AppOfTrigger();
	// Use this for initialization
	void Start () {
        appOfTrigger.appNum = 3;
        appOfTrigger.phoneState = true;
        //appOfTrigger.appList = new List<string>() {"痘印","bilibili","吃鸡" };
        //SaveByJson();
        //appOfTrigger= LoadJson();
        //Debug.Log(appOfTrigger.appNum);
        //Debug.Log(appOfTrigger.phoneState);
        //foreach (var item in appOfTrigger.appList)
        //{
        //    Debug.Log(item);
        //}
        appOfTrigger.appList = new List<AppProperty>();
        AppProperty appProperty = new AppProperty
        {
            appName = "douyin",
            triggerID = "trigger",
            triggerFavour = true,
            useTimeList=new List<int> { 6,7,8}
        };
        appOfTrigger.appList.Add(appProperty);
        //SaveByJson();  
       // appOfTrigger = LoadJson();
    }

    public void SaveByJson()
    {
        //取得具体路径及文件名
        string filePath = Application.dataPath + "/Resources/Json/AppOfTrigger.json";
        //转化成json格式字符串
        string saveJsonStr = JsonMapper.ToJson(appOfTrigger);
        //写入对应文件
        StreamWriter sw = new StreamWriter(filePath);
        sw.Write(saveJsonStr);
        sw.Close();
    }
    public AppOfTrigger LoadJson()
    {
        AppOfTrigger appItem = new AppOfTrigger();
        string filaPath=Application.dataPath+ "/Resources/Json/AppOfTrigger.json";

        if (File.Exists(filaPath))
        {
            StreamReader sr = new StreamReader(filaPath);
            string jsonStr=sr.ReadToEnd();
            sr.Close();
            appItem = JsonMapper.ToObject<AppOfTrigger>(jsonStr);
        }
        if (appItem==null)
        {
            Debug.Log("读取Json文件失败");
        }
        return appItem;
    }
}
