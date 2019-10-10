using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    public GameObject monster;
    public Stack<GameObject> monsterPool;
    public Stack<GameObject> activeMonsterList;
	// Use this for initialization
	void Start () {
        monsterPool = new Stack<GameObject>();
        activeMonsterList = new Stack<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("按了1");
            GameObject itemGO=GetMonster();
            itemGO.transform.position = Vector3.one;
            activeMonsterList.Push(itemGO);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (activeMonsterList.Count>0)
            {
                PushMonster(activeMonsterList.Pop());
            }
        }
	}
    public GameObject GetMonster()
    {
        //Debug.Log("调用1");
        GameObject monsterGO = null;
        if (monsterPool.Count<=0)
        {
            //Debug.Log("执行了1次");
            monsterGO = Instantiate(monster);
        }
        else
        {
            monsterGO = monsterPool.Pop();//pop方法是推出,拿走。push是推入，放进去
            monsterGO.SetActive(true);
        }
        return monsterGO;
    }
    private void PushMonster(GameObject monsterGo)
    {
        monsterGo.transform.SetParent(transform);
        monsterGo.SetActive(false);
        monsterPool.Push(monsterGo);
    }
}
