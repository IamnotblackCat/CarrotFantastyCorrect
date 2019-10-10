using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeAnimatorControllerFactory : IBaseResourceFactory<RuntimeAnimatorController> {

    protected Dictionary<string, RuntimeAnimatorController> factoryDict = new Dictionary<string, RuntimeAnimatorController>();
    protected string loadPath;

    public RuntimeAnimatorControllerFactory()
    {
        loadPath = "Animator/AnimatorController/";
    }

    public RuntimeAnimatorController GetSingleResources(string resourcePath)
    {
        RuntimeAnimatorController itemGo = null;
        string itemLoadPath = loadPath + resourcePath;

        if (factoryDict.ContainsKey(resourcePath))
        {
            itemGo = factoryDict[resourcePath];
        }
        else
        {
            itemGo = Resources.Load<RuntimeAnimatorController>(itemLoadPath);
            factoryDict.Add(resourcePath, itemGo);
        }
        //安全校验
        if (itemGo == null)
        {
            Debug.Log("目标资源无法获取：" + resourcePath + "失败路径是" + itemLoadPath);
        }
        return itemGo;
    }
}
