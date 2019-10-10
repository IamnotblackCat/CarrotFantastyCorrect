using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBuilder : IBuilder<Monster>
{
    public int m_monsterID;
    private GameObject monsterGO;

    public void GetData(Monster productClassGo)
    {
        productClassGo.monsterID = m_monsterID;
        productClassGo.HP = m_monsterID * 110;
        productClassGo.currentHP = productClassGo.HP;
        productClassGo.initMoveSpeed = m_monsterID;//防止怪物初始化调用的初始速度变为0
        productClassGo.moveSpeed = m_monsterID ;
        productClassGo.prize = m_monsterID * 50;
    }

    public void GetOtherResource(Monster productClassGo)
    {
        productClassGo.GetMonsterProperty();
    }

    public GameObject GetProduct()
    {
        GameObject monsterGO = GameController.Instance.GetGameObjectResource("MonsterPrefab");
        Monster monster = GetProductClass(monsterGO);
        GetData(monster);
        GetOtherResource(monster);

        return monsterGO;
    }

    public Monster GetProductClass(GameObject gameObject)
    {
        return gameObject.GetComponent<Monster>();
    }
}
