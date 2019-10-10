using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class GridPoint : MonoBehaviour {
    //属性
    private SpriteRenderer spriteRenderer;
    public GridState gridState;
    public GridIndex gridIndex;
    public bool hasTower;

    private Sprite gridSprite;//格子图片资源
    private Sprite startSprite;//开场显示给玩家看的格子图片
    private Sprite cantBuildSprite;//禁止建造的图片

    private GameController gameController;
    private GameObject towerListGo;
    private GameObject handleTowerListGo;//注意这块画布，其实赋值就是GameController里面的画布，没必要存在的

    private Transform upLevelButtonTrans;
    private Transform sellTowerButtonTrans;
    private Vector3 uplevelBttonInitPos;
    private Vector3 sellTowerButtonInitPos;
    //有塔之后的属性
    public GameObject towerGo;
    public Tower tower;
    public TowerPersonalProperty towerPersonalProperty;
    //public int towerID;
    public int towerLevel;
    private GameObject levelUpSignalGo;//可升级提示
#if Tool
    private Sprite monsterSprite;//怪物路径点图片资源,当这个是怪物路径点的时候显示一个怪物的图片给予开发者提示
    private GameObject[] itemPrefabs;//建筑物数组
    public GameObject currenItem;//当前格子持有道具
#endif
    //格子状态
    public struct GridState
    {
        public bool canBuild;
        public bool isMonsterPoint;
        public bool hasItem;
        public int itemID;//预制体ID
    }

    //格子的索引
    public struct GridIndex
    {
        public int xIndex;
        public int yIndex;

    }
    private void Awake()
    {
#if Tool
        gridSprite = Resources.Load<Sprite>("Pictures/NormalMordel/Game/Grid");
        monsterSprite = Resources.Load<Sprite>("Pictures/NormalMordel/Game/1/Monster/6-1");
        itemPrefabs = new GameObject[10];

        string prefabsPath = "Prefabs/Game/" + MapMaker.Instance.bigLevelID.ToString()+"/Item/";
        for (int i = 0; i < itemPrefabs.Length; i++)
        {
            itemPrefabs[i] = Resources.Load<GameObject>(prefabsPath+i);
            if (!itemPrefabs[i])
            {
                Debug.Log("加载失败，失败路径： "+prefabsPath);
            }
        }
#endif
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        InitGrid();
#if Game
        gameController = GameController.Instance;
        gridSprite = gameController.GetSprite("NormalMordel/Game/Grid");
        startSprite = gameController.GetSprite("NormalMordel/Game/StartSprite");
        cantBuildSprite = gameController.GetSprite("NormalMordel/Game/cantBuild");
        spriteRenderer.sprite = startSprite;
        //这里alpha值是否调为0没多大影响，因为回调函数开始就把渲染器关掉了
        Tween tween = DOTween.To(()=>spriteRenderer.color,toColor=>spriteRenderer.color=toColor,new Color(1,1,1,0.2f),3);
        tween.OnComplete(ChangeSpriteToGrid);
        towerListGo = gameController.towerListGo;
        handleTowerListGo = gameController.handleTowerCanvasGo;
        upLevelButtonTrans = handleTowerListGo.transform.Find("Btn_UpLevel");
        sellTowerButtonTrans = handleTowerListGo.transform.Find("Btn_SellTower");
        uplevelBttonInitPos = upLevelButtonTrans.localPosition;
        sellTowerButtonInitPos = sellTowerButtonTrans.localPosition;
        levelUpSignalGo = transform.Find("LevelUPSignal").gameObject;
#endif
    }
    private void Update()
    {
        if (levelUpSignalGo!=null)//安全校验
        {
            if (hasTower)
            {
                if (towerLevel < 3 && towerPersonalProperty.upLevelPrice <= gameController.coin)
                {
                    levelUpSignalGo.SetActive(true);
                }
                else
                {
                    levelUpSignalGo.SetActive(false);
                }
            }
            else
            {//这里老师加了一个判断，就是如果提示还是亮的，就设置为false，但是不理解
                //这里已经是在每帧判断是否有塔了，卖掉塔的时候，就已经没有塔了，不是直接就调用了这一句代码吗？
                levelUpSignalGo.SetActive(false);
            }
        }
        
    }
    //把格子的显示图片转回去
    private void ChangeSpriteToGrid()
    {
        spriteRenderer.enabled = false;
        spriteRenderer.color = new Color(1,1,1,1);
        if (gridState.canBuild)
        {
            spriteRenderer.sprite = gridSprite;
        }
        else
        {
            spriteRenderer.sprite = cantBuildSprite;
        }
    }
    public void InitGrid()
    {
        gridState.canBuild = true;
        gridState.hasItem = false;
        gridState.isMonsterPoint = false;
        gridState.itemID = -1;
        spriteRenderer.enabled = true;
#if Tool
        spriteRenderer.sprite = gridSprite;
        Destroy(currenItem);
#endif

#if Game
        towerGo = null;
        hasTower = false;
        towerPersonalProperty = null;
#endif
    }
#if Game
    public void UpdateGrid()
    {
        if (gridState.canBuild)
        {
            spriteRenderer.enabled = true;
            if (gridState.hasItem)//如果这一关的这个格子是设计的有物体的，就实例化
            {
                CreateItem();
            }
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }
    public void CreateItem()
    {
        GameObject itemGo = GameController.Instance.GetGameObjectResource(GameController.Instance.mapMaker.bigLevelID.ToString()+"/Item/"+gridState.itemID);
        itemGo.transform.SetParent(GameController.Instance.transform);
        //其实我觉得这个变量的赋值应该在Item里面完成，Item脚本的变量在外面赋值，逻辑很难理解，找起来也很麻烦
        itemGo.GetComponent<Item>().itemID = gridState.itemID;
        //为了让物体更靠近摄像机，不要被遮挡
        Vector3 createPos =  transform.position - new Vector3(0,0,3);
        if (gridState.itemID<=2)//判断这个物体是占了几个格子，《=2是四个，《=4是2个，其他1个
        {
            createPos += new Vector3(GameController.Instance.mapMaker.gridWidth,-GameController.Instance.mapMaker.gridHeight)/2;
        }
        else if(gridState.itemID<=4)
        {
            createPos += new Vector3(GameController.Instance.mapMaker.gridWidth,0)/2;
        }
        itemGo.transform.position = createPos;
        itemGo.GetComponent<Item>().gridPoint = this;
    }
    /// <summary>
    /// 有关格子的处理方法
    /// </summary>
    //public void AfterBuild()
    //{
    //    spriteRenderer.enabled = false;
    //}
    private void OnMouseDown()
    {
        //这个方法是判断鼠标点击到的是不是UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            //Debug.Log("UI");
            return;
        }
        gameController.HandleGrid(this);
    }
    public void ShowGrid()
    {
       // Debug.Log("ShowHasTower");
        if (!hasTower)//格子上面没有塔的时候
        {
            spriteRenderer.enabled = true;
            towerListGo.transform.position = CorrectTowerListGoPosition();
            towerListGo.SetActive(true);

        }
        else
        {
            handleTowerListGo.transform.position = transform.position;
            //Debug.Log("hasTower");
            CorrectHandleTowerCanvasGoPosition();
            handleTowerListGo.SetActive(true);
            //显示塔的攻击范围,这里不作为成员变量是因为每次塔升级都卖掉了之前的塔，新生成的新塔
            towerGo.transform.Find("attackRange").gameObject.SetActive(true); 
        }
    }
    public void HideGrid()
    {
        if (!hasTower)
        {
            towerListGo.SetActive(false);

        }
        else
        {
            handleTowerListGo.SetActive(false);
            //隐藏塔的攻击范围
            towerGo.transform.Find("attackRange").gameObject.SetActive(false);
        }
        spriteRenderer.enabled = false;
        
    }
    public void ShowCantBuild()
    {
        spriteRenderer.enabled = true;
        Tween tween = DOTween.To(()=>spriteRenderer.color,toColor=>spriteRenderer.color=toColor,new Color(1,1,1,0),2f);
        tween.OnComplete(()=>
        {
            spriteRenderer.enabled = false;
            spriteRenderer.color = new Color(1,1,1,1);
        });
    }
    //造塔UI的面板位置修正
    private Vector3 CorrectTowerListGoPosition()
    {
        Vector3 correctPosition = transform.position;
        if (gridIndex.xIndex<=3&&gridIndex.xIndex>=0)//靠左边，往右移动 一个格子距离
        {
            correctPosition += new Vector3(gameController.mapMaker.gridWidth,0,0);
        }
        else if (gridIndex.xIndex>=8&&gridIndex.xIndex<=11)
        {
            correctPosition -= new Vector3(gameController.mapMaker.gridWidth,0,0);
        }
        if (gridIndex.yIndex <=3 && gridIndex.yIndex >=0)
        {
            correctPosition += new Vector3(0, gameController.mapMaker.gridHeight, 0);
        }
        else if (gridIndex.yIndex >= 4 && gridIndex.xIndex <= 7)
        {
            correctPosition -= new Vector3(0, gameController.mapMaker.gridHeight, 0);
        }

        return correctPosition;
    }
    //升级销售塔的面板位置修正
    private void CorrectHandleTowerCanvasGoPosition()
    {
        upLevelButtonTrans.localPosition = Vector3.zero;
        sellTowerButtonTrans.localPosition = Vector3.zero;
        if (gridIndex.yIndex<=0)//最下面的时候
        {
            if (gridIndex.xIndex<=0)
            {
                sellTowerButtonTrans.position += new Vector3(gameController.mapMaker.gridWidth*3/4,0,0);
            }
            else
            {
                sellTowerButtonTrans.position -= new Vector3(gameController.mapMaker.gridWidth*3/4,0,0);
            }
            upLevelButtonTrans.localPosition = uplevelBttonInitPos;
        }
        else if (gridIndex.yIndex>=6)
        {
            if (gridIndex.xIndex <= 0)
            {
                upLevelButtonTrans.position += new Vector3(gameController.mapMaker.gridWidth * 3 / 4, 0, 0);
            }
            else
            {
                upLevelButtonTrans.position -= new Vector3(gameController.mapMaker.gridWidth * 3 / 4, 0, 0);
            }
            sellTowerButtonTrans.localPosition = sellTowerButtonInitPos;
        }
        else
        {
            upLevelButtonTrans.localPosition = uplevelBttonInitPos;
            sellTowerButtonTrans.localPosition = sellTowerButtonInitPos;
        }
    }
    //建造塔之后的处理方法
    public void AfterBuildTower()
    {
        spriteRenderer.enabled = false;
        towerGo= transform.GetChild(1).gameObject;
        tower= towerGo.GetComponent<Tower>();
        
        towerPersonalProperty = towerGo.GetComponent<TowerPersonalProperty>();
        towerLevel = towerPersonalProperty.towerLevel;
    }
#endif
#if Tool
    private void OnMouseDown()
    {
        //怪物路点
        if (Input.GetKey(KeyCode.P))
        {
            gridState.canBuild = false;
            spriteRenderer.enabled = true;//开始渲染
            gridState.isMonsterPoint = !gridState.isMonsterPoint;//如果像要改变，不想要这个怪物路径点了
            if (gridState.isMonsterPoint)//是路径点
            {
                MapMaker.Instance.monsterPath.Add(gridIndex);
                spriteRenderer.sprite = monsterSprite;//显示一个怪物的图片给予提示
            }
            else//不是怪物路径点
            {
                MapMaker.Instance.monsterPath.Remove(gridIndex);
                spriteRenderer.sprite = gridSprite;
            }
        }
        //道具
        else if (Input.GetKey(KeyCode.I))
        {
            gridState.itemID++;
            //当前格子从持有道具转化为没有道具
            if (gridState.itemID==itemPrefabs.Length)
            {
                gridState.hasItem = false;
                gridState.itemID = -1;
                Destroy(currenItem);
                return;
            }
            if (currenItem==null)
            {
                //产生道具
                CreateItem();
            }
            else//本身就有道具，先销毁——我觉得可以只做一个Sprite展示，真正到要创建的时候再产生,那要增加一个监听按钮，那是优化的事了
            {
                Destroy(currenItem);
                //产生道具
                CreateItem();
            }
            
            gridState.hasItem = true;
        }
        else if (Input.GetKey(KeyCode.A)&&currenItem!=null)//一键消除道具，我自己加的。。
        {
            gridState.hasItem = false;
            gridState.itemID = -1;
            Destroy(currenItem);
        }
        else if (!gridState.isMonsterPoint)//排除掉是路径点的情况，因为如果是路径点要走路径点的逻辑：再点一次取消
        {
            gridState.isMonsterPoint = false;//不是很理解这一行代码的必要性，默认已经是false了
            gridState.canBuild = !gridState.canBuild;
            if (!gridState.canBuild)
            {
                spriteRenderer.enabled = false;
            }
            else
            {
                spriteRenderer.enabled = true;
            }
        }
    }
    //生成道具
    private void CreateItem()
    {
        Vector3 createPos = transform.position;
        if (gridState.itemID<=2)//占用了四个格子的，横竖都移动半个格子
        {
            createPos += new Vector3(MapMaker.Instance.gridWidth,-MapMaker.Instance.gridHeight)/2;
        }
        else if (gridState.itemID<=4)//两个格子的，右移动半个
        {
            createPos += new Vector3(MapMaker.Instance.gridWidth,0) / 2;
        }
        GameObject itemGO = Instantiate(itemPrefabs[gridState.itemID],createPos,Quaternion.identity);
        currenItem = itemGO;
    }
    //更新格子状态
    public void UpdateGrid()
    {
        if (gridState.canBuild)
        {
            spriteRenderer.sprite = gridSprite;
            spriteRenderer.enabled = true;
            if (gridState.hasItem)
            {
                CreateItem();
            }
        }
        else
        {//这里之前没做，如果是怪物路点药做怪物渲染
            if (gridState.isMonsterPoint)
            {
               // spriteRenderer.enabled = true;
                spriteRenderer.sprite = monsterSprite;
            }
            else
            {

                spriteRenderer.enabled = false;
            }
        }
    }
#endif
}
