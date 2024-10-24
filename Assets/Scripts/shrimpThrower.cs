using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shrimpThrower : MonoBehaviour
{
    [SerializeField] GameObject shrimp;
    private List<GameObject> shrimpList = new List<GameObject>();

    // Start is called before the first frame update
    IEnumerator spawn()
    {
        for (int i = 0; i < 10; i++)
        {
            var newShrimp = Instantiate(shrimp);
            shrimpList.Add(newShrimp);
            newShrimp.transform.position = transform.position;
            newShrimp.transform.localEulerAngles = new Vector3(0, 90, 0);
            var rb = newShrimp.gameObject.AddComponent<Rigidbody>();
            rb.AddForce(new Vector3(Random.Range(-200, 200), 650));
            yield return new WaitForSeconds(Random.Range(.25f, .75f));
        }

        yield return new WaitForSeconds(3);

        foreach (var item in shrimpList)
        {
            Destroy(item.gameObject);
        }
    }

    public void krill(GameObject shrimp)
    {
        Destroy(shrimp);
    }

    void Start()
    {
        StartCoroutine(spawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
