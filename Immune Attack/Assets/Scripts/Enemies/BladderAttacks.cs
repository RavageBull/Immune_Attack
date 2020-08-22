using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BladderAttacks : MonoBehaviour
{

    //Reflection Raycast stuff
    [Header("Reflection Settings")]
    public int maxReflectionCount = 5;
    public float maxStepDistance = 200;
    public GameObject lineRenderer;
    public LineRenderer lr;
    public GameObject lazerStart;
    public int lrPos = 0;
    //Attack Stats
    [Header("Attack Settings")]
    public int meteorCount = 20;
    public float meteorSpeed;
    public GameObject Meteor;
    public GameObject BladderBoss;
    public GameObject HomingProjectile;

    public GameObject Geyser;
    public int geyserNumber;
    public float geyserSpeed;

    public GameObject PissBeam;
    public float pissSpeed;
    public int pissNumber;
    //public ParticleSystem geyserParticle;

    //Rotation of Boss
    [Header("Rotation Settings")]
    [SerializeField] [Range(0f, 5f)] float lerpTime2;
    [SerializeField] Vector3[] myAngles;
    int angleIndex;
    public int len;
    float t = 0f;
    int x = 0;

    //Lerp of Rising floor/Boss
    [Header("Lerp Settings")]
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float distance = 30f;
    public float lerpTime = 5f;
    private float currentLerpTime = 0;

    //Boss strafe
    [Header("Strage Settings")]
    public float thrust;
    public int strafeDir = 0;
    public float strafeTime = 5f;

    //Beat Settings AI
    Stats stats;
    float beat;
    delegate void BeatDelegate();
    List<BeatDelegate> beatAttack = new List<BeatDelegate>();

    // Start is called before the first frame update
    void Start()
    {


        //Stats
        stats = GetComponent<Stats>();
        stats.maxHealth = 750;
        stats.health = stats.maxHealth;
        stats.damage = 10;

        //Strafe initiated
        StartCoroutine(Strafe());
        lr = lineRenderer.GetComponent<LineRenderer>();
        lr.enabled = false;
        lr.useWorldSpace = true;

        //Rotation stuff
        len = myAngles.Length;

        //LerpStuff
        startPosition = transform.position;
        endPosition = transform.position + Vector3.up * distance;

        //BeatStuff
        beatAttack.Add(ConstantMeteorShower);
        beatAttack.Add(MeteorShower);
        beatAttack.Add(Geysers);
        beatAttack.Add(PissBeamAttack);

        beat = 3f;
        StartCoroutine("BeatTimer");
    }

    // Update is called once per frame
    void Update()
    {
        //Line renderer stuff
        lr.SetPosition(0, lazerStart.transform.position);
        //Testing
        if (Input.GetMouseButtonDown(0))
        {
            //PissBeamAttack();
            //if we want ot use line renederer
            //lr.enabled = true;
        }

        //Rotation stuff
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(myAngles[angleIndex]), lerpTime2 * Time.deltaTime);
        t = Mathf.Lerp(t, 1f, lerpTime2 * Time.deltaTime);
        if (t>.9f)
        {
            x++;
            t = 0f;
            if (x%2==0)
            {
                angleIndex = 0;
            }
            else if (x%2 != 0)
            {
                angleIndex = 1;
            }
        }

        //LerpStuff      
        currentLerpTime += Time.deltaTime;
        if(currentLerpTime >= lerpTime)
        {
            currentLerpTime = lerpTime;
        }

        float Perc = currentLerpTime / lerpTime;
        //Lerps in y direction Only
        transform.position = Vector3.Lerp(startPosition, endPosition, Perc);
        
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

    void MeteorShower()
    {
        StartCoroutine(MeteorShowerC());
    }

    IEnumerator MeteorShowerC()
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

    void ConstantMeteorShower()
    {
        StartCoroutine(ConstantMeteorShowerC());
    }

    IEnumerator ConstantMeteorShowerC()
    {
        for (int i = 0; i <= meteorCount*2; i++)
        {

            var projectile = Instantiate(Meteor, transform.position + new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)), Quaternion.identity);
            var rb = projectile.GetComponent<Rigidbody>();
            rb.AddForce(transform.up * meteorSpeed);
            Destroy(projectile, 2f);
            //yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(3f);

        for (int j = 0; j <= meteorCount*2; j++)
        {
            var projectile2 = Instantiate(Meteor, GameManager.manager.player.transform.position + new Vector3(Random.Range(-40f, 40f), 100f, Random.Range(-40f, 40f)), Quaternion.identity);
            var rb2 = projectile2.GetComponent<Rigidbody>();
            rb2.AddForce(transform.up * -meteorSpeed);
            Destroy(projectile2, 2f);
        }

        yield return null;
        //Can make this happen continuously by writing in StartCoroutine(ConstantMeteorShower()); 
        //here and also in start()
    }

    void Geysers()
    {
        StartCoroutine(GeysersC());
    }

    IEnumerator GeysersC()
    {

        //PLAY PARTICLE EFFECT
        var geyserSpot = GameManager.manager.player.transform.position;
        //geyserParticle.Play();
        yield return new WaitForSeconds(2f);

        for (int i = 0; i <= geyserNumber; i++)
        {
            var geyser = Instantiate(Geyser, geyserSpot + new Vector3(Random.Range(0, 5), -10, Random.Range(0, 5)), Quaternion.identity);
            var rb5 = geyser.GetComponent<Rigidbody>();
            rb5.AddForce(transform.up * geyserSpeed);
            Destroy(geyser, 5f);
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    void PissBeamAttack()
    {
        StartCoroutine(PissBeamAttackC());
    }

    IEnumerator PissBeamAttackC()
    {
        for (int i = 0; i <= pissNumber; i++)
        {
            var pissBeam = Instantiate(PissBeam, lazerStart.transform.position, Quaternion.identity);
            var rb6 = pissBeam.GetComponent<Rigidbody>();
            rb6.AddForce(transform.forward * pissSpeed);
            Destroy(pissBeam, 5f);
            yield return new WaitForSeconds(0.05f);
        }
        yield return null;
    }

    //Left and right Movement
    //THIS DOENST WORK COZ IM LERPING MOVEMENT.
    IEnumerator Strafe()
    {

        var rb3 = GetComponent<Rigidbody>();
        yield return new WaitForSeconds(strafeTime);

        if (strafeDir % 2 == 0)
        {
            if (strafeDir == 0)
            {
                rb3.AddRelativeForce(Vector3.left * thrust/2);
            }

            rb3.AddRelativeForce(Vector3.left * thrust);
        }
        else if (strafeDir % 2 != 0)
        {
            rb3.AddRelativeForce(Vector3.right * thrust);
        }

       
        strafeDir++;
        Debug.Log("strafeDir is" + strafeDir);
        StartCoroutine(Strafe());
    }


    /// <summary>
    /// Raycast Bouncing stuff
    /// </summary>
    void OnDrawGizmos()
    {      
        Handles.color = Color.red;
        Handles.ArrowHandleCap(0, lazerStart.transform.position + lazerStart.transform.forward * 0.25f, lazerStart.transform.rotation, 0.5f, EventType.Repaint);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(lazerStart.transform.position, 0.25f);

        DrawPredictedReflectionPattern(lazerStart.transform.position + lazerStart.transform.forward * 0.75f, lazerStart.transform.forward, maxReflectionCount);
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
