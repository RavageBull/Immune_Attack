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
        Vector3 perpDir = heartboss.GetComponent<HeartAttacks>().perpDirection;
        StartCoroutine(OrbSpawner());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator OrbSpawner()
    {
        for (int i = -spread; i <= spread; i+=5)
        {
            Vector3 projLocation = perpDir * i;
            var projectileSpot = Instantiate(proj, projLocation, Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
        }
        
    }
}
