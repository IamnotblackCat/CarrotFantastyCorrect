using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 顶部UI显示
/// </summary>
public class TopPage : MonoBehaviour {
    //引用
    private Text text_coin;
    private Text tex_roundCount;
    private Text tex_TotalCount;
    private Image img_Btn_GameSpeed;
    private Image img_Btn_Pause;
    private GameObject emp_playingTextGo;
    private GameObject emp_pauseGo;
    private NormalModelPanel normalModelPanel;
    //按钮图片切换资源
    public Sprite[] btn_gameSpeedSprites;//0正常，1加速
    public Sprite[] btn_pauseSprites;//0正常1暂停
    //开关
    private bool isPause;
    private bool isNormalGameSpeed;

    private Animator fireAnimator;
    private void Awake()
    {
        normalModelPanel = transform.GetComponentInParent<NormalModelPanel>();
        text_coin = transform.Find("Txt_Coin").GetComponent<Text>();
        tex_roundCount = transform.Find("Emp_PlayingText").Find("Img_RoundCount").Find("Text_RoundCount").GetComponent<Text>();
        tex_TotalCount = transform.Find("Emp_PlayingText").Find("Text_TotalCount").GetComponent<Text>();
        img_Btn_GameSpeed = transform.Find("Btn_GameSpeed").GetComponent<Image>();
        img_Btn_Pause = transform.Find("Btn_Pause").GetComponent<Image>();
        emp_pauseGo = transform.Find("Emp_Pause").gameObject;
        emp_playingTextGo = transform.Find("Emp_PlayingText").gameObject;
        fireAnimator = transform.Find("Card").GetComponent<Animator>();
    }
    private void OnEnable()
    {
        isPause = false;
        isNormalGameSpeed = true;
        emp_pauseGo.SetActive(false);
        emp_playingTextGo.SetActive(true);
        tex_TotalCount.text = normalModelPanel.totalRound.ToString();
        //Debug.Log("存档关卡是"+ normalModelPanel.totalRound.ToString()+"--当前关卡是" + tex_TotalCount.text);
        img_Btn_Pause.sprite = btn_pauseSprites[0];
        img_Btn_GameSpeed.sprite = btn_gameSpeedSprites[0];
        //Invoke("CorrectWave",0.05f);
        UpdateCoinText();
        
        //UpdateRoundCount();
        //UpdateRoundCount();
    }
    private void CorrectWave()
    {
        if (normalModelPanel.totalRound.ToString() != tex_TotalCount.text.ToString())
        {
            tex_TotalCount.text = normalModelPanel.totalRound.ToString();
            //Debug.Log("矫正波数");
        }
    }
    public void UpdateCoinText()
    {
        //Debug.Log(text_coin);
        //Debug.Log(GameController.Instance.coin);
        text_coin.text = GameController.Instance.coin.ToString();
        CorrectWave();
    }
    public void UpdateRoundCount()
    {
        normalModelPanel.ShowRoundText(tex_roundCount);
    }
    //改变游戏暂停
    public void ChangeGamePause()
    {
        GameManager.Instance.audioSourceManager.PlayButtonAudioClip();
        isPause = !isPause;
        if (isPause)
        {//感觉这里用一个Gamecontroller里封装方法来调用会不会更好？——不行，Gamecontroller不是一直存在，而且会引发变量值固定的问题
            GameController.Instance.isPause = true;
            img_Btn_Pause.sprite = btn_pauseSprites[1];
            emp_pauseGo.SetActive(true);
            emp_playingTextGo.SetActive(false);
        }
        else
        {
            GameController.Instance.isPause = false;
            img_Btn_Pause.sprite = btn_pauseSprites[0];
            emp_pauseGo.SetActive(false);
            emp_playingTextGo.SetActive(true);
        }
    }
    //改变游戏速度
    public void ChangeGameSpeed()
    {
        GameManager.Instance.audioSourceManager.PlayButtonAudioClip();
        isNormalGameSpeed = !isNormalGameSpeed;
        if (isNormalGameSpeed)
        {
            GameController.Instance.gameSpeed = 1;
            img_Btn_GameSpeed.sprite = btn_gameSpeedSprites[0];
        }
        else
        {
            GameController.Instance.gameSpeed = 2;
            img_Btn_GameSpeed.sprite = btn_gameSpeedSprites[1];
        }
    }
    public void ShowMenuPage()
    {
        normalModelPanel.ShowMenuPage();
    }
    //天照相关
    public GameObject explain;
    
    public void ClickCard()
    {
        if (normalModelPanel.cardNum<=0)
        {
            explain.SetActive(true);
            return;
        }
        fireAnimator.Play("BiggerCard");
        GameController.Instance.PlayEffectClip("NormalMordel/6555");
        normalModelPanel.cardNum--;
        GameController.Instance.ShowTheFire();
    }
    public void CloseExplainPanel()
    {
        explain.SetActive(false);
    }
}
