using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prize : MonoBehaviour {
    private void Update()
    {
        if (GameController.Instance.gameOver)
        {
            DestroyImmediate(this);
        }
    }
    private void OnMouseDown()
    {
        GameController.Instance.PlayEffectClip("NormalMordel/GiftGot");
        GameController.Instance.isPause = true;
        //GameController.Instance.
        GameController.Instance.PushGameObjectToFactory("Prize",gameObject);//鼠标点中，推入对象池
        GameController.Instance.ShowPrizePage();
    }
}
