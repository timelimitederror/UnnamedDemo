using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionController : MonoBehaviour
{
    private Image img;
    private float appearTime = 1f;
    private float keepTime = 1f;
    private float disappearTime = 1f;
    private bool isImgPlay = false;
    private bool isImgAppear = false;
    private bool isImgKeep = false;

    private float startPlayTime = 0f;
    private Action<GameScene01Victory> action;


    void Start()
    {
        img = GetComponent<Image>();

        action = new Action<GameScene01Victory>(victoryEvent =>
        {
            Debug.Log("´¥·¢¶¯»­");
            isImgPlay = true;
            isImgAppear = true;
            isImgKeep = false;
            startPlayTime = Time.fixedTime;
        });

        EventBus.Subscribe(action);
    }

    // Update is called once per frame
    void Update()
    {
        if (isImgPlay)
        {
            if (isImgAppear)
            {
                
                float alpha = (Time.fixedTime - startPlayTime) / appearTime;
                Color color = img.color;
                if (alpha >= 1f)
                {
                    color.a = 1f;
                    img.color = color;
                    isImgAppear = false;
                    isImgKeep = true;
                    startPlayTime = Time.fixedTime + keepTime;
                    return;
                }
                color.a = alpha;
                img.color = color;
            }
            else if (isImgKeep)
            {
                if (Time.fixedTime >= startPlayTime)
                {
                    isImgKeep = false;
                    startPlayTime = Time.fixedTime;
                }
            }
            else
            {
                float alpha = (Time.fixedTime - startPlayTime) / disappearTime;
                Color color = img.color;
                if (alpha >= 1f)
                {
                    color.a = 0f;
                    img.color = color;
                    isImgPlay = false;
                    return;
                }
                color.a = 1f - alpha;
                img.color = color;
            }
        }
    }
}
