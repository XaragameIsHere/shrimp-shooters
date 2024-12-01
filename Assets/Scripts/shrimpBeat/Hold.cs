using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hold : shrimpBeat
{
    public float startHoldTime;
    public float startHoldAirTime;
    public float holdTime;
    public GameObject image;
    public float gameStartTime;
    public bool active = false;
    
    public void startTimer()
    {
        gameStartTime = Time.time;
        print(Time.time);
        active = true;
    }
    
    public override void FixedUpdate()
    {
        if (active)
        {
            print(holdTime / startHoldTime);
            image.transform.Find("Image").localPosition = new Vector3(0, 100, 0);
            image.transform.Find("Image").localPosition = new Vector3(0, image.transform.localPosition.y * Mathf.Clamp01(holdTime / startHoldTime), 0);

            if (holdTime <= 0)
            {
                print("Point");
                Destroy(image);
                Destroy(gameObject);
            } 
            else if (Time.time >= gameStartTime + startHoldAirTime)
            {
                Destroy(gameObject);
                Destroy(image);
                print(Time.time);
            } 
            else if (held)
            {
                holdTime -= Time.deltaTime;
                print(holdTime / startHoldTime);
            }

        }
    }
}
