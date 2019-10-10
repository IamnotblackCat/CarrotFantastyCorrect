using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPFacade : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Principal principal = new Principal();
        principal.OrderSummary();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
public class Principal
{
    Teacher teacher = new Teacher();
    public void OrderSummary()
    {
        teacher.OrderStudentWriteSummary();
    }
}
public class Teacher
{
    public Monitor monitor = new Monitor();
    public LeagueSecretary leagueSecretary = new LeagueSecretary();

    public void OrderStudentWriteSummary()
    {
        monitor.WriteSummary();
        leagueSecretary.WriteSummary();
    }
}
public class Monitor
{
    public void WriteSummary()
    {
        Debug.Log("班长的总结");
    }
}
public class LeagueSecretary
{
    public void WriteSummary()
    {
        Debug.Log("团支书的总结");
    }
}
