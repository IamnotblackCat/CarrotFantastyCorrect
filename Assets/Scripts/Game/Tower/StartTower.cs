﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTower : TowerPersonalProperty {

    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()//重写这个的目的是为了去掉转向，便便塔不需要转向
    {
        if (GameController.Instance.isPause || targetTrans == null||GameController.Instance.gameOver)
        {
            return;
        }
        if (timeVal >= attackCD / GameController.Instance.gameSpeed)
        {
            timeVal = 0;
            Attack();
        }
        else
        {
            timeVal += Time.deltaTime;
        }
    }
}
