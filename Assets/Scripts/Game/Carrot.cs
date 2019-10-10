using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Carrot : MonoBehaviour {

    private Sprite[] sprites;
    private Text hpText;
    private SpriteRenderer sr;
    private Animator animator;
    private float timeVal;//计时器负责idle每过多久一次；

	// Use this for initialization
	void Start () {
        sprites = new Sprite[7];
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i] = GameController.Instance.GetSprite("NormalMordel/Game/Carrot/"+i);
        }
        sr = GetComponent<SpriteRenderer>();
        hpText = transform.Find("HpCanvas").Find("Text").GetComponent<Text>();
        animator = GetComponent<Animator>();
        timeVal = 3;
	}
	
	// Update is called once per frame
	void Update () {
        if (GameController.Instance.carrotHP<10)
        {
            animator.enabled = false;
        }
        if (timeVal>=3)
        {
            animator.Play("CarrotIdle");
            timeVal = 0;
        }
        else
        {
            timeVal += Time.deltaTime;
        }
	}
    private void OnMouseDown()
    {
        if (GameController.Instance.carrotHP>=10)
        {
            animator.Play("CarrotTouch");
            int randomNum = Random.Range(1, 4);
            GameController.Instance.PlayEffectClip("NormalMordel/Carrot/" + randomNum.ToString());
        }
        
    }
    //切换萝卜掉血的图片
    public void UpdateCarrotUI()
    {
        int hp = GameController.Instance.carrotHP;
        hpText.text = hp.ToString();
        //Debug.Log(hp);
        if (hp>=7&&hp<10)
        {
            //Debug.Log("zhixing");
            sr.sprite = sprites[6];
        }
        else if (hp>0&&hp<7)
        {
            sr.sprite = sprites[hp - 1];
        }
        else
        {
            //游戏失败
           // Debug.Log("游戏结束");
            GameController.Instance.normalModelPanel.ShowGameOverPage();
            GameController.Instance.gameOver = true;
            //GameController.Instance.creatingMonster = false;
        }
    }
}
