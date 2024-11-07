using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class shrimpThrower : MonoBehaviour
{ 
    [SerializeField] GameObject shrimp;
    [SerializeField] Transform spawnPointRight;
    [SerializeField] Transform spawnPointMid;
    [SerializeField] Transform spawnPointLeft;
    [SerializeField] TextAsset jsonFile;
    [SerializeField] TMP_Text ScoreText;
    private float shrimpThrown = 0;
    private float shrimpShot = 0;
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
            shrimpThrown += 1;
            var newShrimp = Instantiate(shrimp);
            var dir = 1;
            var downDir = 1;
            shrimpList.Add(newShrimp);

            if (side == "left")
            {
                newShrimp.transform.position = spawnPointLeft.position;
                dir = 1;
                downDir = 1;
            }
            else if (side == "right") 
            {
                newShrimp.transform.position = spawnPointRight.position;
                dir = -1;
                downDir = 1;
            }
            else if (side == "mid")
            {
                newShrimp.transform.position = spawnPointMid.position;
                dir = 0;
                downDir = -1;
                //StartCoroutine(midDelete(newShrimp));
            }

            newShrimp.transform.localEulerAngles = new Vector3(0, 90, 0);
            var rb = newShrimp.gameObject.GetComponent<Rigidbody>();
            rb.AddTorque(new Vector3(Random.Range(-60, 60), Random.Range(-60, 60), Random.Range(-60, 60)));
            rb.AddForce(new Vector3(Random.Range(500, 300) * dir, 1500 * speed * downDir));
            yield return new WaitForSeconds(beatTime/beatCount);
        }
    }

    public void krill(GameObject shrimp)
    {
        shrimpShot += 1;
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

    void Update()
    {
        ScoreText.text = "Accuracy: " + Mathf.CeilToInt((shrimpShot/shrimpThrown)*100).ToString() + "%";
    }
}
