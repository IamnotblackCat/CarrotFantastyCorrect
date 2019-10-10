using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CoinMove : MonoBehaviour {

    private Text coinText;
    private Image coinImage;

    [HideInInspector]
    public Sprite[] coinSprites;

    [HideInInspector]
    public int prize;

    private void Awake()
    {
        coinSprites = new Sprite[2];
        coinText = gameObject.transform.Find("Text_Coin").GetComponent<Text>() ;
        coinImage = gameObject.transform.Find("Img_Coin").GetComponent<Image>();
        coinSprites[0] = GameController.Instance.GetSprite("NormalMordel/Game/Coin");
        coinSprites[1] = GameController.Instance.GetSprite("NormalMordel/Game/ManyCoin");
        ShowCoin();
    }
    //每次唤醒游戏物体的时候都需要show一次
    private void OnEnable()
    {
        ShowCoin();
    }
    private void ShowCoin()
    {
        coinText.text = prize.ToString();
        if (prize>=500)
        {
            coinImage.sprite = coinSprites[1];
        }
        else
        {
            coinImage.sprite = coinSprites[0];
        }
        //Debug.Log(transform.localPosition.y);
        transform.DOLocalMoveY(180,1.5f);
        //Debug.Log("移动后"+transform.localPosition.y);
        DOTween.To(()=>coinImage.color,toColor=>coinImage.color=toColor,new Color(1,1,1,0.5f),1.5f);
        //要一个对象来接收text的动画，因为在动画结束后要有一个回调函数，推入对象池
        Tween tween = DOTween.To(() => coinText.color, toColor => coinText.color = toColor, new Color(1, 1, 1, 0.5f), 1.5f);
        tween.OnComplete(DestroyCoin);
    }

    private void DestroyCoin()
    {//脚本是挂载emp上面的，但是要推入对象池的是整个画布
       
        transform.localPosition = Vector3.zero;//这里移动位置不是局部位置
        coinImage.color = new Color(1,1,1,1);
        coinText.color = new Color(1,1,1,1);
        GameController.Instance.PushGameObjectToFactory("CoinCanvas",transform.parent.gameObject);
    }
}
