using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuilder<T>{
    //获取游戏物体身上的脚本对象，从而区赋值
    T GetProductClass(GameObject gameObject);
    //使用工厂区获取具体的游戏对象
    GameObject GetProduct();
    //获取数据信息
    void GetData(T productClassGo);
    //获取数据之外的信息,属性，比如动画控制器
    void GetOtherResource(T productClassGo);
}
