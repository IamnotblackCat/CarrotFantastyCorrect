using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterNestPanel : BasePanel {

    private GameObject shopPanelGo;
    private Text text_Cookies;
    private Text text_Milk;
    private Text text_MonsterNest;
    private Text text_Diamand;
    private List<GameObject> monsterPetGoList;//管理baby，方便清除和添加，避免重复加载
    private Transform emp_MonsterGroup;

    protected override void Awake()
    {
        base.Awake();
        shopPanelGo = transform.Find("ShopPage").gameObject;
        text_Cookies = transform.Find("Img_TopPage").Find("Text_CookiesNum").GetComponent<Text>();
        text_Milk= transform.Find("Img_TopPage").Find("Text_MilkNum").GetComponent<Text>();
        text_MonsterNest= transform.Find("Img_TopPage").Find("Text_NestNum").GetComponent<Text>();
        text_Diamand = shopPanelGo.transform.Find("Img_Diamands").Find("Text_Diamands").GetComponent<Text>();
        emp_MonsterGroup = transform.Find("Emp_MonsterGroup");
        monsterPetGoList = new List<GameObject>();
        for (int i = 1; i < 4; i++)
        {
            mUIFacade.GetSprite("MonsterNest/Monster/Egg/" + i);
            mUIFacade.GetSprite("MonsterNest/Monster/Baby/" + i);
            mUIFacade.GetSprite("MonsterNest/Monster/Normal/" + i);
        }
    }
    private void OnEnable()
    {
        
    }
    public override void InitPanel()
    {
        base.InitPanel();
        for (int i = 0; i < monsterPetGoList.Count; i++)
        {
            mUIFacade.PushGameObjectResource(FactoryType.UIFactory, "Emp_Monsters",monsterPetGoList[i]);
        }
        monsterPetGoList.Clear();
        for (int i = 0; i < GameManager.Instance.playerManager.monsterPetDataList.Count; i++)
        {
            if (GameManager.Instance.playerManager.monsterPetDataList[i].monsterID!=0)
            {
                GameObject monsterPetGo = mUIFacade.GetGameObjectResource(FactoryType.UIFactory, "Emp_Monsters");
                //这三行代码。。。
                monsterPetGo.GetComponent<MonsterPet>().monsterPetData = mUIFacade.mPlayerManager.monsterPetDataList[i];
                monsterPetGo.GetComponent<MonsterPet>().monsterNestPanel = this;
                monsterPetGo.GetComponent<MonsterPet>().InitMonsterPet();


                monsterPetGo.transform.SetParent(emp_MonsterGroup);
                monsterPetGo.transform.localScale = Vector3.one;
                monsterPetGoList.Add(monsterPetGo);
            }
            
        }
        UpdateText();
    }
    //展示商店
    public void ShowShopPage()
    {
        mUIFacade.PlayButtonAduioClip();
        shopPanelGo.SetActive(true);
    }
    public void HideShopPage()
    {
        mUIFacade.PlayButtonAduioClip();
        shopPanelGo.SetActive(false);
    }

    public void UpdateText()
    {
        text_Cookies.text = GameManager.Instance.playerManager.cookies.ToString();
        text_Milk.text = GameManager.Instance.playerManager.milk.ToString();
        text_MonsterNest.text = GameManager.Instance.playerManager.monsterNest.ToString();
        text_Diamand.text = GameManager.Instance.playerManager.diamands.ToString();
    }
    public void ReturnToMain()
    {
        mUIFacade.PlayButtonAduioClip();
        mUIFacade.ChangeSceneState(new MainSceneState(mUIFacade));
    }

    public void BuyMonsterNest()
    {
        mUIFacade.PlayButtonAduioClip();
        if (GameManager.Instance.playerManager.diamands>=60)
        {
            GameManager.Instance.playerManager.diamands -= 60;
            GameManager.Instance.playerManager.monsterNest++;
        }
        UpdateText();
    }
    public void BuyCookies()
    {
        mUIFacade.PlayButtonAduioClip();
        if (GameManager.Instance.playerManager.diamands >= 10)
        {
            GameManager.Instance.playerManager.diamands -= 10;
            GameManager.Instance.playerManager.cookies+=15;
        }
        UpdateText();
    }
    public void BuyMilk()
    {
        mUIFacade.PlayButtonAduioClip();
        if (GameManager.Instance.playerManager.diamands >= 1)
        {
            GameManager.Instance.playerManager.diamands -= 1;
            GameManager.Instance.playerManager.milk+=10;
        }
        UpdateText();
    }
    public void SetCanvasTrans(Transform uiTrans)
    {
        uiTrans.SetParent(mUIFacade.canvasTransform);
    }
}
