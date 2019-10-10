using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindMillBullet : Bullet {

    private bool hasTarget;//有目标吗？
    private float timeVal;//计时器，时间到了就消失
    private float timeDisappear=2.5f;
    private void OnEnable()
    {
        hasTarget = false;
        timeVal = 0;
    }

    private void InitTarget()//这里放在Init里面而不是放在Update里面，因为这个子弹是方向性的，不会去追踪
    {
        if (targetTrans.tag == "Item")
        {
            transform.LookAt(targetTrans.position + new Vector3(0, 0, 3));//物体的Z坐标离摄像机近三个单位
        }
        else
        {
            //Debug.Log(targetTrans.position);
            transform.LookAt(targetTrans.position);
        }
        if (transform.eulerAngles.y==0)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90, transform.eulerAngles.z);
        }
    }
    protected override void Update()
    {
        if (GameController.Instance.gameOver||timeVal>=timeDisappear)
        {
            //Debug.Log("销毁");
            DestroyBullet();
        }
        if (GameController.Instance.isPause)
        {
            return;//不走下面的逻辑
        }
        if (timeVal<timeDisappear)
        {
            timeVal += Time.deltaTime;
        }
        if (hasTarget)
        {
            transform.Translate(transform.forward*moveSpeed*Time.deltaTime*GameController.Instance.gameSpeed,Space.World);
        }
        else
        {
            if (targetTrans != null && targetTrans.gameObject.activeSelf)//这里不能先判断gameObjectactiveSelf，因为它可能为空，无法判断
            {
                hasTarget = true;//因为这个目标前面没有主动赋值，所以在这里检测到就赋值
                InitTarget();
            }
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //先用一个广义的范围包住，只攻击怪物和物品
        if (collision.tag == "Monster" || collision.tag == "Item")
        {//并且物体还存在
            if (collision.gameObject.activeSelf)
            {
                //然后把不攻击的特殊情况排除掉:目标位置为空，或者碰撞体是物品，但是集火目标为空，不攻击物品
                if (targetTrans.position == null || (collision.tag == "Item" && GameController.Instance.targetTrans == null))
                {
                    return;
                }
                //两种情况会攻击：是怪物，或者是被集火的物体
                if (collision.tag == "Monster" || (collision.tag == "Item" && GameController.Instance.targetTrans == collision.transform))
                {
                    collision.SendMessage("TakeDamage", attackDamage);
                    CreateEffect();
                }
            }
        }
    }
}
