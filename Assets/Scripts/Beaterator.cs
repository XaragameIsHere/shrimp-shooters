using System;
using System.Collections;
using System.Net.Mime;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class Beaterator : MonoBehaviour
{
    [SerializeField] Camera _mainCamera;
    
    //models
    [Header("Models")]
    [SerializeField] GameObject shrimpModel;
    
    //shrimp spawn positions
    [Header("Start Positions")]
    [SerializeField] Transform positionStartLeft;
    [SerializeField] Transform positionStartRight;
    [SerializeField] Transform positionStartMiddle;
    
    //shrimp end positions
    [Header("End Positions Hold")]
    [SerializeField] Transform positionEndLeftHold;
    [SerializeField] Transform positionEndRightHold;
    [SerializeField] Transform positionEndMiddleHold;
    
    [Header("End Positions Tween Off")]
    [SerializeField] Transform positionEndLeftOff;
    [SerializeField] Transform positionEndRightOff;
    [SerializeField] Transform positionEndMiddleOff;
    
    [Header("UI Elements")]
    [SerializeField] GameObject holdUI;
    [SerializeField] Transform _canvas;
    
    //variables
    [Header("Variables")] 
    public float bpm = 150;
    
    private enum Positions
    {
        Left,
        Middle,
        Right,
    }
    private Vector3 _startPos;
    private Vector3 _endPos;

    private IEnumerator holdStart(GameObject shrimpObjectMoving)
    {
        yield return shrimpObjectMoving.GetComponent<shrimpBeat>().mainTween.WaitForCompletion();
        
        
        shrimpObjectMoving.GetComponent<Hold>().image = Instantiate(holdUI, _canvas);
        shrimpObjectMoving.GetComponent<Hold>().image.transform.position = shrimpObjectMoving.transform.position;
        shrimpObjectMoving.GetComponent<Hold>().startTimer();
    }
    
    private void Move(string side, shrimpBeat.beatTypes shrimpBeatType, GameObject shrimpObjectToMove, float speed)
    {
        print(Time.time);
        var enumStartPosition = Enum.Parse(typeof(Positions), side);

        var mid = positionStartRight.position;
        var right = positionStartRight.position;
        var left = positionStartRight.position;
        
        switch (shrimpBeatType)
        {
            case shrimpBeat.beatTypes.Hold:
                right = positionEndRightHold.position;
                mid = positionEndMiddleHold.position;
                left = positionEndLeftHold.position;
                break;
            case shrimpBeat.beatTypes.tweenOff:
                right = positionEndRightOff.position;
                mid = positionEndMiddleOff.position;
                left = positionEndLeftOff.position;
                break;
        }
        
        switch (enumStartPosition)
        {
            case Positions.Left:
                _startPos = positionStartLeft.position;
                _endPos = left;
                break;
            case Positions.Middle:
                _startPos = positionStartMiddle.position;
                _endPos = mid;
                break;
            case Positions.Right:
                _startPos = positionStartRight.position;
                _endPos = right;
                break;
        }
        
        shrimpObjectToMove.transform.position = _startPos;
        shrimpObjectToMove.GetComponent<shrimpBeat>().mainTween = shrimpObjectToMove.transform.DOMove(_endPos, speed);

        if (shrimpBeatType == shrimpBeat.beatTypes.Hold)
            StartCoroutine(holdStart(shrimpObjectToMove));
        
    }

    private IEnumerator multiMove(string side, float speed, float beatTime)
    {
        var beatCount = Mathf.CeilToInt(bpm * (beatTime / 60));
        
        for (int i = 0; i < side.Length; i++)
        {
            var ShrimpObject = Instantiate(shrimpModel);
            ShrimpObject.gameObject.AddComponent<tweenOff>();
            ShrimpObject.GetComponent<tweenOff>().ShrimpPhys.useGravity = false;
            
            Move(side, shrimpBeat.beatTypes.tweenOff, ShrimpObject, speed);
            yield return new WaitForSeconds(beatTime/beatCount);
        }
    }
    
    // shoots a shrimp and holds on screen for a certain time
    public void Hold(float holdTime, float pressTime, float speed, string side)
    {

        var ShrimpObject = Instantiate(shrimpModel);
        var Beat = ShrimpObject.AddComponent<Hold>();
        
        Beat.holdTime = pressTime;
        Beat.startHoldTime = pressTime;
        Beat.startHoldAirTime = holdTime;
        
        Move(side, shrimpBeat.beatTypes.Hold, ShrimpObject, speed);
        
    }

    
    
    public void tweenOff(float speed, String side, float beatTime)
    {
        

        StartCoroutine(multiMove(side, speed, beatTime));
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            tweenOff(.7f, "Left", .5f);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Hold(1.5f, .75f, .4f, "Right");
        }
    }
}
