using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class HelpPanel : BasePanel {

    private GameObject helpPageGo;
    private GameObject monsterPageGo;
    private GameObject towerPageGo;

    private SliderCanCoverScrollView sliderCanCoverScrollView;
    private SliderScrowView sliderScrowView;
    private Tween helpTween;

    protected override void Awake()
    {
        base.Awake();
        helpPageGo = transform.Find("HelpPage").gameObject;
        monsterPageGo = transform.Find("MonsterPage").gameObject;
        towerPageGo = transform.Find("TowerPage").gameObject;
        //这里可能存在的问题，就是上面还没找到，这里就取组件，最后是空，如果是这样，把这里不要使用变量，直接再找一次
        //Debug.Log(helpPageGo);
        sliderScrowView = towerPageGo.transform.Find("Scroll View").GetComponent<SliderScrowView>();
        sliderCanCoverScrollView = helpPageGo.transform.Find("Scroll View").GetComponent<SliderCanCoverScrollView>();
        helpTween = transform.DOLocalMoveX(0, 0.5f);
        helpTween.SetAutoKill(false);
        helpTween.Pause();
    }
    public override void InitPanel()
    {
        base.InitPanel();
        transform.SetSiblingIndex(5);
        sliderScrowView.Init();
        sliderCanCoverScrollView.Init();
        ShowHelpPage();

        //其他处理，如果在主面板点击了help面板，然后在大关卡点击了help面板，然后在Help面板点击了返回主面板
        //此时help面板的位置是在正中心的，要挪到外面去
        if (transform.localPosition==Vector3.zero)
        {
            gameObject.SetActive(false);
            helpTween.PlayBackwards();
        }
        //transform.localPosition = new Vector3(1024,0,0);
        transform.localPosition = new Vector3(1024, 0, 0);
    }
    public override void EnterPanel()
    {
        base.EnterPanel();
        gameObject.SetActive(true);
        MoveToCenter();
        sliderScrowView.Init();
        sliderCanCoverScrollView.Init();
    }
    public void MoveToCenter()
    {
        helpTween.PlayForward();
    }
    public override void ExitPanel()
    {
        base.ExitPanel();
        mUIFacade.PlayButtonAduioClip();
        //在冒险模式选择场景
        if (mUIFacade.currentSceneState.GetType()==typeof(NormalGameOptionSceneState))
        {//返回主场景，这里要切换场景
            mUIFacade.ChangeSceneState(new MainSceneState(mUIFacade));
            SceneManager.LoadScene(1);
        }
        else//如果是在主场景,切换面板
        {

            helpTween.PlayBackwards();
            mUIFacade.currentScenePanelDict[StringManager.MainPanel].EnterPanel();
        }
    }
    //显示页面的方法
    public void ShowHelpPage()
    {
        if (!helpPageGo.activeSelf)//原理同setPanel，进来的时候是默认显示的，只有选中按钮才播放音效
        {
            mUIFacade.PlayButtonAduioClip();
            helpPageGo.SetActive(true);
        }
        monsterPageGo.SetActive(false);
        towerPageGo.SetActive(false);
    }
    public void ShowMonsterPage()
    {
        mUIFacade.PlayButtonAduioClip();
        helpPageGo.SetActive(false);
        monsterPageGo.SetActive(true);
        towerPageGo.SetActive(false);
    }
    public void ShowTowerPage()
    {
        mUIFacade.PlayButtonAduioClip();
        helpPageGo.SetActive(false);
        monsterPageGo.SetActive(false);
        towerPageGo.SetActive(true);
    }

}
