using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipFactory : IBaseResourceFactory<AudioClip>
{
    protected Dictionary<string, AudioClip> factoryDict = new Dictionary<string, AudioClip>();
    protected string loadPath;

    public AudioClipFactory()
    {
        loadPath = "AudioClips/";
    }

    public AudioClip GetSingleResources(string resourcePath)
    {
        AudioClip itemGo = null;
        string itemLoadPath = loadPath + resourcePath;

        if (factoryDict.ContainsKey(resourcePath))
        {
            itemGo = factoryDict[resourcePath];
        }
        else
        {
            itemGo = Resources.Load<AudioClip>(itemLoadPath);
            factoryDict.Add(resourcePath,itemGo);
        }
        //安全校验
        if (itemGo==null)
        {
            Debug.Log("目标资源无法获取："+resourcePath+"失败路径是"+itemLoadPath);
        }
        return itemGo;
    }
}
