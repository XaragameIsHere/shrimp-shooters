using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using DG.Tweening;

public class Beaterator : MonoBehaviour
{
    //models
    [Header("Models")]
    [SerializeField] GameObject shrimpModel;
    
    //shrimp spawn positions
    [Header("Start Positions")]
    [SerializeField] Transform positionStartLeft;
    [SerializeField] Transform positionStartRight;
    [SerializeField] Transform positionStartMiddle;
    
    //shrimp end positions
    [Header("End Positions")]
    [SerializeField] Transform positionEndLeft;
    [SerializeField] Transform positionEndRight;
    [SerializeField] Transform positionEndMiddle;
    
    //variables
    private enum Positions
    {
        Left,
        Middle,
        Right,
    }
    private Vector3 _startPos;
    private Vector3 _endPos;

    class ShrimpBeat
    {
        public GameObject ShrimpObject;
        public Rigidbody ShrimpPhys;
        public Collider ShrimpCollider;
        public ParticleSystem Explosion;

        public ShrimpBeat(GameObject model)
        {
            ShrimpObject = Instantiate(model);
            ShrimpPhys = ShrimpObject.GetComponent<Rigidbody>();
            ShrimpCollider = ShrimpObject.GetComponent<Collider>();
            Explosion = ShrimpObject.GetComponent<ParticleSystem>();
        }

        public void Krill()
        {
            
        }
    }
    
    // shoots a shrimp and holds on screen for a certain time
    public void Hold(float holdTime, float pressTime, float speed, string side)
    {
        var enumStartPosition = Enum.Parse(typeof(Positions), side);

        ShrimpBeat newShrimp = new ShrimpBeat(shrimpModel);
        newShrimp.ShrimpPhys.useGravity = false;
        
        switch (enumStartPosition)
        {
            case Positions.Left:
                _startPos = positionStartLeft.position;
                _endPos = positionEndLeft.position;
                break;
            case Positions.Middle:
                _startPos = positionStartMiddle.position;
                _endPos = positionEndMiddle.position;
                break;
            case Positions.Right:
                _startPos = positionStartRight.position;
                _endPos = positionEndRight.position;
                break;
        }
        
        newShrimp.ShrimpObject.transform.position = _startPos;
        
        newShrimp.ShrimpObject.transform.DOMove(_endPos, speed);
    }

    private void Start()
    {
        Hold(2, .25f, 1, "Left");
    }
}
