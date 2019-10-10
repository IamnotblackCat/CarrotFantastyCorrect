using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuilder : IBuilder<Tower> {

    public int m_TowerID;
    private GameObject towerGo;
    public int m_TowerLevel;

    public void GetData(Tower productClassGo)
    {
        m_TowerID = productClassGo.towerID;
        //Debug.Log("Tower的ID："+productClassGo+"  :"+productClassGo.towerID);
    }

    public void GetOtherResource(Tower productClassGo)
    {
        productClassGo.GetTowerProperty();
    }

    public GameObject GetProduct()
    {
        GameObject towerGo = GameController.Instance.GetGameObjectResource("Tower/ID"+m_TowerID+"/TowerSet/"+m_TowerLevel);
        Tower tower= GetProductClass(towerGo);
       // Debug.Log(m_TowerID);
        //GetData(tower);
        //Debug.Log(m_TowerID);
        GetOtherResource(tower);
        return towerGo;
    }

    public Tower GetProductClass(GameObject gameObject)
    {
        //Tower tower = gameObject.GetComponent<Tower>();
        return gameObject.GetComponent<Tower>();
    }

   
}
