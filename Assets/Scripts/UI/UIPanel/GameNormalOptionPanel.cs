using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameNormalOptionPanel : BasePanel {

    [HideInInspector]
    public bool isInBigLevel = true;

    public void ReturnLastPanel()
    {
        if (isInBigLevel)
        {//返回主界面
            mUIFacade.ChangeSceneState(new MainSceneState(mUIFacade));
        }
        else
        {//退出小关卡选择，进入大关卡选择
            mUIFacade.currentScenePanelDict[StringManager.GameNormalLevelPanel].ExitPanel();
            mUIFacade.currentScenePanelDict[StringManager.GameNormalBigLevelPanel].EnterPanel();
        }
        mUIFacade.PlayButtonAduioClip();
        //不管怎么返回，在进入的时候都会是大关卡
        isInBigLevel = true;
    }
    public void ToHelpPanel()
    {
        mUIFacade.PlayButtonAduioClip();
        mUIFacade.currentScenePanelDict[StringManager.HelpPanel].EnterPanel();
    }
}
