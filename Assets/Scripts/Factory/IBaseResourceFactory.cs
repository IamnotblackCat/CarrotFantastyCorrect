using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 其他种类资源工厂的接口，每种工厂获取的资源不一样，所以用泛型接口
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IBaseResourceFactory<T>  {

    T GetSingleResources(string resourcePath);
}
