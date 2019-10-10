using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameNormalLevelPanel : BasePanel {

    
    private string filePath;//图片资源加载的根路径
    private string theSpritePath;//小关卡详细关卡图片路径
    public int currentBigLevelID;
    public int currentLevelID;

    private Transform levelContentTrans;
    private GameObject img_LockBtnGo;//未解锁关卡遮挡板
    private Transform emp_TowerTrans;//下面显示的能建造的塔的集合的UI
    private Image img_BGLeft;
    private Image img_BGRight;
    private Image img_Carrot;
    private Image img_AllClear;
    private Text text_TotalWaves;

    private PlayerManager playerManager;
    private SliderScrowView sliderScrowView;

    private List<GameObject> levelContentImageGO;
    private List<GameObject> towerContentImageGO;
   
    protected override void Awake()
    {
        base.Awake();
        filePath = "GameOption/Normal/Level/";
        playerManager = mUIFacade.mPlayerManager;
        levelContentImageGO = new List<GameObject>();
        towerContentImageGO = new List<GameObject>();
        levelContentTrans = transform.Find("Scroll View").Find("Viewport").Find("Content").transform;
        img_LockBtnGo = transform.Find("Img_LockBtn").gameObject;
        emp_TowerTrans = transform.Find("Emp_Tower");
        img_BGLeft = transform.Find("Img_BGLeft").GetComponent<Image>();
        img_BGRight = transform.Find("Img_BGRight").GetComponent<Image>();
        text_TotalWaves = transform.Find("Image_TotalWaves").Find("Text").GetComponent<Text>();
        sliderScrowView = transform.Find("Scroll View").GetComponent<SliderScrowView>();
        currentBigLevelID = 1;//初始为1
        currentLevelID = 1;
    }
    //更新地图UI的方法，可以移动的UI，动态UI
    public void UpdateMapUI(string spritePath)
    {
        img_BGLeft.sprite = mUIFacade.GetSprite(spritePath + "BG_Left");
        img_BGRight.sprite = mUIFacade.GetSprite(spritePath + "BG_Right");
        for (int i = 0; i < 5; i++)//共五个小关，不建议用死数
        {//把这个创建的物体丢进列表，这样之后方便从列表推入对象池，以及取出
            levelContentImageGO.Add(CreatUIAndSetUIPosition("Img_Level", levelContentTrans));
            //更换图片，列表是按顺序添加的，用索引取出
            levelContentImageGO[i].GetComponent<Image>().sprite = mUIFacade.GetSprite(spritePath+"Level_"+(i+1).ToString());
            Stage stage = playerManager.unlockedNormalModelLevelList[(currentBigLevelID - 1) * 5 + i];//取到对应的小关卡ID对应的stage
            //Debug.Log(stage.mIsRewardLevel);
            //一次性查找取得两个徽章物体，免得后面一直找浪费资源
            GameObject allClearGO = levelContentImageGO[i].transform.Find("Img_AllClear").gameObject;
            GameObject carrotGO = levelContentImageGO[i].transform.Find("Img_Carrot").gameObject;
            allClearGO.SetActive(false);
            carrotGO.SetActive(false);
            if (stage.unLocked)//已解锁
            {
                if (stage.mAllClear)
                {
                    allClearGO.SetActive(true);
                }
                if (stage.mCarrotState!=0)//胡萝卜徽章是用int值存的
                {
                    carrotGO.SetActive(true);
                    carrotGO.transform.GetComponent<Image>().sprite = mUIFacade.GetSprite(filePath+"Carrot"+stage.mCarrotState);
                }
                levelContentImageGO[i].transform.Find("Img_Lock").gameObject.SetActive(false);
                levelContentImageGO[i].transform.Find("Img_BG").gameObject.SetActive(false);
                //Debug.Log(levelContentImageGO[i].transform.Find("Img_BG").gameObject.activeSelf);
            }
            else//未解锁
            {
                //Debug.Log(stage.mLevelID);
                if (stage.mIsRewardLevel)//是隐藏关卡，用隐藏面板遮挡
                {
                    
                    levelContentImageGO[i].transform.Find("Img_Lock").gameObject.SetActive(true);//锁现形
                    levelContentImageGO[i].transform.Find("Img_BG").gameObject.SetActive(true);//背景板
                    Image monsterImage = levelContentImageGO[i].transform.Find("Img_BG").Find("Img_Monster").GetComponent<Image>();
                    monsterImage.sprite = mUIFacade.GetSprite("MonsterNest/Monster/Baby/"+currentBigLevelID);
                    monsterImage.SetNativeSize();
                    monsterImage.transform.localScale = new Vector3(2,2,1);
                }
                else//正常关卡
                {
                    levelContentImageGO[i].transform.Find("Img_Lock").gameObject.SetActive(true);//锁现形
                    levelContentImageGO[i].transform.Find("Img_BG").gameObject.SetActive(false);//背景板
                }
            }
        }
        sliderScrowView.SetContentLength(5);
    }
    //更新静态UI，不可移动的UI
    public void UpdateLevelUI(string spritePath)//暂时没发现这个参数在哪里用到的
    {
        //回收到对象池
        //Debug.Log(towerContentImageGO.Count);
        if (towerContentImageGO.Count>0)
        {
            for (int i = 0; i < towerContentImageGO.Count; i++)
            {
                towerContentImageGO[i].GetComponent<Image>().sprite = null;
                mUIFacade.PushGameObjectResource(FactoryType.UIFactory,"Img_Tower",towerContentImageGO[i]);
            }
            towerContentImageGO.Clear();
            //Debug.Log("清空");
        }
        
        //这个计算公式是存进去的时候用的，可以带入数字算一下就知道了。
        Stage stage = playerManager.unlockedNormalModelLevelList[(currentBigLevelID-1)*5+currentLevelID-1];
        //拿到stage对象，数据几乎都存在stage里面
        if (stage.unLocked)//解锁了
        {
            img_LockBtnGo.SetActive(false);
        }
        else
        {
            img_LockBtnGo.SetActive(true);
        }
        //怪物波次数
        text_TotalWaves.text = stage.mTotalRound.ToString();
        //塔图标列表
        for (int i = 0; i < stage.mTowerIDList.Length; i++)
        {
            GameObject towerIconGO= CreatUIAndSetUIPosition("Img_Tower",emp_TowerTrans);
            //感觉最后一步可以使用i+1，实际上这个list也是int集合
            towerIconGO.GetComponent<Image>().sprite = mUIFacade.GetSprite(filePath+"Tower/Tower_"+stage.mTowerIDList[i].ToString());
            towerContentImageGO.Add(towerIconGO);
        }
    }
    //这个方法是为了进入当前面板并且取得从属的大关卡编号
    public void ToThisPanel(int currentBigLevel)
    {
        currentBigLevelID = currentBigLevel;
        //进入当前面板的时候小关卡肯定是第一个
        currentLevelID = 1;//实际上应该用json存储然后读取
        EnterPanel();
    }
    public override void InitPanel()
    {
        base.InitPanel();
        gameObject.SetActive(false);
    }
    public override void EnterPanel()
    {
        base.EnterPanel();
        gameObject.SetActive(true);
        theSpritePath = filePath + currentBigLevelID.ToString() + "/";
        DestroyMapUI();
        UpdateMapUI(theSpritePath);
        UpdateLevelUI(theSpritePath);
        sliderScrowView.Init();
    }
    public override void UpdatePanel()
    {
        base.UpdatePanel();
        UpdateLevelUI(theSpritePath);
        //Debug.Log("接到了消息");
    }
    public override void ExitPanel()
    {
        base.ExitPanel();
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 跳转到游戏场景，不是面板    
    /// </summary>
    public void ToGamePanel()
    {
        mUIFacade.PlayButtonAduioClip();
        GameManager.Instance.currentStage = playerManager.unlockedNormalModelLevelList[(currentBigLevelID - 1) * 5 + currentLevelID-1];//索引从0开始
        mUIFacade.currentScenePanelDict[StringManager.GameLoadPanel].EnterPanel();
        mUIFacade.ChangeSceneState(new NormalModelSceneState(mUIFacade));//跳转到游戏场景，不是面板了
    }
    //资源加载
    private void LoadResource()
    {
        mUIFacade.GetSprite(filePath + "AllClear");
        mUIFacade.GetSprite(filePath + "Carrot_1");
        mUIFacade.GetSprite(filePath + "Carrot_2");
        mUIFacade.GetSprite(filePath + "Carrot_3");

        for (int i = 1; i < 4; i++)//这里是不建议用死数的，实际上它应该是大关卡的ID，一共三关，所以1-4。建议使用json存储数据，然后读取
        {
            string spritePath = filePath + i.ToString() + "/";
            mUIFacade.GetSprite(spritePath+"BG_Left");
            mUIFacade.GetSprite(spritePath + "BG_Right");
            for (int j = 1; j < 6; j++)//五个小关
            {
                mUIFacade.GetSprite(spritePath+"Level_"+j.ToString());
            }
            for (int j = 1; j < 13; j++)//13个炮塔
            {
                mUIFacade.GetSprite(filePath+"Tower/Tower_"+j.ToString());
            }
        }
    }
    //创建UI的方法
    public GameObject CreatUIAndSetUIPosition(string uiName,Transform parentTrans)
    {
        GameObject itemGO= mUIFacade.GetGameObjectResource(FactoryType.UIFactory, uiName);
        itemGO.transform.SetParent(parentTrans);
        itemGO.transform.localPosition = Vector3.zero;
        itemGO.transform.localScale = Vector3.one;
        return itemGO;
    }
    //前往上一页or下一页的时候要修改index值
    public void ToNextLevel()
    {
        currentLevelID++;
        UpdatePanel();
        //Debug.Log("接消息");
    }
    public void ToLastLevel()
    {
        currentLevelID--;
        UpdatePanel();
    }
    //销毁UI的方法，销毁地图卡
    public void DestroyMapUI()
    {
        if (levelContentImageGO.Count>0)
        {
            for (int i = 0; i < levelContentImageGO.Count; i++)//这里老师用的是死值5，如果有问题记得修改
            {
                //推入对象池
                mUIFacade.PushGameObjectResource(FactoryType.UIFactory,"Img_Level",levelContentImageGO[i]);
            }
            //把content的值设回初始值
            sliderScrowView.InitContentLength();
            //字典记得清空
            levelContentImageGO.Clear();
        }
    }
}
