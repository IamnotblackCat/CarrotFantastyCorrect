using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager{

    public int adventrueModelNum;
    public int burriedLevelNum;
    public int bossModelNum;
    public int coin;
    public int killMonsterNum;
    public int killBossNum;
    public int clearItemNum;
    public List<bool> unLockedNormalModelBigLevelList;//对应大关卡是否解锁
    public List<Stage> unlockedNormalModelLevelList;
    public List<int> unlockedNormalModelLevelNum;//对应每个大关卡解锁的小关卡数量

    //怪物窝
    public int cookies;
    public int milk;
    public int monsterNest;
    public int diamands;
    public List<MonsterPetData> monsterPetDataList;//宠物喂养信息

    //public PlayerManager()
    //{
    //    adventrueModelNum = 0;
    //    burriedLevelNum = 0;
    //    bossModelNum = 0;
    //    coin = 0;
    //    cookies = 100;
    //    milk = 100;
    //    monsterNest = 1;
    //    killBossNum = 0;
    //    killMonsterNum = 0;
    //    clearItemNum = 0;
    //    unlockedNormalModelLevelNum = new List<int>() { 1, 0, 0 };
    //    unLockedNormalModelBigLevelList = new List<bool>() { true, false, false };
    //    unlockedNormalModelLevelList = new List<Stage>()
    //    {
    //        new Stage(1,1,new int[]{ 1},1,0,10,true,false,false),
    //        new Stage(1,2,new int[]{ 2},1,0,9,false,false,false),
    //        new Stage(1,3,new int[]{ 1,2},2,0,8,false,false,false),
    //        new Stage(1,4,new int[]{ 3},1,0,10,false,false,false),
    //        new Stage(1,5,new int[]{ 1,2,3},3,0,9,false,true,false),//第一关最后一个隐藏关卡
    //        new Stage(2,1,new int[]{ 3,2},2,0,8,false,false,false),
    //        new Stage(2,2,new int[]{ 1,3},2,0,10,false,false,false),
    //        new Stage(2,3,new int[]{ 4},1,0,9,false,false,false),
    //        new Stage(2,4,new int[]{ 1,4},2,0,8,false,false,false),
    //        new Stage(2,5,new int[]{ 4,2},2,0,10,false,true,false),
    //        new Stage(3,1,new int[]{ 3,4},2,0,9,false,false,false),
    //        new Stage(3,2,new int[]{ 5},1,0,8,false,false,false),
    //        new Stage(3,3,new int[]{5,4},2,0,7,false,false,false),
    //        new Stage(3,4,new int[]{ 1,3,5},3,0,10,false,false,false),
    //        new Stage(3,5,new int[]{ 1,4,5},3,0,10,false,true,false),
    //    };
    //    monsterPetDataList = new List<MonsterPetData>()
    //    {
    //        new MonsterPetData()
    //        {
    //            monsterID=1,
    //            monsterLevel=1,
    //            levelUpNeedCookies=0,
    //            levelUpNeedMilk=0
    //        }
    //    };
    //}
    //用于玩家所有关卡都解锁的Json文件的制作
    //public PlayerManager()
    //{
    //    adventrueModelNum = 12;
    //    burriedLevelNum = 3;
    //    bossModelNum = 0;
    //    coin = 999;
    //    killMonsterNum = 999;
    //    killBossNum = 0;
    //    clearItemNum = 999;
    //    cookies = 1000;
    //    milk = 1000;
    //    monsterNest = 10;
    //    diamands = 1000;
    //    unlockedNormalModelLevelNum = new List<int>()
    //    {
    //        5,5,5
    //    };
    //    unLockedNormalModelBigLevelList = new List<bool>()
    //    {
    //        true,true,true
    //    };
    //    unlockedNormalModelLevelList = new List<Stage>()
    //    {
    //        new Stage(1,1,new int[]{ 1},1,0,10,true,false,false),
    //        new Stage(1,2,new int[]{ 2},1,0,9,true,false,false),
    //        new Stage(1,3,new int[]{ 1,2},2,0,8,true,false,false),
    //        new Stage(1,4,new int[]{ 3},1,0,10,true,false,false),
    //        new Stage(1,5,new int[]{ 1,2,3},3,0,9,false,true,false),//第一关最后一个隐藏关卡
    //        new Stage(2,1,new int[]{ 3,2},2,0,8,true,false,false),
    //        new Stage(2,2,new int[]{ 1,3},2,0,10,true,false,false),
    //        new Stage(2,3,new int[]{ 4},1,0,9,true,false,false),
    //        new Stage(2,4,new int[]{ 1,4},2,0,8,true,false,false),
    //        new Stage(2,5,new int[]{ 4,2},2,0,10,false,true,false),
    //        new Stage(3,1,new int[]{ 3,4},2,0,9,true,false,false),
    //        new Stage(3,2,new int[]{ 5},1,0,8,true,false,false),
    //        new Stage(3,3,new int[]{5,4},2,0,7,true,false,false),
    //        new Stage(3,4,new int[]{ 1,3,5},3,0,10,true,false,false),
    //        new Stage(3,5,new int[]{ 1,4,5},3,0,10,false,true,false),
    //    };
    //    monsterPetDataList = new List<MonsterPetData>()
    //    {
    //        new MonsterPetData()
    //        {
    //            monsterID=1,
    //            monsterLevel=1,
    //            levelUpNeedCookies=0,
    //            levelUpNeedMilk=0
    //        },
    //        new MonsterPetData()
    //        {
    //            monsterID=2,
    //            monsterLevel=1,
    //            levelUpNeedCookies=0,
    //            levelUpNeedMilk=0
    //        },
    //        new MonsterPetData()
    //        {
    //            monsterID=3,
    //            monsterLevel=1,
    //            levelUpNeedCookies=0,
    //            levelUpNeedMilk=0
    //        }
    //    };
    //}
    public void SaveData()
    {
        Memento memento = new Memento();
        memento.SaveJsonFile();
    }
    public void ReadData()
    {
        Memento memento = new Memento();
        PlayerManager playerManager = memento.LoadByJson();
        adventrueModelNum = playerManager.adventrueModelNum;
        burriedLevelNum = playerManager.burriedLevelNum;
        bossModelNum = playerManager.bossModelNum;
        coin = playerManager.coin;
        killBossNum = playerManager.killBossNum;
        killMonsterNum = playerManager.killMonsterNum;
        clearItemNum = playerManager.clearItemNum;
        unLockedNormalModelBigLevelList = playerManager.unLockedNormalModelBigLevelList;
        unlockedNormalModelLevelList = playerManager.unlockedNormalModelLevelList;
        unlockedNormalModelLevelNum = playerManager.unlockedNormalModelLevelNum;
        cookies = playerManager.cookies;
        milk = playerManager.milk;
        monsterNest = playerManager.monsterNest;
        diamands = playerManager.diamands;
        monsterPetDataList = playerManager.monsterPetDataList;
    }
}
