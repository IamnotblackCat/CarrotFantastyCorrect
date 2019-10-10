using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameWinPage : MonoBehaviour {

    private Text text_RoundCount;
    private Text text_TotalCount;
    private Text text_CurrentLeve;
    private Image img_Carrot;
    private Sprite[] carrotSprites;//0,金  1.银  2正常萝卜
    private NormalModelPanel normalModelPanel;

    private void Awake()
    {
        carrotSprites = new Sprite[3];
        normalModelPanel = GetComponentInParent<NormalModelPanel>();
        text_RoundCount = transform.Find("Img_BG").Find("Text_RoundCount").GetComponent<Text>();
        text_TotalCount = transform.Find("Img_BG").Find("Text_TotalCount").GetComponent<Text>();
        img_Carrot = transform.Find("Img_BG").Find("Image").GetComponent<Image>();
        text_CurrentLeve = transform.Find("Img_BG").transform.Find("Img_Instruction").Find("Text_CurrentLevel").GetComponent<Text>();
        for (int i = 0; i < carrotSprites.Length; i++)
        {
            carrotSprites[i] = GameController.Instance.GetSprite("GameOption/Normal/Level/Carrot_" + (i + 1).ToString());
        }
        //Debug.Log("?");
    }
    private void OnEnable()
    {
        //Debug.Log(normalModelPanel.totalRound);
        text_TotalCount.text = normalModelPanel.totalRound.ToString();
        text_CurrentLeve.text = (GameController.Instance.currentStage.mLevelID + (GameController.Instance.currentStage.mBigLevelID - 1) * 5).ToString();
        normalModelPanel.ShowRoundText(text_RoundCount);
        if (GameController.Instance.carrotHP>=4)
        {
            img_Carrot.sprite = carrotSprites[0];
        }
        else if (GameController.Instance.carrotHP>=2)
        {
            img_Carrot.sprite = carrotSprites[1];
        }
        else
        {
            img_Carrot.sprite = carrotSprites[2];
        }
    }
    public void Replay()
    {
        normalModelPanel.Replay();
    }
    public void ChooseOtherLevel()
    {
        normalModelPanel.ChooseOtherLevel();
    }
}
