
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
//地图编辑器
public class MapMaker : MonoBehaviour {

#if Tool

    //编辑器模式下才划线
    public bool drawLine;
    public GameObject gridGO;

    private static MapMaker _instance;

    public static MapMaker Instance
    {
        get
        {
            return _instance;
        }
        
    }
#endif
    //地图有关属性
    private float mapWidth;
    private float mapHeight;
    [HideInInspector]
    public float gridWidth;
    [HideInInspector]
    public float gridHeight;
    [HideInInspector]
    public const int yRow = 8;
    [HideInInspector]
    public const int xColumn = 12;
    [HideInInspector]
    public int bigLevelID;
    [HideInInspector]
    public int levelID;
    //全部的格子对象
    [HideInInspector]
    public GridPoint[,] gridPoints;
    //怪物路径点的索引集合存储——这个list尚未实例化_再Init里面实例化了
    [HideInInspector]
    public List<GridPoint.GridIndex> monsterPath;
    //怪物路径点的坐标集合
    [HideInInspector]
    public List<Vector3> monsterPathPos;

    //两个渲染器
    private SpriteRenderer BGSR;
    private SpriteRenderer roadSR;
    //每一波产生的怪物的ID
    public List<Round.RoundInfo> roundInfoList;
    [HideInInspector]
    public Carrot mCarrot;

    private void Awake()
    {
#if Tool
        _instance = this;
        InitMapMaker();
#endif

    }
    public void InitMapMaker()
    {
        CalculateSize();
        gridPoints = new GridPoint[xColumn, yRow];
        monsterPath = new List<GridPoint.GridIndex>();
        for (int x = 0; x < xColumn; x++)
        {
            for (int y = 0; y < yRow; y++)
            {
#if Tool

                GameObject itemGO = Instantiate(gridGO,transform.position,transform.rotation);
#endif
                //Debug.Log(gridWidth);
#if Game
                GameObject itemGO = GameController.Instance.GetGameObjectResource("Grid");
#endif
                itemGO.transform.position = CorrectPosition(gridWidth*x,gridHeight*y);
                itemGO.transform.SetParent(transform);
                //取到格子上面的脚本组件
                gridPoints[x, y] = itemGO.GetComponent<GridPoint>();
                //把值赋给格子里面的坐标值
                gridPoints[x, y].gridIndex.xIndex = x;
                gridPoints[x, y].gridIndex.yIndex = y;
            }
        }
        //路径和背景图的渲染
        BGSR = transform.Find("BG").GetComponent<SpriteRenderer>();
        roadSR = transform.Find("Road").GetComponent<SpriteRenderer>();
    }
    //加载地图
#if Game
    public void LoadMap(int bigLevel,int level)
    {
        bigLevelID = bigLevel;
        levelID = level;
        LoadLevelFile(LoadLevelInfoFile("Level"+bigLevel.ToString()+"_"+levelID.ToString()+".json"));
        //Debug.Log("重新加载地图"+ "Level" + bigLevel.ToString() + "_" + levelID.ToString() + ".json");
        monsterPathPos = new List<Vector3>();
        for (int i = 0; i < monsterPath.Count; i++)
        {
            monsterPathPos.Add(gridPoints[monsterPath[i].xIndex,monsterPath[i].yIndex].transform.position);
        }
        //起始点和终止点
        //monsterPath里面存储的只是一个位置，这个要先实例化出来
        GameObject startPointGo = GameController.Instance.GetGameObjectResource("startPoint");
        startPointGo.transform.position = monsterPathPos[0];
        startPointGo.transform.SetParent(this.transform);

        GameObject endPointGo = GameController.Instance.GetGameObjectResource("Carrot");
        //萝卜的位置，这个减去的v3是因为。。。也许有什么挡住了萝卜导致无法点击
        endPointGo.transform.position = monsterPathPos[monsterPathPos.Count-1]-new Vector3(0,0,1);
        endPointGo.transform.SetParent(this.transform);
        mCarrot = endPointGo.GetComponent<Carrot>();
    }
#endif
    public Vector3 CorrectPosition(float x,float y)
    {
        return new Vector3(x-mapWidth/2+gridWidth/2,y-mapHeight/2+gridHeight/2);
    }
    //计算地图和格子的宽高
    private void CalculateSize()
    {
        Vector3 leftDown = new Vector3(0,0);
        Vector3 rightUp = new Vector3(1,1);
        //视口坐标转世界坐标
        Vector3 posOne = Camera.main.ViewportToWorldPoint(leftDown);
        Vector3 posTwo = Camera.main.ViewportToWorldPoint(rightUp);

        mapWidth = posTwo.x - posOne.x;
        mapHeight = posTwo.y - posOne.y;
        gridWidth = mapWidth / xColumn;
        gridHeight = mapHeight / yRow;
        //Debug.Log(gridWidth);
    }
#if Tool
    private void OnDrawGizmos()
    {
        if (drawLine)
        {
            CalculateSize();
            Gizmos.color = Color.green;

            //划行
            for (int i = 0; i <= yRow; i++)
            {
                Vector3 startPos = new Vector3(-mapWidth/2,-mapHeight/2+i*gridHeight);
                Vector3 endPos = new Vector3(mapWidth / 2, -mapHeight / 2 + i * gridHeight);
                Gizmos.DrawLine(startPos,endPos);
            }
            //划列
            for (int i = 0; i <= xColumn; i++)
            {
                Vector3 startPos = new Vector3(-mapWidth / 2+gridWidth*i, mapHeight / 2 );
                Vector3 endPos = new Vector3(-mapWidth / 2 + gridWidth * i, -mapHeight / 2);
                Gizmos.DrawLine(startPos,endPos);
            }
        }
    }
#endif
    /// <summary>
    /// 有关地图编辑的方法
    /// </summary>
    
    //清除怪物路点
    public void ClearMonsterPath()
    {
        //Debug.Log("2");
        foreach (GridPoint.GridIndex item in monsterPath)
        {
            gridPoints[item.xIndex, item.yIndex].InitGrid();
            //Debug.Log("清除");
        }
        monsterPath.Clear();
        
    }
    //恢复地图编辑默认状态
    public void RecoverTowerPoint()
    {
        ClearMonsterPath();
        for (int x = 0; x < xColumn; x++)
        {
            for (int y = 0; y < yRow; y++)
            {
                gridPoints[x, y].InitGrid();//所有格子全部初始化
            }
        }
    }
    //初始化地图
    public void InitMap()
    {
        bigLevelID = 0;
        levelID = 0;
        RecoverTowerPoint();
        roundInfoList.Clear();
        BGSR.sprite = null;
        roadSR.sprite = null;
    }
#if Tool
    //生成一个levelInfo的对象来保存文件
    public LevelInfo CreateLevelInfoGo()
    {
        LevelInfo levelInfo = new LevelInfo
        {
            bigLevelID = this.bigLevelID,
            levelID = this.levelID
        };
        //Debug.Log(levelID);
        levelInfo.gridPoints =new List<GridPoint.GridState>();
        for (int x = 0; x < xColumn; x++)
        {
            for (int y = 0; y < yRow; y++)
            {
                levelInfo.gridPoints.Add(gridPoints[x, y].gridState);
            }
        }
        levelInfo.monsterPath = new List<GridPoint.GridIndex>();
        for (int i = 0; i < monsterPath.Count; i++)
        {
            levelInfo.monsterPath.Add(monsterPath[i]);
        }
        levelInfo.roundInfo = new List<Round.RoundInfo>();
        for (int i = 0; i < roundInfoList.Count; i++)
        {
            levelInfo.roundInfo.Add(roundInfoList[i]);
        }
        Debug.Log("保存成功");
        return levelInfo;
    }
    //保存当前关卡的数据文件
    public void SaveLevelFileByJson()
    {
        LevelInfo levelInfo = CreateLevelInfoGo();
        //这里存了很多关卡，所以要加上大关卡小关卡的ID
        //Debug.Log(levelID);
        string filePath = Application.dataPath + "/Resources/Json/Level/"+"Level"+bigLevelID.ToString()+"_"+levelID.ToString()+".json";
        string levelFile = JsonMapper.ToJson(levelInfo);
        StreamWriter sw = new StreamWriter(filePath);
        sw.Write(levelFile);
        sw.Close();
    }
#endif
    //读取关卡文件解析json转化为LevelInfo对象
    public LevelInfo LoadLevelInfoFile(string fileName)
    {
        LevelInfo levelInfo = new LevelInfo();
        string filePath= Application.streamingAssetsPath + "/Json/Level/" + fileName;
        if (File.Exists(filePath))
        {
            string readFile;
            StreamReader sr = new StreamReader(filePath);
            readFile= sr.ReadToEnd();
            sr.Close();
            levelInfo = JsonMapper.ToObject<LevelInfo>(readFile);
            //Debug.Log("文件路径: "+filePath);
            //Debug.Log("levelInfo:"+ levelInfo.roundInfo.Count+ "levelInfoID:BigLevelID"+levelInfo.bigLevelID+"LevelID"+levelInfo.levelID);
            return levelInfo;
        }
        Debug.Log(bigLevelID.ToString()+"文件名："+fileName);
        Debug.Log("目标路径找不到指定json文件:"+filePath);
        return null;
    }
    //把读取出来的信息存储文件，用MapMaker实现
    public void LoadLevelFile(LevelInfo levelInfo)
    {
        bigLevelID = levelInfo.bigLevelID;
        levelID = levelInfo.levelID;
        //Debug.Log(levelID);
        for (int x = 0; x < xColumn; x++)
        {
            for (int y = 0; y < yRow; y++)
            {
                //Debug.Log(levelInfo.gridPoints.Count);
                gridPoints[x, y].gridState = levelInfo.gridPoints[y+x*yRow];
                //更新格子状态

                gridPoints[x, y].UpdateGrid();

            }
        }
        monsterPath.Clear();
        for (int i = 0; i < levelInfo.monsterPath.Count; i++)
        {
            //monsterPath[i] = levelInfo.monsterIndex[i];
            //直接赋值进去的话，因为list是引用类型，可能会出现奇怪的问题，所以这里老师选择的是添加
            monsterPath.Add(levelInfo.monsterPath[i]);
        }
        //这个列表是只有json文件里面才有的，mapMaker里面没有，所以要new出来
        roundInfoList = new List<Round.RoundInfo>();
        //所有的level.json都设置的是10波怪物，但是关卡总数存储在playerManager.Json里面，这里如果关卡总数小于level.json，就取关卡总数
        int tempRoundCount;
        if (GameController.Instance.normalModelPanel.totalRound<levelInfo.roundInfo.Count)
        {
            tempRoundCount = GameController.Instance.normalModelPanel.totalRound;
        }
        else
        {
            tempRoundCount = levelInfo.roundInfo.Count;
        }
        for (int i = 0; i < tempRoundCount; i++)
        {
            roundInfoList.Add(levelInfo.roundInfo[i]);
        }
        //Debug.Log(levelInfo.roundInfo.Count+"--"+roundInfoList.Count);
        //Debug.Log("Pictures/NormalMordel/Game/" + bigLevelID.ToString() + "/Road" + levelID.ToString());
        //Debug.Log(roadSR.sprite);
        //Debug.Log(levelID);
        //Debug.Log(Resources.Load<Sprite>("Pictures/NormalMordel/Game/" + bigLevelID.ToString() + "/BG" + (levelID / 3).ToString()));
        BGSR.sprite = Resources.Load<Sprite>("Pictures/NormalMordel/Game/" + bigLevelID.ToString() + "/BG" + (levelID / 3).ToString());
        roadSR.sprite = Resources.Load<Sprite>("Pictures/NormalMordel/Game/" + bigLevelID.ToString() + "/Road" + levelID.ToString());
    }
}
