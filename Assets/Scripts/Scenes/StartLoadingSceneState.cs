using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLoadingSceneState : BaseSceneState
{
    public StartLoadingSceneState(UIFacade uiFacade) : base(uiFacade)
    {
    }
    public override void EnterScene()
    {
        mUIFacade.AddPanelToDict(StringManager.StartLoadPanel);
        base.EnterScene();

    }
    public override void ExitScene()
    {
        base.ExitScene();
        SceneManager.LoadScene(1);
    }
}
