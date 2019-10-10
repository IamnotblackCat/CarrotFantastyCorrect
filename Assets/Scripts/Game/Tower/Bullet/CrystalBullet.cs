using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CrystalBullet : Bullet
{
    private float attackTimeVal;//实际扣血的计时器
    private bool canTakeDamage;

    protected override void Update()//这里把父类的代码拷过来，去掉了一个移动距离单位话除以速度
    {
        if (GameController.Instance.gameOver)
        {
            DestroyBullet();
        }
        if (GameController.Instance.isPause)
        {
            return;
        }
        if (targetTrans == null || !targetTrans.gameObject.activeSelf)//攻击目标为空或者失活
        {
            DestroyBullet();
            return;
        }
        
        //朝向
        if (targetTrans.tag == "Item")
        {
            transform.LookAt(targetTrans.position + new Vector3(0, 0, 3));
        }
        else if (targetTrans.tag == "Monster")
        {
            //Debug.Log(targetTrans.position);
            transform.LookAt(targetTrans);
        }
        if (transform.eulerAngles.y == 0)
        {
            transform.eulerAngles += new Vector3(0, 90, 0);
        }
        if (!canTakeDamage)
        {
            attackTimeVal += Time.deltaTime;
            
                if (attackTimeVal >= 0.5f - 0.125 * towerLevel)
                {
                    canTakeDamage = true;
                    attackTimeVal = 0;
                    DecreaseHP();
                }
            
        }
    }
    private void DecreaseHP()
    {
        if (!canTakeDamage||targetTrans==null)
        {
            return;
        }
        if (targetTrans.gameObject.activeSelf)
        {
            if (targetTrans.tag == "Item" && GameController.Instance.targetTrans == null)
            {
                return;
            }
            if (targetTrans.tag == "Monster" || (GameController.Instance.targetTrans.tag == "Item" && targetTrans.tag == "Item"))
            {
                targetTrans.SendMessage("TakeDamage", attackDamage);
                CreateEffect();
                canTakeDamage = false;//造成一次伤害了就改为false，等待下一次读秒
            }
        }
        
    }
    protected override void CreateEffect()
    {
        if (targetTrans.position==null)
        {
            return;
        }
        GameObject effectGo = GameController.Instance.GetGameObjectResource("Tower/ID"+towerID.ToString()
            +"/Effect/"+towerLevel.ToString());
        effectGo.transform.position = targetTrans.position;
    }
}
