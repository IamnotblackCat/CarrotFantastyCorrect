using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NormalGameOptionSceneState : BaseSceneState
{
    public NormalGameOptionSceneState(UIFacade uiFacade) : base(uiFacade)
    {
    }
    public override void EnterScene()
    {
        mUIFacade.AddPanelToDict(StringManager.GameNormalOptionPanel);
        mUIFacade.AddPanelToDict(StringManager.GameNormalLevelPanel);
        mUIFacade.AddPanelToDict(StringManager.GameNormalBigLevelPanel);
        mUIFacade.AddPanelToDict(StringManager.HelpPanel);
        mUIFacade.AddPanelToDict(StringManager.GameLoadPanel);
        base.EnterScene();
    }
    public override void ExitScene()
    {
        GameNormalOptionPanel gameNormalOptionPanel = mUIFacade.currentScenePanelDict[StringManager.GameNormalOptionPanel] as GameNormalOptionPanel;
        if (gameNormalOptionPanel.isInBigLevel)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            SceneManager.LoadScene(3);
        }
        gameNormalOptionPanel.isInBigLevel = true;//离开以后无论是大小关卡，回来的时候都是大关卡了
        base.ExitScene();
    }
}
