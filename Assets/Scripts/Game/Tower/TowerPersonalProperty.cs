using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPersonalProperty : MonoBehaviour {
    [HideInInspector]
    public Tower tower;
    public Transform targetTrans;
    public int towerLevel;
    
    public int price;
    [HideInInspector]
    public int upLevelPrice;
    [HideInInspector]
    public int sellPrice;
    protected Animator animator;

    public float attackCD;
    protected float timeVal;//攻击计时器
    protected GameObject bulletGo;

    private GameController gameController;

    public void Init()
    {
        tower = null;
    }
	// Use this for initialization
	protected virtual void Start () {
        gameController = GameController.Instance;
        animator = transform.Find("tower").GetComponent<Animator>();
        timeVal = attackCD;
        upLevelPrice = (int)(price * 1.5f);
        sellPrice = price / 2;
	}
	
	// Update is called once per frame
	protected virtual void Update () {
        if (targetTrans==null||gameController.isPause||gameController.gameOver)
        {
            return;
        }
        if (!targetTrans.gameObject.activeSelf)//目标被推入对象池,其实感觉可以写成一行，和上面的合并
        {
            targetTrans = null;
            return;
        }
        if (timeVal>=(attackCD/gameController.gameSpeed))
        {
            Attack();
        }
        else
        {
            timeVal += Time.deltaTime;
        }
        if (targetTrans.tag=="Item")
        {
            transform.LookAt(targetTrans.position+new Vector3(0,0,3));
        }
        if (targetTrans.tag=="Monster")
        {
            transform.LookAt(targetTrans);
        }
        if (transform.eulerAngles.y==0)//因为2D游戏，图片产生了侧面偏转，在摄像机视野里面变成了一条线，要手动转过来
        {
            transform.eulerAngles +=new Vector3(0,90,0);
        }
	}
    public void SellTower()
    {
        gameController.PlayEffectClip("NormalMordel/Tower/TowerSell");
        gameController.ChangeCoin(sellPrice);
        GameObject effectGo = gameController.GetGameObjectResource("BuildEffect");//懒得改名字了
        effectGo.transform.position = transform.position;
        tower.DestroyTower();
    }
    //包含了摧毁方法
    public void UpLevelTower()
    {
        gameController.PlayEffectClip("NormalMordel/Tower/TowerUpdata");
        gameController.ChangeCoin(-upLevelPrice);
        GameObject effectGo = gameController.GetGameObjectResource("UpLevelEffect");//懒得改名字了
        effectGo.transform.position = transform.position;
        tower.DestroyTower();
    }
    protected virtual void Attack()
    {
        if (targetTrans==null)
        {
            return;
        }
        animator.Play("Attack");
        gameController.PlayEffectClip("NormalMordel/Tower/Attack/"+tower.towerID.ToString());
        bulletGo = gameController.GetGameObjectResource("Tower/ID"+tower.towerID+"/Bullect/"+towerLevel.ToString());
        bulletGo.transform.position = transform.position;
        //Debug.Log(bulletGo.transform.GetComponent<Bullet>());
        bulletGo.transform.GetComponent<Bullet>().targetTrans = targetTrans;
        timeVal = 0;
    }
}
