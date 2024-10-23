using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shrimpThrower : MonoBehaviour
{
    [SerializeField] GameObject shrimp;
    // Start is called before the first frame update
    void Start()
    {
        var newShrimp = Instantiate(shrimp);
        newShrimp.gameObject.transform.Position = transform.Position;
        newShrimp.gameObject.AddComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
