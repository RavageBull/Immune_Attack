using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartAttacks : MonoBehaviour
{
    public int projectileQuantity;
    public int projectileQuantity2;
    public int projectileQuantity3;

    public GameObject projectile;
    public GameObject projectile2;
    public Vector3 startPoint;
    public Vector3 startPointHigh;
    public Vector3 startPointLow;

    public float radius;
    public float projectileMoveSpeed;
    public float angle;
    public float startAngle;

    public int burstAmount;
    public float distance;
    public Vector3 perpDirection;

    Stats stats;
    Animator animator;

    float beat;

    delegate void BeatDelegate();
    List<BeatDelegate> beatAttack = new List<BeatDelegate>();

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<Stats>();
        stats.maxHealth = 1400;
        stats.health = stats.maxHealth;
        stats.damage = 10;

        animator = GetComponent<Animator>();

        angle = 0f;
        //radius = 10f;
        //projectileMoveSpeed = 2f;

        beatAttack.Add(ProjectileRingHorizontal);
        beatAttack.Add(ProjectileRingSpinning);
        beatAttack.Add(TargetedSingleShots);
        beatAttack.Add(FloatingOrbs);

        beat = 3f;
        StartCoroutine("BeatTimer");
    }

    IEnumerator BeatTimer()
    {
        while (gameObject != null)
        {
            yield return new WaitForSeconds(beat);

            animator.SetTrigger("Beat");

            int index = Random.Range(0, beatAttack.Count);
            beatAttack[index]();

            Debug.Log(index);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {

            StartCoroutine(ProjectileRingSpinningC());
            /*int height = Random.Range(1,4);
            FloatingOrbs();

             if (height == 1)
             {
                 //ProjectileRingTest(projectileQuantity, startPointHigh, 0);
                 //StartCoroutine(FloatingOrbs());

             }
             else if (height == 2)
             {
                 //ProjectileRingTest(projectileQuantity, startPoint, 0);
                 //StartCoroutine(FloatingOrbs());
             }

             else if (height == 3)
             {
                 //ProjectileRingTest(projectileQuantity, startPointLow, 0);
                 //StartCoroutine(FloatingOrbs());
             } 
             */

        }

        //type 1 = vertical shots
        //this is just to test out the boss attack functions
    }

    void ProjectileRingHorizontal()
    {
        StartCoroutine(ProjectileRingHorizontalC());
    }

    IEnumerator ProjectileRingHorizontalC()
    {
        for (int j = 0; j <= 6; j++)
        {
            List<Vector3> startPoints = new List<Vector3>();
            startPoints.Add(new Vector3(0, 4, 0));
            startPoints.Add(new Vector3(0, 8, 0));
            startPoints.Add(new Vector3(0, 12, 0));

            int x = Random.Range(0, startPoints.Count);
            Vector3 startPointH = startPoints[x];

            //Shoots horizontally at 3 differnet heights;
            //Do this whole attack at random heights X times
            float angleStep = 360f / projectileQuantity3;

            angle = 0f;

            for (int i = 0; i <= projectileQuantity3 - 1; i++)
            {
                float projectileX = startPointH.x + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
                //float projectileY = newStartpoint.y + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
                float projectileY = startPointH.y;
                float projectileZ = startPointH.z + Mathf.Cos((angle * Mathf.PI) / 180) * radius;

                Vector3 projectileVector = new Vector3(projectileX, projectileY, projectileZ);
                Vector3 projectileDirection = (projectileVector - startPointH).normalized * projectileMoveSpeed;

                var proj = Instantiate(projectile, startPointH, Quaternion.identity);
                proj.GetComponent<Projectile>().damage = stats.damage;
                Destroy(proj.gameObject, 4);
                proj.GetComponent<Rigidbody>().velocity = new Vector3(projectileDirection.x, projectileDirection.y, projectileDirection.z);
                angle += angleStep;
            }
            yield return new WaitForSeconds(0.75f);
        }
    }

    void ProjectileRingSpinning()
    {
        StartCoroutine(ProjectileRingSpinningC());
    }

    IEnumerator ProjectileRingSpinningC()
    {
        //projectileQuantity = ;
        //startPoint = ;
        //int shootType = Random.Range(0, 2);

        //Dp this attack X times back to back
        //Shoots horizontally at 3 differnet heights, depending on the type it uses a different projectic
        Vector3 startPointring = new Vector3(0, 3, 0);
        for (int j = 0; j <= 100; j++)
        {
            float angleStep = 360f / projectileQuantity2;
            //int type = shootType;

            if (startAngle < 360f)
            {
                startAngle += 5;
            }
            else
            {
                startAngle = 0f;
            }

            angle = 0f;
            angle += startAngle;

            for (int i = 0; i <= projectileQuantity - 1; i++)
            {
                float projectileX = startPointring.x + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
                //float projectileY = newStartpoint.y + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
                float projectileY = startPointring.y;
                float projectileZ = startPointring.z + Mathf.Cos((angle * Mathf.PI) / 180) * radius;

                Vector3 projectileVector = new Vector3(projectileX, projectileY, projectileZ);
                Vector3 projectileDirection = (projectileVector - startPointring).normalized * projectileMoveSpeed;

                var proj = Instantiate(projectile, startPointring, Quaternion.identity);
                proj.GetComponent<Projectile>().damage = stats.damage;
                Destroy(proj.gameObject, 4);
                proj.GetComponent<Rigidbody>().velocity = new Vector3(projectileDirection.x, projectileDirection.y, projectileDirection.z);

                angle += angleStep;
            }
            yield return new WaitForSeconds(0.1f);
        }
   
    }

    void TargetedSingleShots()
    {
        StartCoroutine(TargetedSingleShotsTimer());
    }

    IEnumerator TargetedSingleShotsTimer()
    {
        //Do this attack once after a while
        for (int i = 0; i <= burstAmount; i++)
        {
            Vector3 projectileDirection = (GameManager.manager.player.transform.position - startPoint).normalized * projectileMoveSpeed;
            var proj = Instantiate(projectile, startPoint, Quaternion.identity);
            proj.GetComponent<Projectile>().damage = stats.damage;
            Destroy(proj.gameObject, 4);
            proj.GetComponent<Rigidbody>().velocity = new Vector3(projectileDirection.x, projectileDirection.y, projectileDirection.z);
            yield return new WaitForSeconds(0.1f);

        }

    }

    void FloatingOrbs()
    {
        Vector3 projectileDirection = (GameManager.manager.player.transform.position - startPoint).normalized;
        Vector3 projectileDirectionNeg = (GameManager.manager.player.transform.position - startPoint).normalized * -1;
        Vector3 projectileLocation = projectileDirectionNeg * distance;
        perpDirection = Vector3.Cross(projectileDirectionNeg, Vector3 .up).normalized;
        //Debug.Log("perpDirection is" + perpDirection);
        //Debug.Log("projectileDirectionNeg is " + projectileDirectionNeg);
        var proj = Instantiate(projectile2, projectileLocation, Quaternion.identity);
        proj.GetComponent<ProjectileSpawner>().perpDir = perpDirection;
    }
}
