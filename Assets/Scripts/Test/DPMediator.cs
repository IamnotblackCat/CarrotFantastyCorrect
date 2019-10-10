using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPMediator : MonoBehaviour {

	// Use this for initialization
	void Start () {
        MatchMaker men =new Men(45,20000,99,0);
        MatchMaker women = new Women(18,0,60,0);

        WomenMatcherMakerMediator mediator = new WomenMatcherMakerMediator(men,women);
        mediator.OfferMenInformation();
        mediator.OfferWomenInformation();

        Debug.Log("男方目前的好感度是" + men.m_favor);
        Debug.Log("nv方目前的好感度是" + women.m_favor);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
public abstract class MatchMaker
{
    public int m_age;
    public int m_money;
    public int m_familyBG;
    public int m_favor;

    public abstract void GetInfomation(MatchMaker otherMaker);
}
public class Men : MatchMaker
{
    public Men(int age, int money, int familyBG, int favor)
    {
        m_age = age;
        m_money = money;
        m_familyBG = familyBG;
        m_favor = favor;
    }
    public override void GetInfomation(MatchMaker otherMaker)
    {
        m_favor = -otherMaker.m_age * 3 + otherMaker.m_money + otherMaker.m_familyBG;
    }
}
public class Women : MatchMaker
{
    public Women(int age, int money, int familyBG, int favor)
    {
        m_age = age;
        m_money = money;
        m_familyBG = familyBG;
        m_favor = favor;
    }
    public override void GetInfomation(MatchMaker otherMaker)
    {
        m_favor = (otherMaker.m_age-18) * 3 + otherMaker.m_money + otherMaker.m_familyBG;
    }
}
public class WomenMatcherMakerMediator
{
    private MatchMaker m_men;
    private MatchMaker m_women;

    public WomenMatcherMakerMediator(MatchMaker men, MatchMaker women)
    {
        m_men = men;
        m_women = women;
    }
    public void OfferWomenInformation()
    {
        m_men.m_favor = -m_women.m_age * 3 + m_women.m_money + m_women.m_familyBG;
    }
    public void OfferMenInformation()
    {
        m_women.m_favor = -m_men.m_age * 3 + m_men.m_money + m_men.m_familyBG;
    }
}
