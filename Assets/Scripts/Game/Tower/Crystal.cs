using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//水晶塔，子弹是电
public class Crystal : TowerPersonalProperty {

    private float distance;
    private float bulletWidth;
    private float bulletLength;
    private AudioSource audioSource;

    private void OnEnable()//每次唤醒都实例化子弹
    {
        if (animator==null)//父类的start里面赋值的，这里避免在start之前运行
        {
            return;
        }
        bulletGo = GameController.Instance.GetGameObjectResource("Tower/ID" + tower.towerID.ToString() + "/Bullect/" + towerLevel.ToString());
        bulletGo.SetActive(false);
    }
    // Use this for initialization
    protected override void Start () {
        base.Start();
        bulletGo =GameController.Instance.GetGameObjectResource("Tower/ID"+tower.towerID.ToString()+"/Bullect/"+towerLevel.ToString());
        bulletGo.SetActive(false);//子弹是一直显示的，直到目标死亡就隐藏
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = GameController.Instance.GetAudioClip("NormalMordel/Tower/Attack/" + tower.towerID.ToString());
	}
	
	// Update is called once per frame
	protected override void Update () {
        if (GameController.Instance.isPause||GameController.Instance.gameOver)
        {
            if (targetTrans==null)
            {
                bulletGo.SetActive(false);
            }
            return;
        }
        Attack();
	}
    protected override void Attack()
    {
        if (targetTrans==null)
        {
            return;
        }
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        //GameController.Instance.PlayEffectClip("NormalMordel/Tower/Attack"+tower.towerID.ToString());
        if (targetTrans.tag=="Item")
        {
            distance = Vector3.Distance(transform.position,targetTrans.position+new Vector3(0,0,3));
        }
        if (targetTrans.tag=="Monster")
        {
            distance = Vector3.Distance(transform.position,targetTrans.position);
        }
        bulletWidth = 3 / distance;//三个等级
        bulletLength = distance / 2 - distance * 0.1f;
        if (bulletWidth<=0.5f)
        {
            bulletWidth = 0.5f;
        }
        else if (bulletWidth>=1)
        {
            bulletWidth = 1;
        }
        bulletGo.transform.position = new Vector3((targetTrans.position.x + transform.position.x) / 2, (targetTrans.position.y + transform.position.y) / 2, 0);
        //bulletGo.transform.position = new Vector3((targetTrans.position.x+targetTrans.position.x)/2, (targetTrans.position.y +targetTrans.position.y) / 2,0);
        bulletGo.transform.localScale = new Vector3(1,bulletWidth,bulletLength);
        bulletGo.SetActive(true);
        bulletGo.GetComponent<Bullet>().targetTrans = targetTrans;
    }
    private void DestroyCrystal()
    {
        bulletGo.SetActive(false);
        GameController.Instance.PushGameObjectToFactory("Tower/ID" + tower.towerID.ToString() + "/Bullect/" + towerLevel.ToString(), bulletGo);
        bulletGo = null;
        tower.DestroyTower();
    }
}
