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
        Hold,
        dropDown,
        tweenOff,
        bounce,
        clearScreen,
        explosion
    }

    public beatTypes BeatType;
    
    public virtual void Awake()
    {
        ShrimpPhys = gameObject.GetComponent<Rigidbody>();
        ShrimpCollider = gameObject.GetComponent<Collider>();
        Explosion = gameObject.GetComponent<ParticleSystem>();
    }

    public virtual void FixedUpdate()
    {
        
    }
}
