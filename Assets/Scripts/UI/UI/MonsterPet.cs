using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterPet : MonoBehaviour {

    public MonsterPetData monsterPetData;
    private GameObject[] monsterPetLevelGo;//三种等级的预制体
    public Sprite[] feedSprites;//0 可用牛奶，1不可用牛奶，2可用饼干，3不可用饼干
    //Egg
    private GameObject img_InstructionGo;
    //Baby
    private GameObject emp_FeedGo;
    private Text text_Cookie;
    private Text text_Milk;
    private Image img_Cookie;
    private Image img_Milk;
    private Button btn_Cookie;
    private Button btn_Milk;
    //Normal
    private GameObject img_TalkRight;
    private GameObject img_TalkLeft;

    public MonsterNestPanel monsterNestPanel;

    private void Awake()
    {
        monsterPetLevelGo = new GameObject[3];
        monsterPetLevelGo[0] = transform.Find("Emp_Egg").gameObject;
        monsterPetLevelGo[1] = transform.Find("Emp_Baby").gameObject;
        monsterPetLevelGo[2] = transform.Find("Emp_Normal").gameObject;

        //Egg
        img_InstructionGo = monsterPetLevelGo[0].transform.Find("Img_Instruction").gameObject;

        img_InstructionGo.SetActive(false);
        //Baby
        emp_FeedGo = monsterPetLevelGo[1].transform.Find("Emp_Feed").gameObject;
        emp_FeedGo.SetActive(false);
        btn_Milk = monsterPetLevelGo[1].transform.Find("Emp_Feed").Find("Btn_Milk").GetComponent<Button>();
        img_Milk = monsterPetLevelGo[1].transform.Find("Emp_Feed").Find("Btn_Milk").GetComponent<Image>();
        btn_Cookie = monsterPetLevelGo[1].transform.Find("Emp_Feed").Find("Btn_Cookies").GetComponent<Button>();
        img_Cookie = monsterPetLevelGo[1].transform.Find("Emp_Feed").Find("Btn_Cookies").GetComponent<Image>();
        text_Milk = monsterPetLevelGo[1].transform.Find("Emp_Feed").Find("Btn_Milk").Find("Text").GetComponent<Text>();
        text_Cookie = monsterPetLevelGo[1].transform.Find("Emp_Feed").Find("Btn_Cookies").Find("Text").GetComponent<Text>();
        //Normal
        img_TalkLeft = transform.Find("Img_TalkLeft").gameObject;
        img_TalkRight = transform.Find("Img_TalkRight").gameObject;
    }
    private void OnEnable()
    {
        InitMonsterPet();    
    }

    public void InitMonsterPet()
    {
        if (monsterPetData.levelUpNeedMilk==0)
        {
            monsterPetData.levelUpNeedMilk = monsterPetData.monsterID * 60;
        }
        if (monsterPetData.levelUpNeedCookies==0)
        {
            monsterPetData.levelUpNeedCookies = monsterPetData.monsterID * 30;
        }
        ShowMonster();
    }

    public void ClickPet()
    {
        //Debug.Log(monsterPetData.monsterLevel);
        GameManager.Instance.audioSourceManager.PlayEffect(GameManager.Instance.GetAudioClip("MonsterNest/PetSound"+monsterPetData.monsterLevel));
        switch (monsterPetData.monsterLevel)
        {
            case 1:
                if (GameManager.Instance.playerManager.monsterNest>=1)
                {
                    GameManager.Instance.playerManager.monsterNest--;
                    ToNormal();
                    monsterPetData.monsterLevel++;
                    ShowMonster();
                    monsterNestPanel.UpdateText();
                }
                else
                {
                    img_InstructionGo.SetActive(true);
                    Invoke("CloseTalkUI",2);
                }
                break;
            case 2://幼年形态，点击的时候 打开/关闭  喂养按钮，更新喂养的图片
                //Debug.Log(emp_FeedGo.activeSelf);
                if (emp_FeedGo.activeSelf)
                {
                    emp_FeedGo.SetActive(false);
                }
                else
                {
                    //Debug.Log("1");
                    emp_FeedGo.SetActive(true);
                    if (GameManager.Instance.playerManager.milk>0)
                    {
                        img_Milk.sprite = feedSprites[0];
                        btn_Milk.interactable = true;
                    }
                    else
                    {
                        img_Milk.sprite = feedSprites[1];
                        btn_Milk.interactable = false;
                    }
                    if (GameManager.Instance.playerManager.cookies>0)
                    {
                        img_Cookie.sprite = feedSprites[2];
                        btn_Cookie.interactable = true;
                    }
                    else
                    {
                        img_Cookie.sprite = feedSprites[3];
                        btn_Cookie.interactable = false;
                    }
                    if (monsterPetData.levelUpNeedCookies==0)
                    {
                        btn_Cookie.interactable = false;
                    }
                    else
                    {
                        text_Cookie.text = monsterPetData.levelUpNeedCookies.ToString();
                        btn_Cookie.interactable = true;
                    }
                    if (monsterPetData.levelUpNeedMilk==0)
                    {
                        btn_Milk.interactable = false;
                    }
                    else
                    {
                        text_Milk.text = monsterPetData.levelUpNeedMilk.ToString();
                        btn_Milk.interactable = true;
                    }
                }
                break;
            case 3:
                int randomNum = Random.Range(0,2);
                if (randomNum==0)
                {
                    img_TalkLeft.SetActive(true);
                    Invoke("CloseTalkUI",2);
                }
                else
                {
                    img_TalkRight.SetActive(true);
                    Invoke("CloseTalkUI", 2);
                }
                break;
            default:
                break;
        }
    }
    //成长方法
    public void ToNormal()
    {
        //Debug.Log(monsterPetData.levelUpNeedCookies+"牛奶剩余需要"+monsterPetData.levelUpNeedMilk);
        if (monsterPetData.levelUpNeedCookies==0&&monsterPetData.levelUpNeedMilk==0)
        {
            //先加再判断
            monsterPetData.monsterLevel++;
            GameManager.Instance.audioSourceManager.PlayEffect(GameManager.Instance.GetAudioClip("MonsterNest/PetChange"));
            if (monsterPetData.monsterLevel>=3)
            {//按照怪物的ID来解锁隐藏关卡
                GameManager.Instance.playerManager.unlockedNormalModelLevelList[monsterPetData.monsterID * 5 - 1].unLocked = true;
                GameManager.Instance.playerManager.burriedLevelNum++;
                ShowMonster();
            }
            else
            {
                ShowMonster();
            }
        }
        SaveMonsterData();
    }
    //显示当前等级的宠物
    private void ShowMonster()
    {
        for (int i = 0; i < monsterPetLevelGo.Length; i++)
        {
            monsterPetLevelGo[i].SetActive(false);//先把所有的状态关了，蛋/baby/成年，都关了，然后当前状态是哪个，就只开哪一个
            //Debug.Log(monsterPetData.monsterLevel + "i+1=    " + (i + 1));
            if ((i+1)==monsterPetData.monsterLevel)
            {
               
                monsterPetLevelGo[i].SetActive(true);
                Sprite petSprite = null;
                switch (monsterPetData.monsterLevel)
                {
                    case 1:
                        petSprite = GameManager.Instance.GetSprite("MonsterNest/Monster/Egg/"+monsterPetData.monsterID);
                        break;
                    case 2:
                        petSprite = GameManager.Instance.GetSprite("MonsterNest/Monster/Baby/" + monsterPetData.monsterID);
                        break;
                    case 3:
                        petSprite = GameManager.Instance.GetSprite("MonsterNest/Monster/Normal/" + monsterPetData.monsterID);
                        break;
                    default:
                        break;
                }
                Image monsterImage = monsterPetLevelGo[i].transform.Find("Img_Pet").GetComponent<Image>();
                monsterImage.sprite = petSprite;
                monsterImage.SetNativeSize();
                //float imageLocalScale = 0;
                //if (monsterPetData.monsterLevel==1)
                //{
                //    imageLocalScale = 2;
                //}
                //else
                //{
                //    imageLocalScale = 1 + (monsterPetData.monsterLevel-1) * 0.5f;
                //}
                //monsterImage.transform.localScale = new Vector3(imageLocalScale,imageLocalScale,1);
            }
            
        }
        
    }
    private void CloseTalkUI()
    {
        img_InstructionGo.SetActive(false);
        img_TalkLeft.SetActive(false);
        img_TalkRight.SetActive(false);
    }
    private void SaveMonsterData()
    {
        for (int i = 0; i < GameManager.Instance.playerManager.monsterPetDataList.Count; i++)
        {
            if (GameManager.Instance.playerManager.monsterPetDataList[i].monsterID==monsterPetData.monsterID)
            {
                GameManager.Instance.playerManager.monsterPetDataList[i] = monsterPetData;
            }
        }
    }

    public void FeedMilk()
    {
        GameManager.Instance.audioSourceManager.PlayEffect(GameManager.Instance.GetAudioClip("MonsterNest/Feed01"));
        GameObject heartGo = GameManager.Instance.factoryManager.factoryDict[FactoryType.UIFactory].GetItem("Img_Heart");
        heartGo.transform.position = transform.position;
        monsterNestPanel.SetCanvasTrans(heartGo.transform);
        if (GameManager.Instance.playerManager.milk>=monsterPetData.levelUpNeedMilk)
        {
            GameManager.Instance.playerManager.milk -= monsterPetData.levelUpNeedMilk;
            monsterPetData.levelUpNeedMilk = 0;
            

        }
        else
        {
            monsterPetData.levelUpNeedMilk -= GameManager.Instance.playerManager.milk;
            GameManager.Instance.playerManager.milk = 0;
            btn_Milk.interactable = false;
        }
        monsterNestPanel.UpdateText();
        emp_FeedGo.SetActive(false);
        Invoke("ToNormal",0.433f);//等待爱心特效动画完成
    }
    public void FeedCookies()
    {
        GameManager.Instance.audioSourceManager.PlayEffect(GameManager.Instance.GetAudioClip("MonsterNest/Feed02"));
        GameObject heartGo = GameManager.Instance.factoryManager.factoryDict[FactoryType.UIFactory].GetItem("Img_Heart");
        heartGo.transform.position = transform.position;
        monsterNestPanel.SetCanvasTrans(heartGo.transform);
        if (GameManager.Instance.playerManager.cookies >= monsterPetData.levelUpNeedCookies)
        {
            GameManager.Instance.playerManager.cookies -= monsterPetData.levelUpNeedCookies;
            monsterPetData.levelUpNeedCookies = 0;
           

        }
        else
        {
            monsterPetData.levelUpNeedCookies -= GameManager.Instance.playerManager.cookies;
            GameManager.Instance.playerManager.cookies = 0;
            btn_Cookie.interactable = false;
        }
        monsterNestPanel.UpdateText();
        emp_FeedGo.SetActive(false);
        Invoke("ToNormal", 0.433f);//等待爱心特效动画完成
    }
}
public struct MonsterPetData
{
    public int monsterLevel;//蛋、baby、normal三个等级状态
    public int levelUpNeedCookies;
    public int levelUpNeedMilk;
    public int monsterID;
}
