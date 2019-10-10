using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SliderCanCoverScrollView : MonoBehaviour,IBeginDragHandler,IEndDragHandler {

    private float contentLenth;//容易长度
    private float beginMousePositionX;
    private float endMousePositionX;
    private ScrollRect scrollRect;
    private float lastProportion;//上一个位置比例

    public int cellLength;
    public int spacing;
    public int leftOffset;//左便宜
    private float upperLimit;
    private float lowerLimit;

    private float firstItemLength;//移动第一个单元格的距离
    private float oneItemLength;//滑动一个单元格需要的距离
    private float oneItemProportion;//滑动一个单元格需要的比例

    public int totalItemNum;
    private int currentItemIndex;

    public Text pageText;
    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        contentLenth = scrollRect.content.rect.xMax;//这个地方的坐标0位置正好就是屏幕最右侧，我就没减了
        //Debug.Log("修改前contentLenth :" + contentLenth + "--计算的结果：" + scrollRect.content.rect.xMax + "left" + 2 * leftOffset + "cell" + cellLength);
        firstItemLength = cellLength / 2 + leftOffset;
        oneItemLength = cellLength + spacing;
        oneItemProportion = oneItemLength / contentLenth;
        upperLimit = 1 - firstItemLength / contentLenth;
        lowerLimit = firstItemLength / contentLenth;
        currentItemIndex = 1;//其实我觉得这里用0比较好。。。
        scrollRect.horizontalNormalizedPosition = 0;
        //Debug.Log("contentLenth :" + contentLenth + "--计算的结果：" + scrollRect.content.rect.xMax + "left" + 2 * leftOffset + "cell" + cellLength);
        if (pageText != null)
        {
            pageText.text = currentItemIndex.ToString() + "/" + totalItemNum;
        }
    }
    public void Init()
    {
        currentItemIndex = 1;
        if (scrollRect!=null)
        {
            //scrollRect是在Awake方法里面赋值，但是Init方法在Awake之前执行，所以会报空，如果不为空，说明Awake方法已经执行，pageText也就可以执行了
            scrollRect.horizontalNormalizedPosition = 0;
            pageText.text = currentItemIndex.ToString() + "/" + totalItemNum;
        }
       
        lastProportion = 0;
        
    }
    public void OnEndDrag(PointerEventData eventData)
    {
       
        float offSet;
       
        endMousePositionX = Input.mousePosition.x;
        offSet = (beginMousePositionX - endMousePositionX)*2;
        //Debug.Log("endMousePsositionx :"+endMousePositionX + "offset :" +offSet);
        //Debug.Log("offSet" + offSet);
        //Debug.Log("firstItemLength" + firstItemLength);
        if (Mathf.Abs(offSet) > firstItemLength)//是不是距离达到了滑动的值
        {
            if (offSet > 0)//右滑动
            {
                if (currentItemIndex >= totalItemNum)
                {
                    return;
                }
                //算一下能翻几个，要加上第一个。
                int moveCount = (int)((offSet - firstItemLength) / oneItemLength) + 1;
                currentItemIndex += moveCount;
                if (currentItemIndex >= totalItemNum)//超出格子数量总数
                {
                    currentItemIndex = totalItemNum;
                }
                //当次需要移动的比例位置
                //Debug.Log("scrollRect.content.rect.xMax :" + scrollRect.content.rect.xMax + "--leftOffset :" + leftOffset + "--cellLength :" + 
                //    cellLength+"--contentLenth :"+(scrollRect.content.rect.xMax - 2 * leftOffset - cellLength));
                //Debug.Log("oneItemProportion :" + oneItemProportion+ "oneItemLength :" + oneItemLength+"contentLenth :"+contentLenth);
                //Debug.Log("修改前的lastProportion :" + lastProportion);
                lastProportion += oneItemProportion * moveCount;
                //Debug.Log("lastProportion :" + lastProportion + "-----moveCount :" + moveCount);
                //超出滚动范围
                if (lastProportion >= upperLimit)
                {
                    lastProportion = 1;
                }
            }
            else
            {
                if (currentItemIndex <= 1)
                {
                    return;
                }
                //算一下能翻几个，要加上第一个。
                int moveCount = (int)((offSet - firstItemLength) / oneItemLength) - 1;
                currentItemIndex += moveCount;
                if (currentItemIndex < 1)//超出格子数量总数
                {
                    currentItemIndex = 1;
                }
                //当次需要移动的比例位置
                lastProportion += oneItemProportion * moveCount;
                //超出滚动范围
                if (lastProportion <= lowerLimit)
                {
                    lastProportion = 0;
                }
            }
            if (pageText != null)
            {
                pageText.text = currentItemIndex.ToString() + "/" + totalItemNum;
            }
        }
        //这里用一个dotween 
        DOTween.To(()=>scrollRect.horizontalNormalizedPosition,lerpValue=>scrollRect.horizontalNormalizedPosition=lerpValue,lastProportion,0.5f).SetEase(Ease.InOutQuint);

        //下面的代码仅用于此项目，复用脚本需要删除
        GameManager.Instance.audioSourceManager.PlayPagingAudioClip();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        beginMousePositionX = Input.mousePosition.x;
        //Debug.Log(beginMousePositionX);
    }

    //public void DragOver()
    //{
    //    float offset;
    //    float endDragPos = Input.mousePosition.x;
    //    offset = endDragPos - beginMousePositionX;
    //    //是否达到了拖拽的最低距离
    //    if (Mathf.Abs(offset)>=oneItemLength)
    //    {
    //        //根据偏移量所占用的比例判断需要滑动几个格子
    //        int count = (int)(offset / (oneItemLength + spacing));
    //        //根据偏移量的正负判断拖拽的方向
    //        if (offset>0)
    //        {
    //            if (currentItemIndex>=totalItemNum)
    //            {
    //                return;
    //            }
    //            count++;
    //            currentItemIndex += count;
    //            if (currentItemIndex>=totalItemNum)
    //            {
    //                currentItemIndex = totalItemNum;
    //            }
    //        }
    //        else
    //        {
    //            if (currentItemIndex<=1)
    //            {
    //                return;
    //            }
    //            count--;
    //            currentItemIndex += count;
    //            if (currentItemIndex<=1)
    //            {
    //                currentItemIndex = 1;
    //            }
    //        }
    //        currentItemIndex += count;
    //        lastProportion += oneItemProportion * count;
    //        if (lastProportion>=upperLimit)
    //        {
    //            lastProportion = 1;
    //        }
    //        else if (lastProportion<=lowerLimit)
    //        {
    //            lastProportion = 0;
    //        }
    //    }

    //    //赋值给最终比例变量，并使用dotween来进行浮动处理
    //    DOTween.To(()=>scrollRect.horizontalNormalizedPosition,lerpValue=>scrollRect.horizontalNormalizedPosition=lerpValue,lastProportion,0.5f).SetEase(Ease.InOutQuint);
    //}
}
