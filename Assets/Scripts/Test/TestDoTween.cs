using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TestDoTween : MonoBehaviour {

    //private Image maskImage;
	// Use this for initialization
	void Start () {
        //maskImage = GetComponent<Image>();
        //参数一，想要改变的值，参数二toColor：每次doTween经过计算得到的值。参数三 new Color（）：目标值和完成时间
        //DOTween.To(() => maskImage.color, toColor => maskImage.color = toColor, new Color(0, 0, 0, 0), 2f);
    }
	
	// Update is called once per frame
	void Update () {
       
	}
}
