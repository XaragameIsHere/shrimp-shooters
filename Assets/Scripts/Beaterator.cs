using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class Beaterator : MonoBehaviour
{
    
    [HideInInspector] public beatProcessing.Beats beatsRoot; //JsonParse template for beat
    
    [SerializeField] Camera _mainCamera;
    [SerializeField] ParticleSystem _Particles;
    [SerializeField] TextAsset beatMapFile;
    [SerializeField] private AudioSource _audioSource;
    
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
    
    // UI Elements
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
    private List<GameObject> shrimps = new List<GameObject>();
    private bool clear;
    
    //Clears screen of shrimp
    public void clearScreen()
    {
        
        clear = true;//sets boolean for whether
        //Iterate through shrimp on screen
        
        foreach (GameObject shrimp in shrimps)
        {
            //checks for any active tweens and kills them
            if (shrimp != null)
            {
                if (shrimp.GetComponent<shrimpBeat>().mainTween != null)
                    shrimp.GetComponent<shrimpBeat>().mainTween.Kill();
            }
                
            
        }
        
        foreach (GameObject shrimp in shrimps)
        {
            
            Destroy(shrimp);//destroys the shrimp
        }
    }
    
    //Initiates the timer for the hold function
    private IEnumerator holdStart(GameObject shrimpObjectMoving)
    {
        //waits until the shrimp has been tweened to the point on the screen
        yield return shrimpObjectMoving.GetComponent<shrimpBeat>().mainTween.WaitForCompletion();
        
        //Adds holding UI onto the screen
        shrimpObjectMoving.GetComponent<Hold>().image = Instantiate(holdUI, _canvas);
        shrimpObjectMoving.GetComponent<Hold>().image.transform.position = shrimpObjectMoving.transform.position;
        shrimpObjectMoving.GetComponent<Hold>().startTimer();
    }
    
    //Main function for tweening shrimp to specific points
    private void Move(string side, shrimpBeat.beatTypes shrimpBeatType, GameObject shrimpObjectToMove, float speed)
    {
        
        var enumStartPosition = Enum.Parse(typeof(Positions), side);//Converts string value in json to enum of what side the shrimp spawns on

        //initiates end position variables
        var mid = positionStartRight.position;
        var right = positionStartRight.position;
        var left = positionStartRight.position;
        
        //changes end position type depending on the type of beat, hold or tweenOff
        switch (shrimpBeatType)
        {
            //Sets the end position for each side
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
        
        //Decides on which start and end position is executed
        switch (enumStartPosition)
        {
            case Positions.Left:
                _startPos = positionStartLeft.position;//sets the start position as the side it's on
                _endPos = left;//sets end position as the one newly defined by the type of function
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
        
        
        shrimpObjectToMove.transform.position = _startPos;//Places the shrimp in the start position
        shrimpObjectToMove.GetComponent<shrimpBeat>().mainTween = shrimpObjectToMove.transform.DOMove(_endPos, speed);//Tweens it away

        //(FOR HOLD FUNCTION) Starts the hold timer
        if (shrimpBeatType == shrimpBeat.beatTypes.Hold)
            StartCoroutine(holdStart(shrimpObjectToMove));
        
    }

    //Moves multiple shrimp for the tweenOff thing
    private IEnumerator multiMove(string side, float speed, float beatTime)
    {
        var beatCount = Mathf.CeilToInt(bpm * (beatTime / 60));//Determines how many shrimps have to be spawned based of the beats per minute times the length of the beat in minutes. Then rounded
        
        //instantiates the amount of shrimp from beat count
        for (int i = 0; i < beatCount; i++)
        {
            if (!clear)//if the clear boolean set by clear screen is false then create shrimp
            {
                GameObject ShrimpObject = Instantiate(shrimpModel);
                shrimps.Add(ShrimpObject);
                ShrimpObject.gameObject.AddComponent<tweenOff>();
                ShrimpObject.GetComponent<tweenOff>().ShrimpPhys.useGravity = false;
                
                Move(side, shrimpBeat.beatTypes.tweenOff, ShrimpObject, speed);
                yield return new WaitForSeconds(beatTime/beatCount);
            }
            else//if it's not then stop shooting more shrimp and exit the loop
            {
                clear = false;
                break;
            }
            
        }
    }
    
    // shoots a shrimp and holds on screen for a certain time
    public void Hold(float holdTime, float pressTime, float speed, string side)
    {

        GameObject ShrimpObject = Instantiate(shrimpModel);
        shrimps.Add(ShrimpObject);
        var Beat = ShrimpObject.AddComponent<Hold>();
        
        Beat.holdTime = pressTime;
        Beat.startHoldTime = pressTime;
        Beat.startHoldAirTime = holdTime;
        
        Move(side, shrimpBeat.beatTypes.Hold, ShrimpObject, speed);
        
    }

    public void Bounce(string side, float speed)
    {
        var enumStartPosition = Enum.Parse(typeof(Positions), side);
        float dir = 0;
        float downDir = 0;
        
        GameObject ShrimpObject = Instantiate(shrimpModel);
        shrimps.Add(ShrimpObject);
        
        switch (enumStartPosition)
        {
            case Positions.Left:
                ShrimpObject.transform.position = positionStartLeft.position;
                dir = 1;
                downDir = 1;
                break;
            case Positions.Middle:
                ShrimpObject.transform.position = positionStartMiddle.position;
                dir = 0;
                downDir = -1;
                break;
            case Positions.Right:
                ShrimpObject.transform.position = positionStartRight.position;
                dir = -1;
                downDir = 1;
                break;
        }
        
        ShrimpObject.AddComponent<Bounce>().Go(dir, speed, downDir);
    }
    
    public void tweenOff(float speed, String side, float beatTime)
    {
        StartCoroutine(multiMove(side, speed, beatTime));
    }

    public void Explosion()
    {
        _Particles.Emit(10);
    }

    
    
    IEnumerator Start()
    {
        beatsRoot = JsonUtility.FromJson<beatProcessing.Beats>(beatMapFile.text);//get beatmap file and parse JSON to Unity
        yield return new WaitForSeconds(3);//delay to make sure everything loads properly
        
        _audioSource.Play();//play song
        float timer = Time.fixedTime;//start tracking the time where the song started
        
        //Iterate through every beat in the beatmap file
        foreach (beatProcessing.beat data in beatsRoot.Start)
        {
            var beatType = Enum.Parse(typeof(shrimpBeat.beatTypes), data.type);//parses string of the beat type into enum
            yield return new WaitUntil(() => Mathf.Abs(Time.fixedTime - timer) >= data.beatMoment);//wait until the timer goes to or beyond the moment the beat should happen
            
            //Decide which function with the beat type enum
            switch (beatType)
            {
                case shrimpBeat.beatTypes.Hold:
                    Hold(data.holdTime, data.pressTime, data.beatSpeed, data.side);
                    break;
                case shrimpBeat.beatTypes.tweenOff:
                    tweenOff(data.beatSpeed, data.side, data.beatTime);
                    break;
                case shrimpBeat.beatTypes.Bounce:
                    Bounce(data.side, data.beatSpeed);
                    break;
                case shrimpBeat.beatTypes.Explosion:
                    Explosion();
                    break;
                case shrimpBeat.beatTypes.Log:
                    print("'" + data.message + "', at: " + data.beatMoment);
                    break;
                case shrimpBeat.beatTypes.clearScreen:
                    clearScreen();
                    break;
                    
            }
        }
    }
    
    //Lock framerate when game starts (This was nabbed from a unity discussion post here: https://discussions.unity.com/t/how-to-limit-frame-rate-in-unity-editor/49059/3)
    void Awake () {
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 30;
    }
    
    //For developer debug purposes, just gives me the ability to trigger any function from any side with a key press. Not accessible in game

    #if UNITY_EDITOR
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            tweenOff(.7f, "Left", .5f);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            tweenOff(.7f, "Middle", .5f);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            tweenOff(.7f, "Right", .5f);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Hold(1.5f, .75f, .4f, "Left");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Hold(1.5f, .75f, .4f, "Right");
        }
        
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Bounce( "Left", 2);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Bounce( "Middle", 2);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Bounce( "Right", 2);
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Explosion(); 
        }
    }
    
    #endif
}
