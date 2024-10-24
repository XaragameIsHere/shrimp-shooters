using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunControl : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    [SerializeField] LayerMask enemy;
    [SerializeField] shrimpThrower spawner;
    [SerializeField] float weaponRange = 10000;
    float cameraSensitivity = 2;
    [SerializeField] Texture2D crosshair;

    private void Start()
    {
        Cursor.SetCursor(crosshair, Vector2.zero, CursorMode.Auto);

    }

    void shoot()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, weaponRange, enemy))
        {
            spawner.krill(hit.transform.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.mousePosition.x/Screen.width;
        //float inputY = Input.GetAxis("Mouse Y") * cameraSensitivity;
        transform.localEulerAngles = new Vector3(11, (inputX * 180)+90, 0);

        if (Input.GetButtonDown("Fire1"))
            shoot();
    }
}
