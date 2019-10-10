using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalModelSceneState : BaseSceneState
{
    public NormalModelSceneState(UIFacade uiFacade) : base(uiFacade)
    {
    }
    public override void EnterScene()
    {
        GameManager.Instance.audioSourceManager.CloseBGMusic();
        mUIFacade.AddPanelToDict(StringManager.GameLoadPanel);
        mUIFacade.AddPanelToDict(StringManager.NormalModelPanel);
        base.EnterScene();
    }
    public override void ExitScene()
    {
        GameManager.Instance.audioSourceManager.OpenBGMusic();
        base.ExitScene();
    }
}
