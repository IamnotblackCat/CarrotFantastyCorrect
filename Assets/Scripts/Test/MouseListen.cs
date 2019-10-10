using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseListen : MonoBehaviour,IPointerDownHandler {
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Wow,it`s gold,yeah!!!");
    }
    private void OnMouseDown()
    {
        if (Input.GetKey(KeyCode.P))
        {
            Debug.Log("haliyakatongtongtong!");
        }
        if (Input.GetKey(KeyCode.I))
        {
            Debug.Log("WhatAreYouNongShaLie?");
        }
    }

}
