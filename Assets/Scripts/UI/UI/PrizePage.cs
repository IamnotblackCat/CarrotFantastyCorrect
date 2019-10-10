using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrizePage : MonoBehaviour {

    public Image img_Prize;
    public Image img_Instruction;
    public Text text_PrizeName;
    public Animator animator;
    public NormalModelPanel normalModelPanel;
    //private PlayerManager playerManager; 可能会报空

    private void Awake()
    {
        img_Prize = transform.Find("Img_Prize").GetComponent<Image>();
        img_Instruction = transform.Find("Img_PrizeInstruction").GetComponent<Image>();
        text_PrizeName = img_Prize.transform.Find("Text_PrizeName").GetComponent<Text>();
        animator = GetComponent<Animator>();
        normalModelPanel = GetComponentInParent<NormalModelPanel>();
        //playerManager = GameManager.Instance.playerManager;
    }
    private void Start()
    {
        
    }
    private void OnEnable()
    {
        string prizeName = "";
        int randomNum;
        if (GameManager.Instance.playerManager.monsterPetDataList.Count >= 3)
        {
            randomNum = Random.Range(1, 4);
        }
        else
        {
            randomNum = Random.Range(1, 5);
        }
        if (randomNum >= 4 && GameManager.Instance.playerManager.monsterPetDataList.Count < 3)//一共三关，只给三个蛋
        {
            Debug.Log("当前拥有蛋疏朗："+ GameManager.Instance.playerManager.monsterPetDataList.Count+"本次随机数为"+randomNum);
            int randomEgg = 0;
            do
            {
                randomEgg = Random.Range(0, 4);
            } while (HasThePet(randomEgg));//一定要随机到没有得到的宠物为止。
            MonsterPetData monsterPetData = new MonsterPetData
            {
                monsterLevel = 1,
                levelUpNeedCookies = 0,
                levelUpNeedMilk = 0,
                monsterID = randomEgg
            };
            GameManager.Instance.playerManager.monsterPetDataList.Add(monsterPetData);
            prizeName = "宠物蛋";
        }
        else
        {
            switch (randomNum)
            {
                case 1:
                    prizeName = "Milk";
                    GameManager.Instance.playerManager.milk += 20;
                    break;
                case 2:
                    prizeName = "Cookies";
                    GameManager.Instance.playerManager.cookies += 20;
                    break;
                case 3:
                    prizeName = "Nest";
                    GameManager.Instance.playerManager.monsterNest++;
                    break;
                default:
                    break;
            }
        }
        text_PrizeName.text = prizeName;
        img_Prize.sprite = GameController.Instance.GetSprite("MonsterNest/Prize/Prize" + randomNum);
        img_Instruction.sprite = GameController.Instance.GetSprite("MonsterNest/Prize/Instruction" + randomNum);
    }

    private bool HasThePet(int monsterID)
    {
        for (int i = 0; i < GameManager.Instance.playerManager.monsterPetDataList.Count; i++)
        {
            if (GameManager.Instance.playerManager.monsterPetDataList[i].monsterID == monsterID)
                return true;
        }
        return false;
    }

    public void ClosePrizePage()
    {
        normalModelPanel.HidePrizePage();
        GameController.Instance.isPause = false;
    }
}
