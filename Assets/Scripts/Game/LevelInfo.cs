using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo  {

    public int bigLevelID;
    public int levelID;

    public List<GridPoint.GridState> gridPoints;//是格子的状态gridStates

    public List<GridPoint.GridIndex> monsterPath;//实际上是怪物的状态monsterState

    public List<Round.RoundInfo> roundInfo;
}
