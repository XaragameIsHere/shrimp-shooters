using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class tweenOff : shrimpBeat
{
    
    public override void FixedUpdate()
    {
        if (held)
        {
            print("Point");
            mainTween.Kill();
            Destroy(gameObject);
        }
    }
}
