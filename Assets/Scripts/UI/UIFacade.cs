using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIFacade  {
    //管理者们
    private GameManager mGameManager;
    public PlayerManager mPlayerManager;
    private AudioSourceManager mAudioSourceManager;
    private UIManager mUIManager;
    //下层UI，的脚本
    public Dictionary<string, IBasePanel> currentScenePanelDict = new Dictionary<string, IBasePanel>();
    public IBaseSceneState lastSceneState;
    public IBaseSceneState currentSceneState;
    //遮罩，让界面慢慢显示出来
    private GameObject mask;
    private Image maskImage;
    //Canvas只有这一个，所以设置为不要摧毁
    public Transform canvasTransform;

    public UIFacade(UIManager uiManager)
    {
        //这个赋值是为了封装，并且省略很多次GameManager
        mGameManager = GameManager.Instance;
        mPlayerManager = mGameManager.playerManager;
        mAudioSourceManager = mGameManager.audioSourceManager;
        mUIManager = uiManager;
        InitMask();
        
    }
    public void InitMask()
    {
        canvasTransform = GameObject.Find("Canvas").transform;
        // mask = GetGameObjectResource(FactoryType.UIFactory,"Img_Mask");
        mask = CreateUIAndSetUIPosition("Img_Mask");
        maskImage = mask.GetComponent<Image>();

    }
    //进行场景状态切换——这里就是状态模式的具体实现
    public void ChangeSceneState(IBaseSceneState baseSceneState)
    {
        //现有场景已经成为过去式
        lastSceneState = currentSceneState;
        ShowMask();
        //需要切换的场景变为现有场景
        currentSceneState = baseSceneState;
    }
    //显示遮罩，用来切换场景
    public void ShowMask()
    {
        //这个API代表在画布上 渲染顺序，数字越大越靠后，越覆盖
        mask.transform.SetSiblingIndex(10);
        //俩参数：遮罩的color，差值。想要变的值=差值，目标值，时间。
        Tween t = DOTween.To(() => maskImage.color, lerpColor => maskImage.color = lerpColor, new Color(0, 0, 0, 1), 1f);
        t.OnComplete(ExitSceneComplete);
    }
    //离开当前场景的方法
    private void ExitSceneComplete()
    {
        lastSceneState.ExitScene();
        currentSceneState.EnterScene();
        HideMask();
    }
    //隐藏遮罩
    public void HideMask()
    {
        //这个API代表在画布上 渲染顺序，数字越大越靠后，越覆盖
        mask.transform.SetSiblingIndex(10);
        //这里不用tween接收原因是不需要回调
        DOTween.To(() => maskImage.color, lerpColor => maskImage.color = lerpColor, new Color(0, 0, 0, 0), 1f);
    }
    //实例化当前场景所有面板，并存入字典.因为它这个其实是切换场景以后，清空字典并重新添加面板
    public void InitDict()
    {
        foreach (var item in mUIManager.currentScenePanelDict)
        {
            //注意这个地方的item是一个键值对，不是单纯的string，是字典的一个完整元素，包含了string和gameObject
            item.Value.transform.SetParent(canvasTransform);
            item.Value.transform.localPosition = Vector3.zero;
            item.Value.transform.localScale = Vector3.one;
            IBasePanel basePanel = item.Value.GetComponent<IBasePanel>();
            if (basePanel==null)
            {
                Debug.Log("获取UI面板:"+item+"的脚本失败");
            }
            //else
            //{
            //    Debug.Log("成功获取UI面板:" + item );
            //}
            basePanel.InitPanel();
            currentScenePanelDict.Add(item.Key,basePanel);//存储面板上面的脚本
            if (item.Key==StringManager.HelpPanel)
            {
                //Debug.Log("帮助面板已添加");
            }
            
        }
    }
    //添加UI到UIManager的字典里面
    public void AddPanelToDict(string uiPanelName)
    {
        mUIManager.currentScenePanelDict.Add(uiPanelName,GetGameObjectResource(FactoryType.UIPanelFactory,uiPanelName));
        //Debug.Log("添加UI到字典 ：" + uiPanelName);
    }
    /// <summary>
    /// 从当前字典里面清除键值对数据。需要注意的是，这个仅仅是清除了字典里面存储的数据，物体本身依旧挂着IBasePanel的脚本
    /// 还需要从UIManager里面清除数据，也是一样，UIManager字典仅仅是删除了字典的数据，物体本身的实例依旧存在。所以UIManager里面不能简单的使用清除命令，而是摧毁物体的实例。
    /// 因为使用了对象池技术，应该要使用推进对象池
    /// </summary>
    public void ClearDict()
    {
        //Debug.Log("清除字典");
        currentScenePanelDict.Clear();
        mUIManager.ClearDict();
    }
    //实例化UI
    public GameObject CreateUIAndSetUIPosition(string uiName)
    {
        GameObject ui = null;
        ui = GetGameObjectResource(FactoryType.UIFactory,uiName);
        ui.transform.SetParent(canvasTransform);
        ui.transform.localPosition = Vector3.zero;
        ui.transform.localScale = Vector3.one;

        return ui;
    }
    //对GameManager里面的方法进行进一步的封装，下层不用知道管理层的存在
    public Sprite GetSprite(string resourcePath)
    {
        return mGameManager.GetSprite(resourcePath);
    }
    public AudioClip GetAudioClip(string resourPath)
    {
        return mGameManager.GetAudioClip(resourPath);
    }
    public GameObject GetGameObjectResource(FactoryType factoryType,string resourcePath)
    {
        return mGameManager.GetGameObjectResource(factoryType,resourcePath);
    }
    public void PushGameObjectResource(FactoryType factoryType,string resourcePath,GameObject itemGo)
    {
        mGameManager.PushGameObjectToFactory(factoryType,resourcePath,itemGo);
    }
    //封装音效管理者，让下层ui面板调用
    public void CloseOrOpenBGMusic()
    {
        mAudioSourceManager.CloseOrOpenBGMusic();
    }
    public bool NeedPlayBGMusic()
    {
        return mAudioSourceManager.playBGMusic;
    }
    public void CloseOrOpenEffectMusic()
    {
        mAudioSourceManager.CloseOrOpenEffectMusic();
    }
    //封装按钮和翻书音效
    public void PlayButtonAduioClip()
    {
        mAudioSourceManager.PlayButtonAudioClip();
    }
    public void PlayPagingAudioClip()
    {
        mAudioSourceManager.PlayPagingAudioClip();
    }
}
