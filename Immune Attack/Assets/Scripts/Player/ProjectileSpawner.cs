using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject heartboss;
    public GameObject proj;
    public int spread;
    
    public Vector3 perpDir;

    // Start is called before the first frame update
    void Start()
    {
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
        for (float i = -1 * spread; i <= spread; i+=10f)
        {
           
            Vector3 projLocation = transform.position + perpDir * i;
            Vector3 projLocation2 = transform.position + perpDir * i + new Vector3(0, Random.Range(0, 20), 0);
            //Debug.Log("Location is" + projLocation2);
            //Debug.Log("i is" + i);

            var projectileSpot = Instantiate(proj, projLocation2, Quaternion.identity);
            Destroy(projectileSpot.gameObject, 4);
            yield return new WaitForSeconds(0.1f);
        }
        
    }
}
