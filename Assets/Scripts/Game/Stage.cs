using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//舞台，关卡数据记录
public class Stage  {

    public int mBigLevelID;
    public int mLevelID;
    public int[] mTowerIDList;//本关卡可以建造的塔
    public int mTowerListLength;//键塔数组长度
    public int mCarrotState;//萝卜状态，金银普通
    public int mTotalRound;

    public bool unLocked;
    public bool mIsRewardLevel;
    public bool mAllClear;

    //public Stage(int bigLevelID, int levelID, int[] towerIDList, int towerListLength, int carrotState, int totalRound,
    //    bool unLock, bool isRewardLevel, bool allClear)
    //{
    //    mBigLevelID = bigLevelID;
    //    mLevelID = levelID;
    //    mTowerIDList = towerIDList;
    //    mTowerListLength = towerListLength;
    //    mCarrotState = carrotState;
    //    mTotalRound = totalRound;

    //    unLocked = unLock;
    //    mIsRewardLevel = isRewardLevel;
    //    mAllClear = allClear;
    //}
}
