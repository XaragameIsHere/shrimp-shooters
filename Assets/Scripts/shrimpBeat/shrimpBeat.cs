using System;
using DG.Tweening;
using UnityEngine;

public class shrimpBeat : MonoBehaviour
{
    public Rigidbody ShrimpPhys;
    public Collider ShrimpCollider;
    public ParticleSystem Explosion;
    public bool held;
    public Tween mainTween;

    
    public enum beatTypes
    {
        Hold,//
        tweenOff,//
        Bounce,//
        clearScreen,
        Log,
        Explosion
    }

    public beatTypes BeatType;
    
    public virtual void Awake()
    {
        ShrimpPhys = gameObject.GetComponent<Rigidbody>();
        ShrimpCollider = gameObject.GetComponent<Collider>();
        Explosion = gameObject.GetComponent<ParticleSystem>();
    }

    public virtual void OnParticleTrigger()
    {
        
    }

    public virtual void FixedUpdate()
    {
        
    }
}
