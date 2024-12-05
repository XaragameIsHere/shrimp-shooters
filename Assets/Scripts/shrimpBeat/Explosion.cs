using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Explosion : MonoBehaviour
{
    public ParticleSystem ps;

    private void OnParticleTrigger()
    {
        //print("b");
        //if (particleSystem.layer == 3)
        //{
            print("OnParticleTrigger");
            List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
            
            int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
    
            // iterate
            for (int i = 0; i < numEnter; i++)
            {
                ParticleSystem.Particle p = enter[i];
                print("point");
                enter[i] = p;
            }
        //}
        
    }
}
