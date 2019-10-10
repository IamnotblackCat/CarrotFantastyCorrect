using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneState : BaseSceneState
{
    public MainSceneState(UIFacade uiFacade) : base(uiFacade)
    {
    }
    public override void EnterScene()
    {
        mUIFacade.AddPanelToDict(StringManager.MainPanel);
        mUIFacade.AddPanelToDict(StringManager.GameLoadPanel);
        mUIFacade.AddPanelToDict(StringManager.SetPanel);
        mUIFacade.AddPanelToDict(StringManager.HelpPanel);
        
        base.EnterScene();
        if (mUIFacade.NeedPlayBGMusic())
        {
            GameManager.Instance.audioSourceManager.PlayBGMusic(GameManager.Instance.factoryManager.audioClipFactory.GetSingleResources("Main/BGMusic"));
        }
        
    }
    public override void ExitScene()
    {
        base.ExitScene();
        //这里 当前场景是因为在MainPanel脚本的EnterPanel方法里面已经把下一个场景替换为当前场景了，一路追踪发现是UIFacade的ShowMask（）
        if (mUIFacade.currentSceneState.GetType()==typeof(NormalGameOptionSceneState))
        {
            SceneManager.LoadScene(2);//冒险模式
        }
        else if (mUIFacade.currentSceneState.GetType() == typeof(BossGameOptionSceneState))
        {
            SceneManager.LoadScene(3);//boss模式
        }
        else
        {
            SceneManager.LoadScene(6);//怪物窝
        }
    }
}
