using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameNormalBigLevelPanel : BasePanel {

    public Transform bigLevelContentTrans;
    public int bigLevelPageCount;
    private SliderScrowView sliderScrowView;
    private PlayerManager playerManager;
    private Transform[] bigLevelPage;

    private bool hasRegisterEvent;

    //public Text xiale;
    protected override void Awake()
    {
        base.Awake();
        sliderScrowView = gameObject.transform.Find("Scroll View").GetComponent<SliderScrowView>();
        playerManager = mUIFacade.mPlayerManager;
        bigLevelPage = new Transform[bigLevelPageCount];
        //显示大关卡信息
        for (int i = 0; i < bigLevelPageCount; i++)
        {
            bigLevelPage[i] = bigLevelContentTrans.GetChild(i);
            ShowBigLevelState(i+1,5,playerManager.unlockedNormalModelLevelNum[i],playerManager.unLockedNormalModelBigLevelList[i],bigLevelPage[i]);
        }
        hasRegisterEvent = true;
    }
    //每次进入都要更新
    public void OnEnable()
    {
        for (int i = 0; i < bigLevelPageCount; i++)
        {
            bigLevelPage[i] = bigLevelContentTrans.GetChild(i);
            ShowBigLevelState(i + 1, 5, playerManager.unlockedNormalModelLevelNum[i], playerManager.unLockedNormalModelBigLevelList[i], bigLevelPage[i]);
            
        }
        //Debug.Log(bigLevelPage[0].Find("Img_Page").Find("Txt_Page").GetComponent<Text>().text+ bigLevelPage[0].ToString());
        //Debug.Log(xiale.text);
    }
    //面板的进入退出
    public override void EnterPanel()
    {
        base.EnterPanel();
        sliderScrowView.Init();
        gameObject.SetActive(true);
        //sliderScrowView.SetContentLength(3);
    }
    public override void ExitPanel()
    {
        base.ExitPanel();
        gameObject.SetActive(false);
    }
    //封装一个方法来更新大关卡的信息
    public void ShowBigLevelState(int bigLevelID,int totalNum,int unlockedLevelNum, bool unLocked,Transform theBigLevelButtonTrans)
    {
        if (unLocked)//已解锁
        {
            theBigLevelButtonTrans.Find("Img_Page").gameObject.SetActive(true);
            theBigLevelButtonTrans.Find("Img_Lock").gameObject.SetActive(false);
            //Debug.Log(theBigLevelButtonTrans);
            //Debug.Log(theBigLevelButtonTrans.Find("Img_Page").Find("Txt_Page"));
            theBigLevelButtonTrans.Find("Img_Page").Find("Txt_Page").GetComponent<Text>().text=
                unlockedLevelNum.ToString()+"/"+totalNum.ToString();
            //Debug.Log("theBigLevel"+ theBigLevelButtonTrans.ToString()+"/"+ theBigLevelButtonTrans.Find("Img_Page") +"/"+ theBigLevelButtonTrans.Find("Img_Page") .Find("Txt_Page")+ theBigLevelButtonTrans.Find("Img_Page").Find("Txt_Page").GetComponent<Text>().text.ToString()+"--"+unlockedLevelNum.ToString() + "/" + totalNum.ToString());
            Button theBigLevelButtonCom=
            theBigLevelButtonTrans.GetComponent<Button>();
            theBigLevelButtonCom.interactable = true;
            //添加监听器这个动作是会重复进行的，多次进行以后可能导致程序崩溃，所以加一个bool值判断，当已经注册以后，不再执行。
            if (!hasRegisterEvent)
            {
                theBigLevelButtonCom.onClick.AddListener(() =>
                {//离开大关卡，进入小关卡
                    mUIFacade.PlayButtonAduioClip();
                    mUIFacade.currentScenePanelDict[StringManager.GameNormalBigLevelPanel].ExitPanel();
                    GameNormalLevelPanel gameNormalLevelPanel = mUIFacade.currentScenePanelDict[StringManager.GameNormalLevelPanel] as GameNormalLevelPanel;
                    gameNormalLevelPanel.ToThisPanel(bigLevelID);
                    GameNormalOptionPanel gameNormalOptionPanel = mUIFacade.currentScenePanelDict[StringManager.GameNormalOptionPanel] as GameNormalOptionPanel;
                    gameNormalOptionPanel.isInBigLevel = false;//不在大关卡的panel里面了
                });
            }
        }
        else//未解锁
        {
            theBigLevelButtonTrans.Find("Img_Page").gameObject.SetActive(false);//书签隐藏
            theBigLevelButtonTrans.Find("Img_Lock").gameObject.SetActive(true);//锁显示
            theBigLevelButtonTrans.GetComponent<Button>().interactable = false;//button按钮设置为不可交互
        }
       
    }
    //左右按键滑动的方法
    public void ToNextBigLevel()
    {
        mUIFacade.PlayButtonAduioClip();
        sliderScrowView.ToNextPage();
    }
    public void ToLastBigLevel()
    {
        mUIFacade.PlayButtonAduioClip();
        sliderScrowView.ToLastPage();
    }
}
