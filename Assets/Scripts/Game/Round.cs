using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round  {
    [System.Serializable]
    public struct RoundInfo
    {
        public int[] mMonsterIDList;
    }
    public RoundInfo roundInfo;
    protected Level mLevel;
    protected int mRoundID;
    protected Round mNextRound;

    public Round(int[]monsterIDList,Level Level,int roundID)
    {
        roundInfo.mMonsterIDList = monsterIDList;
        mRoundID = roundID;
    }

    public void SetNextRound(Round round)
    {
        mNextRound = round;
        //return mNextRound;
    }
    //责任链模式，当前关卡处理不来就让下一关卡处理
    public void Handle(int roundID)
    {
        if (mRoundID<roundID)
        {
            mNextRound.Handle(roundID);
        }
        else
        {
            //产生怪物
            GameController.Instance.mMonsterIDList = roundInfo.mMonsterIDList;
            GameController.Instance.CreateMonster();
            GameController.Instance.creatingMonster = true;
        }
    }
}
