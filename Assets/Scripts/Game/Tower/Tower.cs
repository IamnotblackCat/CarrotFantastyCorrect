using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    public int towerID;
    private CircleCollider2D circleCollider2D;//真正的攻击范围
    private TowerPersonalProperty towerPersonalProperty;
    private SpriteRenderer attackRangeSr;//显示给玩家看的攻击范围
    public bool isTarget;//是否赋值给塔集火（因为可能不再范围内）
    public bool hasTarget;//有没有攻击目标

    private void OnEnable()
    {
        Init();
    }
    // Use this for initialization
    void Start () {
        //Init();
	}
	
	// Update is called once per frame
	void Update () {
        if (GameController.Instance.isPause||GameController.Instance.gameOver)
        {
            return;
        }
        //玩家可能随时取消掉集火目标，每帧判断当前塔的集火目标是否是游戏的激活目标
        if (isTarget&&towerPersonalProperty.targetTrans!=GameController.Instance.targetTrans)
        {
            towerPersonalProperty.targetTrans = null;
            isTarget = false;
            hasTarget = false;
        }
        //每一帧都需要判断当前是否有目标，并且目标是否存活，因为多个塔同时攻击，可能第一个塔已经把目标打死了，后面的还在打。
        if (hasTarget&&!towerPersonalProperty.targetTrans.gameObject.activeSelf)
        {
            towerPersonalProperty.targetTrans = null;
            isTarget = false;
            hasTarget = false;
        }
	}
    public void Init()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        towerPersonalProperty = GetComponent<TowerPersonalProperty>();
        towerPersonalProperty.tower = this;
        attackRangeSr = transform.Find("attackRange").GetComponent<SpriteRenderer>();
        attackRangeSr.gameObject.SetActive(false);
        circleCollider2D.radius = 1.1f*towerPersonalProperty.towerLevel;
        attackRangeSr.transform.localScale = new Vector3(towerPersonalProperty.towerLevel,towerPersonalProperty.towerLevel,1);
        isTarget = false;
        hasTarget = false;
    }
    public void GetTowerProperty()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SearchAttackObject(collision);
        //Debug.Log("进来了");
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        SearchAttackObject(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("说了离开了："+towerPersonalProperty.targetTrans+"碰撞的是："+collision.transform);
        if (towerPersonalProperty.targetTrans==collision.transform)
        {
            //Debug.Log("离开了");
            towerPersonalProperty.targetTrans = null;
            hasTarget = false;
            isTarget = false;
        }
    }
    //搜寻攻击对象的逻辑
    private void SearchAttackObject(Collider2D collision)
    {
        //先排除掉不攻击的情况，不是怪物也不是物体，就不攻击（也许未来会加NPC呢）,或者攻击范围内有集火目标了
        if ((collision.tag != "Monster" && collision.tag != "Item") || isTarget == true)
        {
            return;
        }
        Transform targetTransform = GameController.Instance.targetTrans;
        if (targetTransform != null)//有集火目标
        {
            if (!isTarget)//但是尚未赋值给塔集火，三种情况：怪物or物品？ 物品-集火-攻击；怪物-集火-攻击/不集火-没有攻击目标-攻击
            {
                if (collision.tag == "Item" && targetTransform == collision.transform)
                {
                    towerPersonalProperty.targetTrans = collision.transform;
                    isTarget = true;//赋值集火
                    hasTarget = true;
                }
                else if (collision.tag == "Monster" && targetTransform == collision.transform)//进来一个怪，并且是集火目标
                {
                    towerPersonalProperty.targetTrans = collision.transform;
                    isTarget = true;//赋值集火
                    hasTarget = true;
                }
                else if (collision.tag == "Monster" && !hasTarget)//不是集火目标，并且没有攻击目标，并且进来一个怪
                {
                    towerPersonalProperty.targetTrans = collision.transform;
                    hasTarget = true;
                }
            }
        }
        else//没有集火目标
        {
            if (!hasTarget && collision.tag == "Monster")//没有攻击目标且进来的是怪物
            {
                towerPersonalProperty.targetTrans = collision.transform;
                hasTarget = true;

            }
        }
    }
    public void DestroyTower()
    {
        Init();
        towerPersonalProperty.Init();
        GameController.Instance.PushGameObjectToFactory("Tower/ID"+towerID+"/TowerSet/"+towerPersonalProperty.towerLevel,gameObject);
    }
    
}
