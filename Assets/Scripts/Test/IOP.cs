using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOP : MonoBehaviour {

	// Use this for initialization
	void Start () {
        BaseHero myHero = new Leblanc();
        myHero.SkillE();
        myHero.hp = 10;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

public interface IHero
{
    void SkillQ();
    void SkillW();
    void SkillE();
    void SkillR();
}
public class BaseHero : IHero
{
    public int hp;
    public virtual void SkillE()
    {
        Debug.Log("玩家按下了E");
    }

    public void SkillQ()
    {
        Debug.Log("恶意魔音");
    }

    public void SkillR()
    {
        Debug.Log("故技重施");
    }

    public void SkillW()
    {
        Debug.Log("魔影密宗");
    }
}

public class Leblanc : BaseHero
{
    public override void SkillE()
    {
        base.SkillE();
        Debug.Log("幻影锁链");
    }

    //public void SkillQ()
    //{
    //    Debug.Log("恶意魔音");
    //}

    //public void SkillR()
    //{
    //    Debug.Log("故技重施");
    //}

    //public void SkillW()
    //{
    //    Debug.Log("魔影密宗");
    //}
}
public class Zed : IHero
{
    public void SkillE()
    {
        Debug.Log("中二3");
    }

    public void SkillQ()
    {
        Debug.Log("中二1");
    }

    public void SkillR()
    {
        Debug.Log("中二*R");
    }

    public void SkillW()
    {
        Debug.Log("中二2");
    }
}
