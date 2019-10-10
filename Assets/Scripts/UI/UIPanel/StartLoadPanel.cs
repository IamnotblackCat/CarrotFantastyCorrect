using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLoadPanel : BasePanel {

    protected override void Awake()
    {
        base.Awake();
        EnterPanel();
    }
    private void Update()
    {
        
    }
    public override void EnterPanel()
    {
        base.EnterPanel();
        Invoke("LoadNextScene",1f);
    }
    private void LoadNextScene()
    {
        //这个参数IBaseSceneState，其实就是场景，每一个场景都新建了自己的场景类
        mUIFacade.ChangeSceneState(new MainSceneState(mUIFacade));
        
    }
}
