using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public Transform targetTrans;
    public float moveSpeed;
    public int attackDamage;
    public int towerID;
    public int towerLevel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	protected virtual void Update () {
        if (GameController.Instance.gameOver)
        {
            DestroyBullet();
        }
        if (GameController.Instance.isPause)
        {
            return;
        }
        if (targetTrans==null||!targetTrans.gameObject.activeSelf)//攻击目标为空或者失活
        {
            DestroyBullet();
            return;
        }
        //把移动距离单位化再乘以速度
        transform.position = Vector3.Lerp(transform.position,targetTrans.position,
            1/Vector3.Distance(transform.position,targetTrans.position)*Time.deltaTime*moveSpeed*GameController.Instance.gameSpeed*1.5f);
        //朝向
        if (targetTrans.tag=="Item")
        {
            transform.LookAt(targetTrans.position+new Vector3(0,0,3));
        }
        else if (targetTrans.tag=="Monster")
        {
            //Debug.Log(targetTrans.position);
            transform.LookAt(targetTrans);
        }
        if (transform.eulerAngles.y==0)//因为2D游戏，图片产生了侧面偏转，在摄像机视野里面变成了一条线，要手动转过来
        {
            transform.eulerAngles += new Vector3(0, 90, 0);
        }
	}


    protected virtual void DestroyBullet()
    {
        targetTrans = null;
        GameController.Instance.PushGameObjectToFactory("Tower/ID"+towerID+"/Bullect/"+towerLevel,gameObject);
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //先用一个广义的范围包住，只攻击怪物和物品
        if (collision.tag=="Monster"||collision.tag=="Item")
        {//并且物体还存在
            if (collision.gameObject.activeSelf)
            {
                //然后把不攻击的特殊情况排除掉:目标位置为空，或者碰撞体是物品，但是集火目标为空，不攻击物品
                if (targetTrans.position==null||(collision.tag=="Item"&&GameController.Instance.targetTrans==null))
                {
                    return; 
                }
                //两种情况会攻击：是怪物，或者是被集火的物体
                if (collision.tag=="Monster"||(collision.tag=="Item"&&GameController.Instance.targetTrans==collision.transform))
                {
                    collision.SendMessage("TakeDamage", attackDamage);
                    CreateEffect();
                    DestroyBullet();
                }
            }
        }
    }

    protected virtual void CreateEffect()
    {
        GameObject EffectGo = GameController.Instance.GetGameObjectResource("Tower/ID" + towerID + "/Effect/" + towerLevel.ToString());
        EffectGo.transform.position = transform.position;
    }
}
