using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SliderScrowView : MonoBehaviour,IBeginDragHandler,IEndDragHandler {

    private ScrollRect scrollRect;
    private RectTransform contentTrans;
    private float beginDragPosX;
    private float endDragPosX;
    private float moveOneItemLength;
    public int totalNum;

    public bool needSendMessage;
    public Text pageText;

    //这个是上一次的位置，已经滑到的位置。
    private Vector3 currentContentLastLocalPos;
    //为了存储初始的位置值，声明一个变量来记录,这个变量的命名也有问题，应该是初始位置
    private Vector3 contentInitPos;
    private Vector2 contentInitTransSize;//content的trans值初始大小

    private int currentItemIndex;

    public int cellLength;
    public int spacing;
    public int leftOffset;

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        contentTrans = scrollRect.content;
        moveOneItemLength = cellLength + spacing;
        currentItemIndex = 1;
        currentContentLastLocalPos = contentTrans.localPosition;
        contentInitPos = contentTrans.localPosition;
        contentInitTransSize = contentTrans.sizeDelta;

        if (pageText!=null)
        {
            pageText.text = currentItemIndex.ToString() + "/" + totalNum;
        }
    }
    public void Init()
    {
        currentItemIndex = 1;
        currentContentLastLocalPos = contentInitPos;//把上一个滑动的位置也归为初始
        if (contentTrans!=null)
        {

            contentTrans.localPosition = contentInitPos;//把位置复原初始
        }
        if (pageText != null)
        {
            pageText.text = currentItemIndex.ToString() + "/" + totalNum;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        endDragPosX = Input.mousePosition.x;
        float offSetX = beginDragPosX - endDragPosX;
        float moveDistance = 0;
        if (offSetX>0)//右滑
        {
            if (currentItemIndex>=totalNum)
            {
                return;
            }
            if (needSendMessage)
            {
                UpdatePanel(true);
                //Debug.Log("滑动");
            }
            moveDistance = -moveOneItemLength;
            currentItemIndex++;
        }
        else//左滑
        {
            if (currentItemIndex <=1)
            {
                return;
            }
            if (needSendMessage)
            {
                UpdatePanel(false);
            }
            moveDistance = moveOneItemLength;
            currentItemIndex--;
        }
        if (pageText != null)
        {
            pageText.text = currentItemIndex.ToString() + "/" + totalNum;
        }
        //就觉得很奇怪，明明没有调整content的pos位置，这里的函数体赋值错误
        DOTween.To(()=> contentTrans.localPosition, lerpValue=>contentTrans.localPosition=lerpValue,currentContentLastLocalPos+new Vector3(moveDistance,0,0),0.5f).SetEase(Ease.InOutQuint);
        //虽然content的实际位置已经改变了，但是这个变量之前赋值的，需要手动调整。
        //这个变量的命名有问题，实际上这个变量是为了记录上一个位置，方便之后的调整用的
        currentContentLastLocalPos += new Vector3(moveDistance,0,0);
        //下面的代码仅用于此项目，复用脚本需要删除
        GameManager.Instance.audioSourceManager.PlayPagingAudioClip();
        //Debug.Log(currentContentLocalPos+"index :"+currentItemIndex);
    }

    //按钮滑动
    public void ToNextPage()
    {
        float moveDistance = 0;
        if (currentItemIndex>=totalNum)//最后一页了
        {
            return;
        }
        moveDistance = -moveOneItemLength;//因为画布左右坐标是反的
        currentItemIndex++;
        if (pageText!=null)
        {
            pageText.text = currentItemIndex.ToString() + "/" + totalNum;
        }
        if (needSendMessage)
        {
            UpdatePanel(true);
        }
        DOTween.To(()=>contentTrans.localPosition,lerpValue=> contentTrans.localPosition = lerpValue,currentContentLastLocalPos+new Vector3(moveDistance,0,0),0.5f);
        currentContentLastLocalPos += new Vector3(moveDistance,0,0);
    }
    public void ToLastPage()
    {
        float moveDistance = 0;
        if (currentItemIndex <=1)//最后一页了
        {
            return;
        }
        moveDistance = moveOneItemLength;//因为画布左右坐标是反的
        currentItemIndex--;
        if (pageText != null)
        {
            pageText.text = currentItemIndex.ToString() + "/" + totalNum;
        }
        if (needSendMessage)
        {
            UpdatePanel(false);
        }
        DOTween.To(() => contentTrans.localPosition, lerpValue => contentTrans.localPosition = lerpValue, currentContentLastLocalPos + new Vector3(moveDistance, 0, 0), 0.5f);
        currentContentLastLocalPos += new Vector3(moveDistance, 0, 0);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        beginDragPosX = Input.mousePosition.x;
    }
    /// <summary>
    /// 调整content的大小
    /// </summary>
    /// <param name="itemNum"></param>
    public void SetContentLength(int itemNum)
    {
        //Debug.Log(contentTrans.sizeDelta);
        contentTrans.sizeDelta = new Vector2(contentTrans.sizeDelta.x+(cellLength+spacing)*(itemNum-1),contentTrans.sizeDelta.y);
        totalNum = itemNum;//这个脚本是复用的，每一个地方的totalNum是不同的，需要重新赋值
    }
    //初始化content的大小
    public void InitContentLength()
    {
        contentTrans.sizeDelta = contentInitTransSize;

    }
    /// <summary>
    /// SliderScrow的发送翻页信息的方法,toNext代表的是0和1，true代表1，往右边翻页，false代表0往左边翻页
    /// </summary>
    public void UpdatePanel(bool toNext)
    {
        if (toNext)
        {
            gameObject.SendMessageUpwards("ToNextLevel");
            //Debug.Log("发消息");
        }
        else
        {
            gameObject.SendMessageUpwards("ToLastLevel");
        }
    }
}
