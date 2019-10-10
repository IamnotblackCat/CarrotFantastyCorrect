using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItachFire : MonoBehaviour {

    private void Start()
    {
        Destroy(gameObject,2.5f);
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.transform.tag=="Monster")
    //    {
    //        Destroy(collision.gameObject);
    //    }
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("烧到了吗");
        if (collision.tag == "Monster")
        {
            //Debug.Log("烧到了怪物");
            collision.SendMessage("TakeDamage", 1000);
        }
    }
}
