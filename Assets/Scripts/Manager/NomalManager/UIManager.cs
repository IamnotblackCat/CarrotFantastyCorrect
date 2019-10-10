using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 负责管理UI的管理者,存储当前面板的字典，清空当前字典，将当前面板放回工厂
/// </summary>
public class UIManager {
    public UIFacade mUIFacade;
    private GameManager mGameManager;
    public Dictionary<string, GameObject> currentScenePanelDict;

    public UIManager()
    {
        mGameManager = GameManager.Instance;
        currentScenePanelDict = new Dictionary<string, GameObject>();
        mUIFacade = new UIFacade(this);
        //初始场景
        mUIFacade.currentSceneState = new StartLoadingSceneState(mUIFacade);
    }
    //将UI面板放回工厂
    public void PushUIPanel(string uiPanelName,GameObject uiPanelGo)
    {
        mGameManager.PushGameObjectToFactory(FactoryType.UIPanelFactory,uiPanelName,uiPanelGo);
    }

    //清空UI面板实例的字典数据
    public void ClearDict()
    {
        foreach (var item in currentScenePanelDict)
        {//这里推入栈的时候，使用的是item.Key，也就是键，因为物体的值的名称item.Value.name是带克隆体的，不是预制体
            PushUIPanel(item.Key,item.Value.gameObject);
        }
        currentScenePanelDict.Clear();
    }
}
