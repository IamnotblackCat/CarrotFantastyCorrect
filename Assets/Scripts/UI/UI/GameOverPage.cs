using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPage : MonoBehaviour {

    private Text text_RoundCount;
    private Text text_TotalCount;
    private Text text_CurrentLeve;
    private NormalModelPanel normalModelPanel;

    private void Awake()
    {
        normalModelPanel = GetComponentInParent<NormalModelPanel>();
        text_RoundCount = transform.Find("Img_BG").Find("Text_RoundCount").GetComponent<Text>();
        text_TotalCount= transform.Find("Img_BG").Find("Text_TotalCount").GetComponent<Text>();
        //Debug.Log(transform.Find("Img_BG"));
        text_CurrentLeve= transform.Find("Img_BG").Find("Img_Instruction ").Find("Text_CurrentLevel").GetComponent<Text>();
    }
    private void OnEnable()
    {
        text_TotalCount.text = normalModelPanel.totalRound.ToString();
        text_CurrentLeve.text = (GameController.Instance.currentStage.mLevelID + (GameController.Instance.currentStage.mBigLevelID - 1) * 5).ToString();
        normalModelPanel.ShowRoundText(text_RoundCount);
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
