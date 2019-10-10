using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
#if Tool
[CustomEditor(typeof(MapMaker))]
public class MapTool : Editor {

    private MapMaker mapMaker;
    //关卡文件列表
    private List<FileInfo> fileList = new List<FileInfo>();
    private string[] filaNameList;//关卡名字的下拉列表

    //当前编辑的关卡索引
    private int selectIndex = -1;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (Application.isPlaying)
        {
            mapMaker = MapMaker.Instance;
            EditorGUILayout.BeginHorizontal();
            //获取操作的文件名
            filaNameList = GetNames(fileList);
            //使用的是老师推荐的一种，弹出框
            int currentIndex = EditorGUILayout.Popup(selectIndex, filaNameList);
            if (currentIndex != selectIndex)//当前选择对象是否改变
            {
                selectIndex = currentIndex;

                //实例化地图的方法
                mapMaker.InitMap();
                //加载当前选择的level文件
                //根据当前选择的关卡index得到文件名，得到存储信息文件，得到信息文件数据
                mapMaker.LoadLevelFile(mapMaker.LoadLevelInfoFile(filaNameList[selectIndex]));
            }
            if (GUILayout.Button("读取关卡列表"))
            {
                LoadLevelFiles();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("恢复地图编辑默认状态"))
            {
                mapMaker.RecoverTowerPoint();
            }
            if (GUILayout.Button("清除怪物路点"))
            {
                mapMaker.ClearMonsterPath();
                Debug.Log("1");
            }
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("保存当前关卡数据文件"))
            {
                mapMaker.SaveLevelFileByJson();
            }
        }
    }
    //加载关卡数据
    private void LoadLevelFiles()
    {
        ClearList();
        fileList = GetLevelFiles();
    }
    //清除列表文件的方法
    private void ClearList()
    {
        fileList.Clear();
        selectIndex = -1;
    }
    //具体读取关卡列表
    private List<FileInfo> GetLevelFiles()//上面的关卡列表文件是通过外部读取的，无法直接获得
    {
        string[] files = Directory.GetFiles(Application.dataPath+"/Resources/Json/Level","*.json");//*.json是读取所有.json结尾的文件

        List<FileInfo> list = new List<FileInfo>();
        for (int i = 0; i < files.Length; i++)
        {
            FileInfo fileInfo =new FileInfo(files[i]);//按文件内容赋值进变量
            list.Add(fileInfo);

        }
        return list;
    }
    //取得文件名字列表
    private string[] GetNames(List<FileInfo> files)
    {
        List<string> names = new List<string>();
        foreach (FileInfo item in files)
        {
            names.Add(item.Name);
        }
        return names.ToArray();
    }
}
#endif