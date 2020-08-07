using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BladderAttacks : MonoBehaviour
{
   
    /// Reflection Raycast stuff
    public int maxReflectionCount = 5;
    public float maxStepDistance = 200;
    public GameObject lineRenderer;
    public LineRenderer lr;
    public GameObject lazerStart;
    public int lrPos = 0;
    //Attack Stats
    public int meteorCount = 20;
    public float meteorSpeed;
    public GameObject Meteor;
    public GameObject BladderBoss;
    public GameObject HomingProjectile;


    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lineRenderer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(MeteorShower());
            lineRenderer.SetActive(true);
        }
    }

    IEnumerator MeteorShower()
    {
        for (int i = 0; i <= meteorCount; i++)
        {

            var projectile = Instantiate(Meteor, transform.position + new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f)), Quaternion.identity);
            var rb = projectile.GetComponent<Rigidbody>();
            rb.AddForce(transform.up * meteorSpeed);
            Destroy(projectile, 2f);
            yield return new WaitForSeconds(0.1f);
        }

        for (int j = 0; j <= meteorCount; j++)
        {
            //+new Vector3(Random.Range(0.25f, 5f), 100f, Random.Range(0.25f, 5f))
            var projectile2 = Instantiate(Meteor, GameManager.manager.player.transform.position + new Vector3(Random.Range(-10f, 10f), 100, Random.Range(-10f, 10f)), Quaternion.identity);
            var rb2 = projectile2.GetComponent<Rigidbody>();
            rb2.AddForce(transform.up * -meteorSpeed);
            //Destroy(projectile2, 2f);
        }



        yield return null;
    }


    IEnumerator ConstantMeteorShower()
    {
        for (int i = 0; i <= meteorCount*2; i++)
        {

            var projectile = Instantiate(Meteor, transform.position, Quaternion.identity);
            var rb = projectile.GetComponent<Rigidbody>();
            rb.AddForce(transform.up * meteorSpeed);
            Destroy(projectile, 2f);
            yield return new WaitForSeconds(0.1f);
        }

        for (int j = 0; j <= meteorCount*2; j++)
        {
            var projectile2 = Instantiate(HomingProjectile, GameManager.manager.player.transform.position + new Vector3(Random.Range(1f, 30f), 100f, Random.Range(1f, 30f)), Quaternion.identity);
            var rb2 = projectile2.GetComponent<Rigidbody>();
            rb2.AddForce(transform.up * -meteorSpeed);
            Destroy(projectile2, 2f);
        }





        yield return null;
    }




























    /// <summary>
    /// Raycast Bouncing stuff
    /// </summary>
    void OnDrawGizmos()
    {      
        Handles.color = Color.red;
        Handles.ArrowHandleCap(0, this.transform.position + this.transform.forward * 0.25f, this.transform.rotation, 0.5f, EventType.Repaint);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, 0.25f);

        DrawPredictedReflectionPattern(this.transform.position + this.transform.position + this.transform.forward * 0.75f, this.transform.forward, maxReflectionCount);
    }


    private void DrawPredictedReflectionPattern(Vector3  position, Vector3 direction, int reflectionsRemaining)
    {
        if (reflectionsRemaining == 0)
        {
            lrPos = 0;
            return;
        }

        lrPos++;

        Vector3 startingPosition = position;
        Ray ray = new Ray(position, direction);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, maxStepDistance))
        {
            direction = Vector3.Reflect(direction, hit.normal);
            position = hit.point;
            lr.SetPosition(lrPos, hit.point);
        }
        else
        {
            position += direction * maxStepDistance;
        }
     
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(startingPosition, position);

       
        DrawPredictedReflectionPattern(position, direction, reflectionsRemaining - 1);
    }
}
