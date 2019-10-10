using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SetPanel : BasePanel {

    private GameObject optionPageGo;
    private GameObject statisticPageGo;
    private GameObject producerPageGo;
    private GameObject panel_ResetGo;
    //动画
    private Tween setPanelTween;
    private bool playBGMusic = true;
    private bool playEffectMusic = true;
    //public为了直接在面板拖，不然找起来麻烦。
    public Sprite[] btnSprites;//0.音效开，1.音效关，2音乐开，3.音乐关
    //持有两个按钮的image组件，这样才能替换
    private Image image_Btn_EffectAudio;
    private Image image_Btn_BGAudio;

    public Text[] statisticesText;

    protected override void Awake()
    {
        base.Awake();
        //动画设置出来并且暂停
        setPanelTween = transform.DOLocalMoveX(0,0.5f);
        setPanelTween.SetAutoKill(false);
        setPanelTween.Pause();
        //持有成员变量
        optionPageGo = transform.Find("OptionImage").gameObject;
        statisticPageGo = transform.Find("StatisticImage").gameObject;
        producerPageGo = transform.Find("ProducerImage").gameObject;
        image_Btn_EffectAudio = optionPageGo.transform.Find("Btn_Audio").GetComponent<Image>();
        image_Btn_BGAudio = optionPageGo.transform.Find("Btn_BGM").GetComponent<Image>();
        panel_ResetGo = transform.Find("Panel_Reset").gameObject;
        //InitPanel();
    }
    //private void OnEnable()
    //{
    //    Debug.Log(playBGMusic);
    //    if (!playBGMusic)
    //    {
    //        Debug.Log(playBGMusic);
    //        mUIFacade.CloseBGMusi();
    //    }
    //}
    public override void InitPanel()
    {
        transform.localPosition = new Vector3(-1024,0,0);
        transform.SetSiblingIndex(2);
    }

    //三个页面的显示
    public void ShowOptionPage()
    {
        //第一次进来的时候是默认开启的，不播放声音，后续切换的时候播放
        if (!optionPageGo.activeSelf)
        {
            mUIFacade.PlayButtonAduioClip();
            optionPageGo.SetActive(true);
        }
        
        statisticPageGo.SetActive(false);
        producerPageGo.SetActive(false);
    }
    public void ShowStatisticPage()
    {
        mUIFacade.PlayButtonAduioClip();
        optionPageGo.SetActive(false);
        statisticPageGo.SetActive(true);
        producerPageGo.SetActive(false);
        ShowStatistics();
    }
    public void ShowProducerPage()
    {
        mUIFacade.PlayButtonAduioClip();
        optionPageGo.SetActive(false);
        statisticPageGo.SetActive(false);
        producerPageGo.SetActive(true);
    }
    //重置游戏
    public void ResetGame()
    {
        mUIFacade.PlayButtonAduioClip();
        GameManager.Instance.isInitPlayerManager = true;
        GameManager.Instance.playerManager.ReadData();//重新读取
        ShowStatistics();//重新计算统计数据
        HideResetPage();
    }
    //确认重置面板
    public void ShowResetPage()
    {
        mUIFacade.PlayButtonAduioClip();
        panel_ResetGo.SetActive(true);
    }
    //关闭重置面板
    public void HideResetPage()
    {
        mUIFacade.PlayButtonAduioClip();
        panel_ResetGo.SetActive(false);
    }
    //进入退出面板
    public override void EnterPanel()
    {
        ShowOptionPage();
        MoveToCenter();
    }
    public override void ExitPanel()
    {
        mUIFacade.PlayButtonAduioClip();
        setPanelTween.PlayBackwards();
        //退出面板的时候要调用主面板,中介者模式or状态模式
        mUIFacade.currentScenePanelDict[StringManager.MainPanel].EnterPanel();
        InitPanel();
    }
    public void MoveToCenter()
    {
        setPanelTween.PlayForward();
    }
    //音乐开关
    public void CloseOrOpenBGMusic()
    {
        //Debug.Log(playBGMusic);
        mUIFacade.PlayButtonAduioClip();
        playBGMusic = !playBGMusic;
        mUIFacade.CloseOrOpenBGMusic();
        if (playBGMusic)
        {
            image_Btn_BGAudio.sprite = btnSprites[2];
        }
        else
        {
            image_Btn_BGAudio.sprite = btnSprites[3];
        }
    }
    //音效开关
    public void CloseOrOpenEffectMusic()
    {
        mUIFacade.PlayButtonAduioClip();
        playEffectMusic = !playEffectMusic;
        mUIFacade.CloseOrOpenEffectMusic();
        if (playEffectMusic)
        {
            image_Btn_EffectAudio.sprite = btnSprites[0];
        }
        else
        {
            image_Btn_EffectAudio.sprite = btnSprites[1];
        }
    }
    //显示数据的方法
    public void ShowStatistics()
    {
        PlayerManager playerManager = mUIFacade.mPlayerManager;
        statisticesText[0].text = playerManager.adventrueModelNum.ToString();
        statisticesText[1].text = playerManager.burriedLevelNum.ToString();
        statisticesText[2].text = playerManager.bossModelNum.ToString();
        statisticesText[3].text = playerManager.coin.ToString();
        statisticesText[4].text = playerManager.killMonsterNum.ToString();
        statisticesText[5].text = playerManager.killBossNum.ToString();
        statisticesText[6].text = playerManager.clearItemNum.ToString();
    }
}
