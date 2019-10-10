using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 这个类，就是面板进来需要做的事
/// </summary>
public class MainPanel : BasePanel {
    private Animator carrotAnimator;
    private Transform monsterTransform;
    private Transform cloudTransform;

    private Tween[] mainPanelTween;//0右移，1左移动
    private Tween exitTween;//离开页面运行的动画

    private GameObject dontClickMe;
    protected override void Awake()
    {
        base.Awake();
        //获取成员变量，设置渲染顺序
        transform.SetSiblingIndex(8);
        carrotAnimator = GameObject.Find("Emp_Carrot").GetComponent<Animator>();

        monsterTransform = GameObject.Find("Img_MonsterFly").transform;
        cloudTransform = GameObject.Find("Img_Cloud").transform;

        mainPanelTween = new Tween[2];
        mainPanelTween[0] = transform.DOLocalMoveX(1024,0.5f);
        mainPanelTween[0].SetAutoKill(false);
        mainPanelTween[0].Pause();
        mainPanelTween[1] = transform.DOLocalMoveX(-1024, 0.5f);
        mainPanelTween[1].SetAutoKill(false);
        mainPanelTween[1].Pause();

        dontClickMe = transform.Find("DontClickMe").gameObject;
        PlayUITween();
    }
    //UI播放动画的方法
    private void PlayUITween()
    {
        monsterTransform.DOLocalMoveY(290,2f).SetLoops(-1,LoopType.Yoyo);
        cloudTransform.DOLocalMoveX(750,6f).SetLoops(-1,LoopType.Restart);
    }
    //无论跳转哪个场景，先进入loadingPanel
    public void LoadingImage()
    {
        mUIFacade.currentScenePanelDict[StringManager.GameLoadPanel].EnterPanel();//loading页面的enterPanel还没写
    }
    //跳转对应的场景方法
    public void ToNormalModelScene()
    {
        mUIFacade.PlayButtonAduioClip();
        //先跳转到loading页面
        LoadingImage();
        //转变当前场景
        mUIFacade.ChangeSceneState(new NormalGameOptionSceneState(mUIFacade));
    }
    public void ToBossModelScene()
    {
        mUIFacade.PlayButtonAduioClip();
        dontClickMe.SetActive(true);
        //LoadingImage();
        //mUIFacade.ChangeSceneState(new BossModelSceneState(mUIFacade));
    }
    public void HideDontClickMePage()
    {
        dontClickMe.SetActive(false);
    }
    public void ToMonsterNestScene()
    {
        mUIFacade.PlayButtonAduioClip();
        LoadingImage();
        mUIFacade.ChangeSceneState(new MonsterNestSceneState(mUIFacade));
    }
    //设置面板移动开的动画，并跳转到对应的Panel,SetPanel
    public void MoveToRight()
    {
        mUIFacade.PlayButtonAduioClip();
        exitTween = mainPanelTween[0];
        mUIFacade.currentScenePanelDict[StringManager.SetPanel].EnterPanel();
    }
    public void MoveToLeft()
    {
        mUIFacade.PlayButtonAduioClip();
        exitTween = mainPanelTween[1];
        mUIFacade.currentScenePanelDict[StringManager.HelpPanel].EnterPanel();
    }
    //重写进入面板方法
    public override void EnterPanel()
    {
        transform.SetSiblingIndex(8);
        carrotAnimator.Play("CarrotGrow");
        //如果退出动画不为空，说明之前已经运行过退出动画了，那就意味着主面板在外面，要调回来
        if (exitTween!=null)
        {
            exitTween.PlayBackwards();
        }
        cloudTransform.gameObject.SetActive(true);//出去的时候云朵隐藏了，这里显示回来
    }
    //离开面板方法
    public override void ExitPanel()
    {
        if (exitTween!=null)
        {
            exitTween.PlayForward();
            cloudTransform.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError(exitTween+"动画为空");
        }
    }
    public void ExitGame()
    {
        mUIFacade.PlayButtonAduioClip();
        Application.Quit();
    }
}

