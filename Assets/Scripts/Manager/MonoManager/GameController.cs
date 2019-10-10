using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour {

    private static GameController _instance;
   
    public static GameController Instance
    {
        get
        {
            return _instance;
        }
    }

    private GameManager mGameManager;

    //游戏UI的面板
    public NormalModelPanel normalModelPanel;
    public RuntimeAnimatorController[] controllers;
    //引用值
    public Level level;
    public int[] mMonsterIDList;//当前回合可生成的怪物列表
    public int mMonsterIDIndex;//当前生成的怪物ID索引，是第几个
    public Stage currentStage;
    public MapMaker mapMaker;
    public Transform targetTrans;//集火目标
    public GameObject targetSignal;//集火信号
    public GridPoint selectGrid;//上一个选择的格子
    //变动计算值
    public int killMonsterNum;
    public int killMonsterTotalNum;
    public int clearItemNum;
    //属性值
    public int carrotHP = 10;
    public int gameSpeed;
    public bool isPause;
    public int coin;
    
    public bool creatingMonster;//是否继续产怪
    public bool gameOver;//游戏是否结束
    //建造者
    public MonsterBuilder monsterBuilder;
    public TowerBuilder towerBuilder;
    //箭塔价格标
    public Dictionary<int, int> towerPriceDict ;
    //箭塔按钮列表的那个空物体
    public GameObject towerListGo;

    //处理塔的升级与买卖的画布
    public GameObject handleTowerCanvasGo;

    
    private void Awake()
    {
#if Game
        _instance = this;
        mGameManager = GameManager.Instance;
        //测试代码：currentStage = new Stage(1, 1,new int[]{ 1,2,3,4,5},5,0,10,true,false,false);
        currentStage = mGameManager.currentStage;
        normalModelPanel = mGameManager.uiManager.mUIFacade.currentScenePanelDict[StringManager.NormalModelPanel] as NormalModelPanel;
        normalModelPanel.EnterPanel();
        mapMaker = GetComponent<MapMaker>();
        mapMaker.InitMapMaker();
        mapMaker.LoadMap(currentStage.mBigLevelID,currentStage.mLevelID);
        //为什么要这样写呢？因为可以在进来的时候把资源都加载一次放进数组里面，以后直接拿就好了，不要每次都问工厂要，加载要时间
        controllers = new RuntimeAnimatorController[12];

        //mapMaker.LoadMap(2, 1);
        //成员变量赋值
        coin = 1000;
        gameSpeed = 1;
        monsterBuilder = new MonsterBuilder();
        towerBuilder = new TowerBuilder();
        //建塔列表的处理
        for (int i = 0; i < currentStage.mTowerIDList.Length; i++)
        {
            GameObject itemGo = GameManager.Instance.GetGameObjectResource(FactoryType.UIFactory, "Btn_TowerBuild");
            itemGo.GetComponent<ButtonTower>().towerID=currentStage.mTowerIDList[i];//把关卡可以建造的箭塔ID赋值过来
            itemGo.transform.localPosition = transform.position;
            itemGo.transform.SetParent(towerListGo.transform);
            itemGo.transform.localScale = Vector3.one;
        }
        //箭塔价格标
        towerPriceDict = new Dictionary<int, int>
        {
            { 1,100},
            { 2,120},
            { 3,140},
            { 4,160},
            { 5,160}
        };
        //monsterBuilder.m_monsterID = 1;
        for (int i = 0; i < controllers.Length; i++)
        {
            controllers[i] = GetRuntimeAnimatorController("Monster/"+mapMaker.bigLevelID.ToString()+"/"+(i+1).ToString());
        }

        level = new Level(mapMaker.roundInfoList.Count,mapMaker.roundInfoList);
        normalModelPanel.topPage.UpdateCoinText();
        normalModelPanel.topPage.UpdateRoundCount();
        isPause = true;
        //level.HandleRound();//执行责任链方法,注释掉是因为要在倒计时完毕以后调用这个方法
#endif
    }
    private void Update()
    {
#if Game
        if (!isPause)
        {
            //正常
            if (killMonsterNum >= mMonsterIDList.Length)
            {
                if (level.currentRound==level.totalRound)
                {
                    return;
                }
                //添加当前回合数的索引
                AddRoundNum();
            }
            else
            {
                if (!creatingMonster)
                {
                    CreateMonster();
                }
            }
        }
        else
        {
            //暂停
            StopCreateMonster();
            creatingMonster = false;
        }
        //if (isFireOn)
        //{
        //    OnFire();
        //}
#endif
    }
#if Game
    public void DecreaseHP()//萝卜被咬了
    {
        PlayEffectClip("NormalMordel/Carrot/Crash");
        carrotHP--;
        //更新萝卜UI显示
        mapMaker.mCarrot.UpdateCarrotUI();
        //Debug.Log("咬了两次?");
    }
#endif
    /// <summary>
    /// 产生怪物有关的方法
    /// </summary>
    //产怪方法——里面包含延时调用
    public void CreateMonster()
    {
        //if (gameOver)
        //{
        //    return;
        //}
        creatingMonster = true;
        InvokeRepeating("InstantiateMonster",(float)1/gameSpeed,(float)1/gameSpeed);//两个参数是起始时间和间隔时间
    }
    //具体的产怪方法
    public void InstantiateMonster()
    {
        if (mMonsterIDIndex >= mMonsterIDList.Length)
        {
            StopCreateMonster();
            return;
        }
        PlayEffectClip("NormalMordel/Monster/Create");
        GameObject effectGo = GetGameObjectResource("CreateEffect");
        effectGo.transform.position = mapMaker.monsterPathPos[0];
        effectGo.transform.SetParent(transform);
        //产生怪物
        if (mMonsterIDIndex<mMonsterIDList.Length)
        {
            //Debug.Log(monsterBuilder.m_monsterID);
            monsterBuilder.m_monsterID = level.roundList[level.currentRound].roundInfo.mMonsterIDList[mMonsterIDIndex];
        }
        GameObject monster = monsterBuilder.GetProduct();
        monster.transform.position = mapMaker.monsterPathPos[0];
        monster.transform.SetParent(transform);
        mMonsterIDIndex++;
        
    }
    /// <summary>
    /// 处理玩家有关的
    /// </summary>
    //增加或者减少玩家金币
    public void ChangeCoin(int prize)
    {
        coin += prize;
        normalModelPanel.UpdatePanel();
    }
    public void StopCreateMonster()
    {
        CancelInvoke();
    }
    public bool IsAllClear()
    {
        for (int x = 0; x < MapMaker.xColumn; x++)
        {
            for (int y = 0; y < MapMaker.yRow; y++)
            {
                if (mapMaker.gridPoints[x,y].gridState.hasItem)
                {
                    return false;
                }
            }
        }
        return true;
    }
    public int GetCarrotState()
    {
        int carrotState = 0;
        if (carrotHP>=4)
        {
            carrotState = 1;//金萝卜，后面的以此类推
        }
        else if (carrotHP>=2)
        {
            carrotState = 2;
        }
        else
        {
            carrotState = 3;
        }
        return carrotState;
    }
    //增加回合数，并且交给下一个回合来处理产怪
    public void AddRoundNum()
    {
        //更新有关回合显示的UI
        normalModelPanel.UpdatePanel();
        mMonsterIDIndex = 0;
        killMonsterNum = 0;
        level.AddRoundNum();
        level.HandleRound();
    }
    /// <summary>
    ///处理有关游戏逻辑的方法 
    /// </summary>
    //格子处理方法
#if Game
    //打开礼物页面的方法
    public void ShowPrizePage()
    {
        normalModelPanel.ShowPrizePage();
    }
    //开始游戏
    public void StartGame()
    {
        isPause = false;
        level.HandleRound();
    }
    public void HandleGrid(GridPoint grid)
    {
        if (grid.gridState.canBuild)
        {
            //三种判断
            if (selectGrid==null)//之前没选中
            {
                PlayEffectClip("NormalMordel/Grid/GridSelect");
                selectGrid = grid;
                selectGrid.ShowGrid();
            }
            else if (selectGrid==grid)//选中同一个
            {
                PlayEffectClip("NormalMordel/Grid/GridDeselect");
                selectGrid.HideGrid();
                selectGrid = null;
            }
            else if (selectGrid!=grid)//现在选的是另一个格子
            {
                PlayEffectClip("NormalMordel/Grid/GridSelect");
                selectGrid.HideGrid();
                selectGrid = grid;
                selectGrid.ShowGrid();
            }
        }
        else
        {
            grid.HideGrid();//不是很明白——明白了，这里是如果这个格子不能建造，就隐藏格子
            grid.ShowCantBuild();
            PlayEffectClip("NormalMordel/Grid/SelectFault");
            if (selectGrid!=null)
            {
                selectGrid.HideGrid();//隐藏上一个格子，如果上一个格子已经选中了其他的塔
            }
        }
    }
#endif
    //封装方法，方便其他类调用的时候不知道GameManager
    public Sprite GetSprite(string resourcePath)
    {
        return mGameManager.GetSprite(resourcePath);
    }
    public AudioClip GetAudioClip(string resourcePath)
    {
        return mGameManager.GetAudioClip(resourcePath);
        
    }
    public RuntimeAnimatorController GetRuntimeAnimatorController(string resourcePath)
    {
        return mGameManager.GetRuntimeAnimatorController(resourcePath);
    }
    public GameObject GetGameObjectResource(string resourcePath)
    {
        return  mGameManager.GetGameObjectResource(FactoryType.GameFactory,resourcePath);
    }
    public void PushGameObjectToFactory(string resourcePath,GameObject itemGO)
    {
        mGameManager.PushGameObjectToFactory(FactoryType.GameFactory,resourcePath,itemGO);
    }
    /// <summary>
    /// 与集火目标有关的方法
    /// </summary>
    public void ShowSignal()
    {
        PlayEffectClip("NormalMordel/Tower/ShootSelect");
        targetSignal.transform.position = targetTrans.position+new Vector3(0,mapMaker.gridHeight/2,0);
        targetSignal.transform.SetParent(targetTrans);
        targetSignal.SetActive(true);
    }
    public void HideSignal()
    {
        targetSignal.SetActive(false);
        targetTrans = null;
    }
    //封装音效
    public void PlayEffectClip(string resourcePath)
    {
        mGameManager.audioSourceManager.PlayEffect(GetAudioClip(resourcePath));
    }
    //天照
    //private bool isFireOn = false;
    public void ShowTheFire()
    {
        //isFireOn = !isFireOn;
        for (int x = 0; x < MapMaker.xColumn; x++)
        {
            for (int y = 0; y < MapMaker.yRow; y++)
            {
                if (mapMaker.gridPoints[x, y].gridState.canBuild == false && mapMaker.gridPoints[x, y].gridState.hasItem == false)
                {
                    GameObject fireGo = GetGameObjectResource("Fire");
                    fireGo.transform.SetParent(gameObject.transform);
                    fireGo.transform.localPosition = mapMaker.gridPoints[x, y].transform.localPosition;
                    fireGo.transform.localScale = new Vector3(0.4f,0.35f,1);
                    //for (float timer = 0.2f; timer >= 0; timer -= Time.deltaTime)
                    //{
                    //    Debug.Log("There is nothing");
                    //}
                }
            }
        }
    }
    //public void OnFire()
    //{
    //    for (int x = 0; x < MapMaker.xColumn; x++)
    //    {
    //        for (int y = 0; y < MapMaker.yRow; y++)
    //        {
    //            if (mapMaker.gridPoints[x, y].gridState.canBuild == false && mapMaker.gridPoints[x, y].gridState.hasItem == false)
    //            {
    //                GameObject fireGo = GetGameObjectResource("Fire");
    //                fireGo.transform.SetParent(gameObject.transform);
    //                fireGo.transform.localPosition = mapMaker.gridPoints[x, y].transform.localPosition;
    //                for (float timer = 0.2f; timer >= 0; timer -= Time.deltaTime)
    //                {
    //                    Debug.Log("There is nothing");
    //                }
    //            }
    //        }
    //    }

    //}
}
