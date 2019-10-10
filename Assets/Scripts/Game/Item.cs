using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour {

    public GridPoint gridPoint;
    public int itemID;
    private GameController gameController = GameController.Instance;

    private int HP;
    private int currentHP;
    private Slider slider;
    private float timeVal;//计时器，血条不被攻击三秒后消失
    private bool showHp;
    private int prize;//玩家摧毁物体以后给的奖励
                      // Use this for initialization
    private void OnEnable()
    {
        if (itemID==0)//start方法初始过了，防止第一次的时候初始两次
        {
            return;
        }
        InitItem();
    }
    void Start () {
        slider = transform.Find("ItemCanvas").Find("HpSlider").GetComponent<Slider>();
        showHp = true;
        InitItem();
	}
	
	// Update is called once per frame
	void Update () {
        if (showHp)
        {
            if (timeVal<=0)
            {
                slider.gameObject.SetActive(false);
                showHp = false;
                timeVal = 3;
            }
            else
            {
                timeVal -= Time.deltaTime;
            }
        }
	}
#if Game
    private void TakeDamage(int attackValue)
    {
        currentHP -= attackValue;
        slider.gameObject.SetActive(true);
        if (currentHP<=0)
        {
            DestroyItem();
            return;
        }
        slider.value =(float)currentHP / HP;
        timeVal = 3;
        showHp = true;
    }
    private void DestroyItem()
    {
        //销毁物体
        if (gameController.targetTrans==transform)//如果还是集火目标，隐藏标记
        {
            gameController.HideSignal();
        }
        //金币奖励
        GameObject coinGo = gameController.GetGameObjectResource("CoinCanvas");
        coinGo.transform.Find("Emp_Coin").GetComponent<CoinMove>().prize=prize;//赋值金币数
        coinGo.transform.position = transform.position;
        coinGo.transform.SetParent(gameController.transform);
        gameController.ChangeCoin(prize);
        //销毁特效
        GameObject effectGo = gameController.GetGameObjectResource("DestoryEffect");
        effectGo.transform.position = transform.position;
        effectGo.transform.SetParent(gameController.transform);

        gameController.PushGameObjectToFactory(gameController.mapMaker.bigLevelID+"/Item/"+itemID,gameObject);
        gridPoint.gridState.hasItem = false;
        InitItem();
        gameController.PlayEffectClip("NormalMordel/Item");
    }
    private void InitItem()
    {
        prize = 1000 - 100 * itemID;//越小的东西越便宜嘛
        HP = 1000 - 150 * itemID;
        currentHP = HP;
        timeVal = 3;
    }
    //被集火
    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (gameController.targetTrans == null)
        {
            gameController.targetTrans = transform;
            gameController.ShowSignal();
        }
        else if (gameController.targetTrans != transform)//更换目标
        {
            gameController.HideSignal();
            gameController.targetTrans = transform;
            gameController.ShowSignal();
        }
        else if (gameController.targetTrans == transform)//点的同一个
        {
            gameController.HideSignal();
        }
    }
#endif
}
