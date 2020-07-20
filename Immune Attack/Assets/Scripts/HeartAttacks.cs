using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartAttacks : MonoBehaviour
{
    [SerializeField]
    int projectileQuantity;

    [SerializeField]
    public GameObject projectile;
    public GameObject projectile2;
    public Vector3 startPoint;
    public Vector3 startPointHigh;
    public Vector3 startPointLow;

    public float radius;
    public float projectileMoveSpeed;
    public float angle;
    public float startAngle;

    [SerializeField]
    public GameObject player1;
    public int burstAmount;


    // Start is called before the first frame update
    void Start()
    {
        angle = 0f;
        //radius = 10f;
        //projectileMoveSpeed = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            int height = Random.Range(1,4);

            if (height == 1)
            {
                //ProjectileRingTest(projectileQuantity, startPointHigh, 0);
                StartCoroutine(TargetedSingleShots());

            }
            else if (height == 2)
            {
                //ProjectileRingTest(projectileQuantity, startPoint, 0);
                StartCoroutine(TargetedSingleShots());
            }
            
            else if (height == 3)
            {
                //ProjectileRingTest(projectileQuantity, startPointLow, 0);
                StartCoroutine(TargetedSingleShots());
            }
      
            
        }

        //type 1 = vertical shots
        //this is just to test out the boss attack functions
    }



    void ProjectileRingHorizontal(int projectileQuantity, Vector3 startpoint, int shootType)
    {
        //Shoots horizontally at 3 differnet heights, depending on the type it uses a different projectile
        float angleStep = 360f / projectileQuantity;
        int type = shootType;

        angle = 0f;

        for (int i = 0; i<= projectileQuantity -1; i++)
        {
            float projectileX = startpoint.x + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
            //float projectileY = newStartpoint.y + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
            float projectileY = startpoint.y;
            float projectileZ = startpoint.z + Mathf.Cos((angle * Mathf.PI) / 180) * radius;

            Vector3 projectileVector = new Vector3(projectileX, projectileY, projectileZ);
            Vector3 projectileDirection = (projectileVector - startpoint).normalized * projectileMoveSpeed;

            if (type == 1)
            {
                var proj = Instantiate(projectile2, startpoint, Quaternion.identity);
                proj.GetComponent<Rigidbody>().velocity = new Vector3(projectileDirection.x, projectileDirection.y, projectileDirection.z);

            }
            else
            {
                var proj = Instantiate(projectile, startpoint, Quaternion.identity);
                proj.GetComponent<Rigidbody>().velocity = new Vector3(projectileDirection.x, projectileDirection.y, projectileDirection.z);
            }

            angle += angleStep;   
        }
    }



    void ProjectileRingTest(int projectileQuantity, Vector3 startpoint, int shootType)
    {
        //Shoots horizontally at 3 differnet heights, depending on the type it uses a different projectile
        float angleStep = 360f / projectileQuantity;
        int type = shootType;

        if (startAngle < 360f)
        {
            startAngle += 30;
        }
        else
        {
            startAngle = 0f;
        }

        angle = 0f;
        angle += startAngle;

        for (int i = 0; i <= projectileQuantity - 1; i++)
        {
            float projectileX = startpoint.x + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
            //float projectileY = newStartpoint.y + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
            float projectileY = startpoint.y;
            float projectileZ = startpoint.z + Mathf.Cos((angle * Mathf.PI) / 180) * radius;

            Vector3 projectileVector = new Vector3(projectileX, projectileY, projectileZ);
            Vector3 projectileDirection = (projectileVector - startpoint).normalized * projectileMoveSpeed;

            if (type == 1)
            {
                var proj = Instantiate(projectile2, startpoint, Quaternion.identity);
                proj.GetComponent<Rigidbody>().velocity = new Vector3(projectileDirection.x, projectileDirection.y, projectileDirection.z);

            }
            else
            {
                var proj = Instantiate(projectile, startpoint, Quaternion.identity);
                proj.GetComponent<Rigidbody>().velocity = new Vector3(projectileDirection.x, projectileDirection.y, projectileDirection.z);
            }

            angle += angleStep;
        }
    }

    IEnumerator TargetedSingleShots()
    {
        
        for (int i = 0; i <= burstAmount; i++)
        {
            Vector3 projectileDirection = (player1.transform.position - transform.position).normalized * projectileMoveSpeed;
            var proj = Instantiate(projectile, startPoint, Quaternion.identity);
            proj.GetComponent<Rigidbody>().velocity = new Vector3(projectileDirection.x, projectileDirection.y, projectileDirection.z);
            yield return new WaitForSeconds(0.2f);

        }

    }
}
