using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject heartboss;
    public GameObject proj;
    public int spread;
    public Vector3 perpDir;
    public GameObject mon1;
    public GameObject mon2;
    public GameObject mon3;
    public GameObject mon4;
    public GameObject mon5;
    public GameObject mon6;
    List<GameObject> unityGameObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
       
        unityGameObjects.Add(mon1);
        unityGameObjects.Add(mon2);
        unityGameObjects.Add(mon3);
        unityGameObjects.Add(mon4);
        unityGameObjects.Add(mon5);


        StartCoroutine(OrbSpawner());
        //Debug.Log("neg spread is" + -spread);
        //Debug.Log("perDir is" + perpDir);
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator OrbSpawner()
    {
        for (float i = -1 * spread; i <= spread; i += 10f)
        {

            int index = Random.Range(0, unityGameObjects.Count);
            proj = unityGameObjects[index];

            Vector3 projLocation = transform.position + perpDir * i;
            Vector3 projLocation2 = transform.position + perpDir * i + new Vector3(Random.Range(0,10), Random.Range(0, 10), Random.Range(0, 10));
            //Debug.Log("Location is" + projLocation2)
            //Debug.Log("i is" + i);

            var projectileSpot = Instantiate(proj, projLocation2, Quaternion.identity);
            yield return new WaitForSeconds(1f);
        }

    }
}
