using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewTest : MonoBehaviour {

    private ScrollRect scrollRect;
    private RectTransform contentRectTrans;
	// Use this for initialization
	void Start () {
        scrollRect = GetComponent<ScrollRect>();
        contentRectTrans = scrollRect.content;

        contentRectTrans.sizeDelta = new Vector2(320,300);

        scrollRect.horizontalNormalizedPosition = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
