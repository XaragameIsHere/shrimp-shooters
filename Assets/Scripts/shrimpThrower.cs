using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shrimpThrower : MonoBehaviour
{ 
    [SerializeField] GameObject shrimp;
    [SerializeField] Transform spawnPointRight;
    [SerializeField] Transform spawnPointMid;
    [SerializeField] Transform spawnPointLeft;
    [SerializeField] TextAsset jsonFile;
    [HideInInspector] public beatProcessing.Beats beatsRoot;

    int bpm = 150;
    private List<GameObject> shrimpList = new List<GameObject>();

    // Start is called before the first frame update
    
    IEnumerator midDelete(GameObject srimp)
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(srimp);
    }

    IEnumerator beat(float beatTime, float speed, string side)
    {
        var beatCount = Mathf.CeilToInt(bpm * (beatTime / 60));

        

        for (int i = 0;i < beatCount;i++)
        {
            var newShrimp = Instantiate(shrimp);
            var dir = 1;
            shrimpList.Add(newShrimp);

            if (side == "left")
            {
                newShrimp.transform.position = spawnPointLeft.position;
                dir = 1;
            }
            else if (side == "right") 
            {
                newShrimp.transform.position = spawnPointRight.position;
                dir = -1;
            }
            else if (side == "mid")
            {
                newShrimp.transform.position = spawnPointMid.position;
                dir = 0;
                StartCoroutine(midDelete(newShrimp));
            }

            newShrimp.transform.localEulerAngles = new Vector3(0, 90, 0);
            var rb = newShrimp.gameObject.GetComponent<Rigidbody>();
            rb.AddTorque(new Vector3(Random.Range(-60, 60), Random.Range(-60, 60), Random.Range(-60, 60)));
            rb.AddForce(new Vector3(Random.Range(dir * 400, dir * 300) * speed, 1500 * speed));
            yield return new WaitForSeconds(beatTime/beatCount);
        }
    }

    public void krill(GameObject shrimp)
    {
        Destroy(shrimp);
    }

    IEnumerator processBeats()
    {
        foreach (beatProcessing.beat data in beatsRoot.Start)
        {
            print(data.beatMoment);
            yield return new WaitUntil(() => Time.fixedTime >= data.beatMoment);
            StartCoroutine(beat(data.beatTime, data.beatSpeed, data.side));
        }
    }

    void Start()
    {
        beatsRoot = JsonUtility.FromJson<beatProcessing.Beats>(jsonFile.text);
        StartCoroutine(processBeats());

    }

}
