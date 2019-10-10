using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 这个是游戏总管理，管理其他的管理者
/// </summary>
public class GameManager : MonoBehaviour {

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public PlayerManager playerManager;
    public FactoryManager factoryManager;
    public AudioSourceManager audioSourceManager;
    public UIManager uiManager;
    public Stage currentStage;

    public bool isInitPlayerManager;//是否重置游戏
    private void Awake()
    {
        //注意这个代码要先加，这个脚本附着的物体不能被销毁
        DontDestroyOnLoad(gameObject);
        _instance = this; 
        //开始实例化
        playerManager = new PlayerManager();
        //playerManager.SaveData();
        playerManager.ReadData();
        factoryManager = new FactoryManager();
        audioSourceManager = new AudioSourceManager();
        uiManager = new UIManager();
        //进入游戏开始加载界面
        uiManager.mUIFacade.currentSceneState.EnterScene();
    }
    public GameObject CreateGO(GameObject itemGo)
    {
        GameObject go= Instantiate(itemGo);
        return go;
    }
    //获取精灵资源
    public Sprite GetSprite(string resourcePath)
    {
        return factoryManager.spriteFactory.GetSingleResources(resourcePath);
    }
    //获取音效资源
    public AudioClip GetAudioClip(string resourcePath)
    {
        return factoryManager.audioClipFactory.GetSingleResources(resourcePath);
    }
    public RuntimeAnimatorController GetRuntimeAnimatorController(string resourcePath)
    {
        return factoryManager.runtimeAnimatorControllerFactory.GetSingleResources(resourcePath);
    }
    //获取游戏物体,这里要区分三种枚举情况,因为三种文件夹路径不同
    public GameObject GetGameObjectResource(FactoryType factoryType,string resourcePath)
    {
        return factoryManager.factoryDict[factoryType].GetItem(resourcePath);
    }
    //用完了，把游戏物体放回对象池
    public void PushGameObjectToFactory(FactoryType factoryType, string resourcePath,GameObject itemGo)
    {
        factoryManager.factoryDict[factoryType].PushItem(resourcePath,itemGo);
    }
}
