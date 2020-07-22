using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject heartboss;
    public GameObject proj;
    public int spread;
    Vector3 perpDir;

    // Start is called before the first frame update
    void Start()
    {
        heartboss = GameObject.Find("HeartBoss");
        perpDir = heartboss.GetComponent<HeartAttacks>().perpDirection;
        StartCoroutine(OrbSpawner());
        Debug.Log("neg spread is" + -spread);
        Debug.Log("perDir is" + perpDir);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator OrbSpawner()
    {
        for (float i = -1 * spread; i <= spread; i+=5f)
        {
            Vector3 projLocation = transform.position + perpDir * i;
            Debug.Log("Location is" + projLocation);
            Debug.Log("i is" + i);

            var projectileSpot = Instantiate(proj, projLocation, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }
        
    }
}
