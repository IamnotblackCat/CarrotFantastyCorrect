using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NormalModelPanel : BasePanel {

    public int cardNum=1;
    //持有面板
    private GameObject topPageGo;
    private GameObject menuPageGo;
    private GameObject gameOverGo;
    private GameObject gameWinGo;
    private GameObject img_FinalWave;
    private GameObject img_StartGame;
    private GameObject prizePageGo;
    //引用
    public TopPage topPage;
    //private GameController gameController;
    private PlayerManager playerManager;

    public int totalRound;
    protected override void Awake()
    {
        base.Awake();
        //gameController = GameController.Instance;
        transform.SetSiblingIndex(1);//设置渲染层级在底层，其他UI都是在上面的
        topPageGo = transform.Find("Img_TopPage").gameObject;
        menuPageGo = transform.Find("MenuPage").gameObject;
        gameOverGo = transform.Find("GameOverPage").gameObject;
        gameWinGo= transform.Find("GameWinPage").gameObject;
        img_FinalWave= transform.Find("Img_FinalWave").gameObject;
        img_StartGame= transform.Find("StartUI").gameObject;
        prizePageGo= transform.Find("PrizePage").gameObject;

        topPage = topPageGo.GetComponent<TopPage>();

        playerManager = GameManager.Instance.playerManager;
        //totalRound = gameController.currentStage.mTotalRound;
    }
    private void OnEnable()
    {
        InvokeRepeating("PlayAudio",0.5f,1);
        Invoke("StartGame",3.5f);
        cardNum = 1;
    }
    private void PlayAudio()
    {//这个面板和uifacade都是不销毁的，所以会先于GameController实例化，
     //这样如果在这里打开startGame，GameController单例还没有，就会报错,所以放在这里打开，并且整体推迟0.5秒
        img_StartGame.SetActive(true);
        GameController.Instance.PlayEffectClip("NormalMordel/CountDown");
    }
    private void StartGame()
    {
        GameController.Instance.PlayEffectClip("NormalMordel/GO");
        GameController.Instance.StartGame();
        img_StartGame.SetActive(false);
        CancelInvoke();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public override void EnterPanel()
    {
        base.EnterPanel();
        //Debug.Log(gameController);
        totalRound = GameController.Instance.currentStage.mTotalRound;
        //Debug.Log(totalRound);
        topPageGo.SetActive(true);
    }
    public override void UpdatePanel()
    {
        base.UpdatePanel();
        topPage.UpdateCoinText();
        topPage.UpdateRoundCount();
    }
    /// <summary>
    /// 页面显示隐藏的方法
    /// </summary>

    //奖励页面
    public void ShowPrizePage()
    {
        GameController.Instance.isPause = true;
        prizePageGo.SetActive(true);
    }
    public void HidePrizePage()
    {
        mUIFacade.PlayButtonAduioClip();
        GameController.Instance.isPause = false;
        prizePageGo.SetActive(false);
    }

    //菜单页面
    public void ShowMenuPage()
    {
        mUIFacade.PlayButtonAduioClip();
        GameController.Instance.isPause = true;
        menuPageGo.SetActive(true);
    }
    public void HideMenuPage()
    {
        mUIFacade.PlayButtonAduioClip();
        GameController.Instance.isPause = false;
        menuPageGo.SetActive(false);
    }
    //胜利页面
    public void ShowGameWinPage()
    {
        if (!GameController.Instance.isPause)
        {
            GameController.Instance.isPause=true;
        }
        Stage stage = GameManager.Instance.playerManager.unlockedNormalModelLevelList
            [GameController.Instance.currentStage.mLevelID + (GameController.Instance.currentStage.mBigLevelID - 1) * 5];
        GameController.Instance.PlayEffectClip("NormalMordel/Perfect");
        //是否全清
        if (GameController.Instance.IsAllClear())
        {
            stage.mAllClear = true;
        }
        //萝卜徽章更新
        int carrotState = GameController.Instance.GetCarrotState();
        if (carrotState!=0&&stage.mCarrotState!=0)//安全校验
        {
            if (stage.mCarrotState > carrotState)
            {
                stage.mCarrotState = carrotState;//图片顺序是金的最小，越好越小，所以这里是取最高分存储
            }

        }
        else if (stage.mCarrotState==0)
        {
            stage.mCarrotState = carrotState;
        }
        //开启下一个关卡，注意不能是隐藏关卡;
        if (GameController.Instance.currentStage.mLevelID%5!=0&&//这里有一个问题，隐藏关卡不解锁下一个关卡，但是隐藏前一个能解锁隐藏
            (GameController.Instance.currentStage.mLevelID-1+(GameController.Instance.currentStage.mBigLevelID-1)*5)
            <GameManager.Instance.playerManager.unlockedNormalModelLevelList.Count)
        {
            GameManager.Instance.playerManager.unlockedNormalModelLevelList
                [GameController.Instance.currentStage.mLevelID + (GameController.Instance.currentStage.mBigLevelID-1) * 5].unLocked = true;
        }
        UpdatePlayerData();
        //ShowRoundText();
        gameWinGo.SetActive(true);
        GameController.Instance.gameOver = false;
        GameManager.Instance.playerManager.adventrueModelNum++;//解锁地图数量增加
        
        if (cardNum <= 0)
        {
            cardNum = 1;
        }
    }
    public void HideGameWinPage()
    {
        gameWinGo.SetActive(false);
    }
    //失败页面
    public void ShowGameOverPage()
    {
        //Debug.Log("播放失败音效");
        if (cardNum<=0)
        {
            cardNum = 1;
        }
        GameController.Instance.PlayEffectClip("NormalMordel/Lose");
        UpdatePlayerData();
        //GameController.Instance.gameOver = false;
        //GameController.Instance.creatingMonster = false;
        gameOverGo.SetActive(true);
    }
    public void HideGameOverPage()
    {
        gameOverGo.SetActive(false);
    }
    /// <summary>
    /// 与UI相关的方法
    /// </summary>
    //最后一波
    public void ShowFinalWaveUI()
    {
        GameController.Instance.PlayEffectClip("NormalMordel/FinalWave");
        img_FinalWave.SetActive(true);
        Invoke("HideFinalWaveUI",1);
    }
    public void HideFinalWaveUI()
    {
        img_FinalWave.SetActive(false);
        GameController.Instance.level.HandleLastRound();
    }
    private void ResetUI()
    {
        gameWinGo.SetActive(false);
        gameOverGo.SetActive(false);
        menuPageGo.SetActive(false);
        gameObject.SetActive(false);//隐藏是为了再次调用，然后运行enable方法
    }
    //更新回合显示的文本
    public void ShowRoundText(Text text)
    {
        string roundStr = "";
        
        int roundNum = GameController.Instance.level.currentRound+1;
        //Debug.Log(GameController.Instance.level.currentRound);
        if (roundNum<10)
        {
            roundStr = "0  " + roundNum;
        }
        else
        {
            roundStr = roundNum / 10 + "  " + roundNum % 10;
        }
        text.text = roundStr;

    }



    /// <summary>
    /// 
    /// </summary>

    //玩家基础数据
    public void UpdatePlayerData()
    {
        playerManager.coin += GameController.Instance.coin;
        playerManager.killMonsterNum += GameController.Instance.killMonsterNum;
        playerManager.clearItemNum += GameController.Instance.clearItemNum;
    }

    //重玩游戏
    public void Replay()
    {
        //mUIFacade.PlayButtonAduioClip();
        UpdatePlayerData();
        //切换场景状态原样进行一次，是为了重新触发enterScene和ExitScene方法
        mUIFacade.ChangeSceneState(new NormalModelSceneState(mUIFacade));
        GameController.Instance.gameOver = false;
        //这一行代码是目的，在Monster的Update里面，如果暂停和over都是false，会走下面的扣萝卜的血的逻辑，经过
        //一连串的反应，最终会在点击重玩的时候又播放一次失败音效
        GameController.Instance.isPause = true;
        //Debug.Log(GameController.Instance.gameOver);
        Invoke("ResetGame", 1f);
    }
    public void ResetGame()
    {
        SceneManager.LoadScene(3);
        ResetUI();
        gameObject.SetActive(true);
    }
    //选择其他关卡
    public void ChooseOtherLevel()
    {
        //mUIFacade.PlayButtonAduioClip();在菜单面板添加过了
        //gameObject.SetActive(false);
        UpdatePlayerData();
        GameController.Instance.gameOver = false;
        GameController.Instance.isPause = true;
        Invoke("ToOtherScene", 2f);
        mUIFacade.ChangeSceneState(new NormalGameOptionSceneState(mUIFacade));
    }
    //返回关卡选择页面
    public void ToOtherScene()
    {
        //GameController.Instance.gameOver = false;
        ResetUI();
        SceneManager.LoadScene(2);
    }
}
