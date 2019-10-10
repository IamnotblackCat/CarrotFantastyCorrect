using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTower : MonoBehaviour {

    public int towerID;
    private int price;
    private Button button;
    private Sprite canClickSprite;
    private Sprite canNotClickSprite;
    //private GameController GameController.Instance;
    private Image image;

    private void OnEnable()
    {
        if (price==0)
        {
            return;
        }
        UpdateCoin();
    }
    private void Start()
    {
        //GameController.Instance = GameController.Instance;
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        canClickSprite = GameController.Instance.GetSprite("NormalMordel/Game/Tower/"+towerID+"/CanClick1");
        canNotClickSprite= GameController.Instance.GetSprite("NormalMordel/Game/Tower/" + towerID + "/CanClick0");
        //这里我先修改了价格，就不会出现价格和金币都为0了，老师设置的是金币=价格不能购买，我觉得可以买
        price = GameController.Instance.towerPriceDict[towerID];
        UpdateCoin();
        button.onClick.AddListener(BuildTower);
    }
    
    private void UpdateCoin()
    {
        if (GameController.Instance.coin>=price)
        {
            image.sprite = canClickSprite;
            button.interactable = true;
        }
        else
        {
            image.sprite = canNotClickSprite;
            button.interactable = false;
        }
    }
    //建造塔
    private void BuildTower()
    {
        GameController.Instance.PlayEffectClip("NormalMordel/Tower/TowerBuild");
        GameController.Instance.towerBuilder.m_TowerID = towerID;
        GameController.Instance.towerBuilder.m_TowerLevel = 1;

        GameObject towerGo = GameController.Instance.towerBuilder.GetProduct();
        towerGo.transform.position = GameController.Instance.selectGrid.transform.position;
        towerGo.transform.SetParent(GameController.Instance.selectGrid.transform);

        GameObject effectGo = GameController.Instance.GetGameObjectResource("BuildEffect");
        effectGo.transform.position = GameController.Instance.selectGrid.transform.position;
        effectGo.transform.SetParent(GameController.Instance.transform);
        //处理格子
        GameController.Instance.selectGrid.AfterBuildTower();
        //完成创建之后的后续方法
        GameController.Instance.selectGrid.HideGrid();
        GameController.Instance.selectGrid.hasTower = true;
        GameController.Instance.ChangeCoin(-price);

        //选中的格子滞空
        GameController.Instance.selectGrid = null;
        //让升级销售的画布先隐藏一次，这样再次出现就会调用Onenable方法，重新计算一次数值，重新图片切换
        GameController.Instance.handleTowerCanvasGo.SetActive(false);
    }
}
