using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour {

    public float animationTime;
    public string resourcePath;
    // Use this for initialization
    private void OnEnable()
    {
        Invoke("DestroyEffect",0.433f);
    }
    private void DestroyEffect()
    {
        GameManager.Instance.factoryManager.factoryDict[FactoryType.UIFactory].PushItem(resourcePath,gameObject);
    }
}
