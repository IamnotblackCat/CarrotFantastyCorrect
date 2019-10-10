using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUpLevel : MonoBehaviour {

    private int price;
    private Text text;
    private GameController gameController;
    private Button button;
    private Image image;
    private Sprite canUpLevel;
    private Sprite canNotUpLevel;
    private Sprite reachHighestLevel;

    private void OnEnable()
    {
        if (text==null)
        {
           // Debug.Log("A");
            return;
        }
        //Debug.Log("B");
        UpdateUIView();
    }
    // Use this for initialization
    void Start () {
        gameController = GameController.Instance;
        button = GetComponent<Button>();
        button.onClick.AddListener(UpLevel);
        canUpLevel = gameController.GetSprite("NormalMordel/Game/Tower/Btn_CanUpLevel");
        canNotUpLevel = gameController.GetSprite("NormalMordel/Game/Tower/Btn_CantUpLevel");
        reachHighestLevel = gameController.GetSprite("NormalMordel/Game/Tower/Btn_ReachHighestLevel");
        image = GetComponent<Image>();
        text = transform.Find("Text").GetComponent<Text>();
        //Debug.Log("D");
        //UpdateUIView();不明白为什么这里会价格报空，是因为都是start赋值的关系？
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void UpdateUIView()
    {
        //Debug.Log("1");
        text.enabled = true;
        price = gameController.selectGrid.towerPersonalProperty.upLevelPrice;
        text.text = price.ToString();
        if (gameController.selectGrid.towerLevel>=3)
        {
            image.sprite = reachHighestLevel;
            button.interactable = false;
            text.enabled = false;
        }
        else//一二级塔的建造
        {
            if (gameController.coin>=price)
            {
                image.sprite = canUpLevel;
                button.interactable = true;
                text.enabled = true;
            }
            else
            {
                image.sprite = canNotUpLevel;
                button.interactable = false;
                text.enabled = false;
            }
        }
    }
    //升级建造
    private void UpLevel()
    {
        //赋值建造者要产生的塔的属性
        gameController.towerBuilder.m_TowerID = gameController.selectGrid.tower.towerID;
        gameController.towerBuilder.m_TowerLevel = gameController.selectGrid.towerLevel+1;
        //拿到塔所在的格子，销毁旧的塔
        gameController.selectGrid.towerPersonalProperty.UpLevelTower();//包含了摧毁方法
        //建立新塔
        GameObject towerGo = gameController.towerBuilder.GetProduct();
        towerGo.transform.position = gameController.selectGrid.transform.position;
        towerGo.transform.SetParent(gameController.selectGrid.transform);

        gameController.selectGrid.AfterBuildTower();
        gameController.selectGrid.HideGrid();
        gameController.selectGrid = null;

        
    }
    //private void OnDisable()
    //{
    //    Debug.Log("G");
    //}
}
