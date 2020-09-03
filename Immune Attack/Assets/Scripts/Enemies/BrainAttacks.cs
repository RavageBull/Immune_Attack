using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainAttacks : MonoBehaviour
{

    //Boss strafe
    [Header("Strafe Settings")]
    public float thrust;
    public int strafeDir = 0;
    public float strafeTime = 5f;

    [Header("Attack Settings")]
    public int projectileQuantity;
    public float radius;
    public float projectileMoveSpeed;
    public float projectileMoveSpeed2;
    public float angle;
    public float startAngle;
    public GameObject projectile;
    public GameObject projectile2;
    public int burstAmount;
    public GameObject startPoint;
    public int spread;

    [Header("Spawning")]
    public float distance;
    public Vector3 perpDirection;
    public GameObject monsterSpawner;

    Stats stats;
    delegate void BeatDelegate();
    List<BeatDelegate> beatAttack = new List<BeatDelegate>();
    float beat;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Strafe());

        stats = GetComponent<Stats>();
        stats.maxHealth = 1400;
        stats.health = stats.maxHealth;
        stats.damage = 10;

        //BeatStuff
        beatAttack.Add(ProjectileRingHorizontal);
        beatAttack.Add(FloatingOrbs);
        beatAttack.Add(TargetedSingleShots);

        beat = 3f;
        StartCoroutine("BeatTimer");
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //ProjectileRingHorizontal();
        }
    }

    IEnumerator BeatTimer()
    {
        while (gameObject != null)
        {
            yield return new WaitForSeconds(beat);

            //animator.SetTrigger("Beat");

            int index = Random.Range(0, beatAttack.Count);
            beatAttack[index]();

            Debug.Log(index);
        }
    }

    void FloatingOrbs()
    {
        Vector3 projectileDirection = (GameManager.manager.player.transform.position - startPoint.transform.position).normalized;
        Vector3 projectileDirectionNeg = (GameManager.manager.player.transform.position - startPoint.transform.position).normalized * -1;
        Vector3 projectileLocation = projectileDirectionNeg * distance;
        perpDirection = Vector3.Cross(projectileDirectionNeg, Vector3.up).normalized;
        //Debug.Log("perpDirection is" + perpDirection);
        //Debug.Log("projectileDirectionNeg is " + projectileDirectionNeg);
        var proj = Instantiate(monsterSpawner, projectileLocation, Quaternion.identity);
        proj.GetComponent<MonsterSpawner>().perpDir = perpDirection;
    }


    IEnumerator Strafe()
    {
        var rb3 = GetComponent<Rigidbody>();
        yield return new WaitForSeconds(strafeTime);

        rb3.velocity = Vector3.zero;
        if (strafeDir % 2 == 0)
        {
            if (strafeDir == 0)
            {
                rb3.AddRelativeForce(Vector3.up * thrust / 2);
            }
            else
            {
                rb3.AddRelativeForce(Vector3.up * thrust);
            }
        }
        else if (strafeDir % 2 != 0)
        {
            rb3.AddRelativeForce(Vector3.down * thrust);
        }

        strafeDir++;
        //Debug.Log("strafeDir is" + strafeDir);
        StartCoroutine(Strafe());
    }

    void TargetedSingleShots()
    {
        StartCoroutine(TargetedSingleShotsTimer());
    }

    IEnumerator TargetedSingleShotsTimer()
    {
        //Do this attack once after a while

        for (int j = 0; j <= 2; j++)
        {
            for (int i = 0; i <= burstAmount; i++)
            {
                Vector3 projectileDirection = (GameManager.manager.player.transform.position - startPoint.transform.position).normalized * projectileMoveSpeed2;
                var proj = Instantiate(projectile2, startPoint.transform.position, Quaternion.identity);
                proj.GetComponent<Projectile>().damage = stats.damage;
                Destroy(proj.gameObject, 4);
                proj.GetComponent<Rigidbody>().velocity = new Vector3(projectileDirection.x + Random.Range(-spread, spread), projectileDirection.y + Random.Range(-spread, spread), projectileDirection.z + Random.Range(-spread, spread));
            }
            yield return new WaitForSeconds(0.33f);
        }

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
            startPoints.Add(new Vector3(transform.position.x, 4, transform.position.z));
            startPoints.Add(new Vector3(transform.position.x, 8, transform.position.z));
            startPoints.Add(new Vector3(transform.position.x, 12, transform.position.z));

            int x = Random.Range(0, startPoints.Count);
            Vector3 startPointH = startPoints[x];

            //Shoots horizontally at 3 differnet heights;
            //Do this whole attack at random heights X times
            float angleStep = 360f / projectileQuantity;

            angle = 0f;

            for (int i = 0; i <= projectileQuantity - 1; i++)
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
            yield return new WaitForSeconds(0.25f);
        }
    }
}
