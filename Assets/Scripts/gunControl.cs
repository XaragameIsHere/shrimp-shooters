using UnityEngine;

public class gunControl : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    [SerializeField] LayerMask enemy;
    [SerializeField] float weaponRange = 10000;
    float cameraSensitivity = 2;
    [SerializeField] Texture2D crosshair;
    private float timePressed = 0.0f;
    private float timeLastPress = 0.0f;
    public  float timeDelayThreshold = 1.0f;


    
    private void Start()
    {
        Cursor.SetCursor(crosshair, Vector2.zero, CursorMode.Auto);

    }

    void shoot()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.mousePosition.x/Screen.width;
        //float inputY = Input.GetAxis("Mouse Y") * cameraSensitivity;
        transform.localEulerAngles = new Vector3(11, (inputX * 180)+90, 0);

        /*if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began) { // If the user puts her finger on screen...
                timePressed = Time.time - timeLastPress;
            }*/

            if (Input.GetButton("Fire1")) { // If the user raises her finger from screen
                timeLastPress = Time.time;
                //if (timePressed > timeDelayThreshold) { // Is the time pressed greater than our time delay threshold?
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, weaponRange, enemy))
                    {
                        hit.transform.gameObject.GetComponent<shrimpBeat>().held = true;
                    }
                //}
            }
        //}
        
    }
}
