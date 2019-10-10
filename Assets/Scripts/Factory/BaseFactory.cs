using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏物体类型的工厂基类
/// </summary>
public class BaseFactory : IBaseFactory
{
    //当前拥有的gameobject类型的资源（UI/UIPanel/Game），切记：它存放的是游戏物体预制体资源
    protected Dictionary<string, GameObject> factoryDict = new Dictionary<string, GameObject>();
    //对象池字典
    protected Dictionary<string, Stack<GameObject>> objectPoolDict = new Dictionary<string, Stack<GameObject>>();

    protected string loadPath ;
    public BaseFactory()
    {
        loadPath = "Prefabs/";
    }
    /// <summary>
    /// 放入池子
    /// </summary>
    /// <param name="itemName">对象池类别的名字</param>
    /// <param name="item">具体的对象</param>
    public void PushItem(string itemName, GameObject item)
    {
        item.SetActive(false);
        item.transform.SetParent(GameManager.Instance.transform);
        if (objectPoolDict.ContainsKey(itemName))
        {
            objectPoolDict[itemName].Push(item);//放入指定对象池的栈的最顶端
        }
        else
        {
            Debug.Log("当前字典没有："+itemName+"的栈");
        }
    }
    //取实例
    public GameObject GetItem(string itemName)
    {
        GameObject itemGO = null;
        if (objectPoolDict.ContainsKey(itemName))
        {
            //对象池里面是否有物体
            if (objectPoolDict[itemName].Count==0)
            {
                itemGO = GameManager.Instance.CreateGO(GetResource(itemName));
            }
            else
            {
                itemGO = objectPoolDict[itemName].Pop();//推出指定对象池的最末尾的一个物体
                itemGO.SetActive(true);//记得激活
            }
        }
        else
        {
            objectPoolDict.Add(itemName, new Stack<GameObject>());
            //取到资源以后要进行实例化。这个参数是下面的取资源的方法，我简写了，本来是写两行的
            itemGO = GameManager.Instance.CreateGO(GetResource(itemName));
        }
        //如果经过以上所有，还是为空
        if (itemGO==null)
        {
            Debug.Log(itemName+"的实例获取失败");
        }
        return itemGO;
    }
    //取资预制体源的方法
    public GameObject GetResource(string itemName)
    {
        GameObject itemGO = null;
        string itemLoadPath = loadPath + itemName;//中间的路径会有子类来添加，这个是基类。
        if (factoryDict.ContainsKey(itemName))
        {
            itemGO = factoryDict[itemName];
        }
        else
        {//没有就实例化出来
            itemGO = Resources.Load<GameObject>(itemLoadPath);
            factoryDict.Add(itemName,itemGO);
        }
        //安全校验，如果还是没得到，那就说明路径错了
        if (itemGO==null)
        {
            Debug.Log("资源路径加载失败"+itemName+"路径是："+itemLoadPath);
        }
        return itemGO;
    }
}
