using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAwake : MonoBehaviour {

    private void Awake()
    {
        InvokeNow();
    }
    private void InvokeNow()
    {
        Debug.Log("正在调用");
    }
}
