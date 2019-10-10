using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBullet : MonoBehaviour {

    public int attackValue;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster"||collision.tag=="Item")
        {
            collision.SendMessage("TakeDamage",attackValue);
        }
    }
}
