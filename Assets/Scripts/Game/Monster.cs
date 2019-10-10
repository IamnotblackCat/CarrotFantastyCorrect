using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour {
    //属性值
    public int monsterID;
    public int HP;
    public int currentHP;//血条显示需要两个值
    public float moveSpeed;
    public float initMoveSpeed;
    public int prize;//奖励金币
    
    //引用
    public RuntimeAnimatorController runtimeAnimatorController;
    private Animator animator;
    private GameObject TshitGo;//便便特效
    private Slider slider;
    //计数的属性或者开关
    private int roadPointIndex = 1;
    private bool reachCarrot;//到终点了没
    private bool hasDecreasSpeed;//是否减速状态

    private float decreaseTimeVal;//减速计时器
    private float decreaseTime;//减速时间
    //资源
    public AudioClip dieAudioClip;

    //封装长串代码
    //private GameController gameController;这里数据被固定了，导致后面不会反应真实的数据变动
    private List<Vector3> monsterPointList;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        slider = gameObject.transform.Find("MonsterCanvas").Find("HPSlider").GetComponent<Slider>();
        slider.gameObject.SetActive(false);//默认是不显示的，挨揍了才显示
        //gameController = GameController.Instance;
        monsterPointList = GameController.Instance.mapMaker.monsterPathPos;
        TshitGo = transform.Find("TshitEffect").gameObject;
    }
    private void OnEnable()
    {//怪物的偏转
        if (roadPointIndex + 1 < monsterPointList.Count)
        {
            float xOffset = monsterPointList[roadPointIndex].x - monsterPointList[roadPointIndex + 1].x;
            if (xOffset < 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (xOffset > 0)//正数说明现有的点在目标点右边
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
        slider.gameObject.transform.eulerAngles = Vector3.zero;
    }
    private void Update()
    {
        if (GameController.Instance.isPause||GameController.Instance.gameOver)
        {
            //Debug.Log(GameController.Instance.gameOver);
            return;
        }
        if (!reachCarrot)
        {
            //这里Lerp函数是抛物线移动的，为了线性移动，使用1/Vector3.Distance()，来把单次移动距离单位向量化
            transform.position = Vector3.Lerp(transform.position,monsterPointList[roadPointIndex],
                1/Vector3.Distance(transform.position, monsterPointList[roadPointIndex])*moveSpeed*Time.deltaTime*GameController.Instance.gameSpeed);
            if (Vector3.Distance(transform.position,monsterPointList[roadPointIndex])<=0.01f)
            {
                roadPointIndex++;
                if (roadPointIndex + 1 < monsterPointList.Count)
                {
                    float xOffset = monsterPointList[roadPointIndex].x - monsterPointList[roadPointIndex + 1].x;
                    if (xOffset < 0)
                    {
                        transform.eulerAngles = new Vector3(0, 0, 0);
                    }
                    else if (xOffset > 0)//正数说明现有的点在目标点右边
                    {
                        transform.eulerAngles = new Vector3(0, 180, 0);
                    }
                }
                slider.gameObject.transform.eulerAngles = Vector3.zero;
                if (roadPointIndex>=monsterPointList.Count)
                {
                    reachCarrot = true;
                }

            }
        }
        else//吃到萝卜了
        {
            DestroyMonster();
            GameController.Instance.DecreaseHP();
            //Debug.Log("你确定吃到了?");
        }
        if (hasDecreasSpeed)
        {
            if (decreaseTimeVal>=decreaseTime)
            {
                CancelDecreaseDebuff();
                decreaseTimeVal = 0;
            }
            else
            {
                decreaseTimeVal += Time.deltaTime;
            }
        }
    }
    private void DestroyMonster()
    {
        if (GameController.Instance.targetTrans == transform)
        {
            GameController.Instance.HideSignal();
        }
        if (!reachCarrot)//被玩家干掉的
        {
            GameController.Instance.killMonsterTotalNum++;//我个人觉得这里被玩家杀的才计入总数，自己吃了萝卜的不算
            //生成金币画布
            GameObject coinGO = GameController.Instance.GetGameObjectResource("CoinCanvas");
            coinGO.transform.Find("Emp_Coin").GetComponent<CoinMove>().prize = prize;
            coinGO.transform.SetParent(GameController.Instance.transform);
            coinGO.transform.position = transform.position;
           
            //增加玩家金币
            GameController.Instance.ChangeCoin(prize);
            //随机掉落奖励物品
            int randomNum = Random.Range(0,100);
            if (randomNum<10)
            {
                GameObject prizeGO = GameController.Instance.GetGameObjectResource("Prize");
                prizeGO.transform.position = transform.position-new Vector3(0,0,6);
                GameController.Instance.PlayEffectClip("NormalMordel/GiftCreate");
            }
        }
        //产生销毁特效
        GameObject effectGo = GameController.Instance.GetGameObjectResource("DestoryEffect");
        effectGo.transform.position = transform.position;
        effectGo.transform.SetParent(GameController.Instance.transform);

        GameController.Instance.killMonsterNum++;
        InitMonsterGo();
        GameController.Instance.PushGameObjectToFactory("MonsterPrefab",gameObject);
    }
    //初始化怪物
    private void InitMonsterGo()
    {
        monsterID = 0;
        HP = 0;
        currentHP = 0;
        moveSpeed = 0;
        roadPointIndex = 1;
        dieAudioClip = null;
        reachCarrot = false;
        slider.value = 1;
        slider.gameObject.SetActive(false);
        prize = 0;
        transform.eulerAngles = Vector3.zero;
        decreaseTime = 0;
        decreaseTimeVal = 0;
        CancelDecreaseDebuff();
    }

    //减速方法
    private void DecreaseSpeed(BulletProperty bulletProperty)
    {
        TshitGo.SetActive(true);
        decreaseTimeVal = 0;//每次收到便便攻击减速时间要重置
        decreaseTime = bulletProperty.debuffTime;
        if (hasDecreasSpeed)//防止速度被降到0
        {
            return;
        }
        moveSpeed = moveSpeed - bulletProperty.debuffValue;
        hasDecreasSpeed = true;
    }
    //用来取消减速的方法,为什么是使用计时器，并且提供一个去掉debuff的方法呢？因为游戏存在暂停，如果使用延时调用会很麻烦
    private void CancelDecreaseDebuff()
    {
        TshitGo.SetActive(false);
        moveSpeed = initMoveSpeed;
        hasDecreasSpeed = false;
    }
    //受伤
    private void TakeDamage(int attackValue)
    {
        slider.gameObject.SetActive(true);
        currentHP -= attackValue;
        if (currentHP<=0)
        {
            //播放死亡音效
            GameController.Instance.PlayEffectClip("NormalMordel/Monster/"+GameController.Instance.currentStage.mBigLevelID.ToString()+"/"+monsterID.ToString());
            DestroyMonster();
            return;
        }
        slider.value = (float)currentHP / HP;
    }
    public void GetMonsterProperty()
    {
        runtimeAnimatorController = GameController.Instance.controllers[monsterID-1];//索引是从0开始的，id是从1开始的
        animator.runtimeAnimatorController=runtimeAnimatorController;
    }
    //被集火
    private void OnMouseDown()
    {
        if (GameController.Instance.targetTrans==null)
        {
            GameController.Instance.targetTrans = transform;
            GameController.Instance.ShowSignal();
        }
        else if (GameController.Instance.targetTrans!=transform)//更换目标
        {
            GameController.Instance.HideSignal();
            GameController.Instance.targetTrans = transform;
            GameController.Instance.ShowSignal();
        }
        else if (GameController.Instance.targetTrans==transform)//点的同一个
        {
            GameController.Instance.HideSignal();
        }
    }
}
