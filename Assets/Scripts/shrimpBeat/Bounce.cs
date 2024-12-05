using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bounce : shrimpBeat
{


    public void Go(float dir, float speed, float downDir)
    {
        ShrimpPhys.useGravity = true;
        
        ShrimpPhys.AddForce(new Vector3(800 * dir, 6000 * speed * downDir));
    }
    
    public override void FixedUpdate()
    {
        if (held)
        {
            print("Point");
            Destroy(gameObject);
        }
    }
}
