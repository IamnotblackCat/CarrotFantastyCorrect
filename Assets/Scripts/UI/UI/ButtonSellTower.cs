using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSellTower : MonoBehaviour {

    private int price;
    private GameController gameController;
    private Button button;
    private Text text;

	// Use this for initialization
	void Start () {
        gameController = GameController.Instance;
        button = GetComponent<Button>();
        text =transform.Find("Text").GetComponent<Text>();
        button.onClick.AddListener(SellTower);
	}

    private void OnEnable()
    {//这一步的目的是，先让start方法执行一次给成员变量赋值，因为price需要拿的值，是在towerPersonalProPerty那个脚本的Start方法里的
        //只能在那个脚本的sellPrice变量赋值完毕以后，才能转给这个变量，所以price要放在这个start的后面
        if (text==null)
        {
            return;
        }
        price = gameController.selectGrid.towerPersonalProperty.sellPrice;
        text.text = price.ToString();
    }
    private void SellTower()
    {
        //Debug.Log("调用");
        gameController.ChangeCoin(price);
        gameController.selectGrid.towerPersonalProperty.SellTower();
        gameController.selectGrid.InitGrid();
        gameController.handleTowerCanvasGo.SetActive(false);//这里老师用的是格子内部的那个画布，但是那个画布其实也是这个画布
        gameController.selectGrid.HideGrid();
        gameController.selectGrid = null;
    }
}
