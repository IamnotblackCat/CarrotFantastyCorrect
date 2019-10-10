using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TshitBullet : Bullet {

    private BulletProperty bulletProperty;
	// Use this for initialization
	void Start () {
        bulletProperty = new BulletProperty
        {
            debuffTime = towerLevel * 0.3f,
            debuffValue = towerLevel * 0.4f
        };
	}
	
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.activeSelf)
        {
            return;
        }
        if (collision.tag=="Monster")
        {
            collision.SendMessage("DecreaseSpeed",bulletProperty);
        }
        base.OnTriggerEnter2D(collision);
    }
}
public struct BulletProperty
{
    public float debuffTime;
    public float debuffValue;
}
