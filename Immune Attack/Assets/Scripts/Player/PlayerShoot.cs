using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    //Hit Enemy//
    public GameObject EnemyObject;

    //Base Gun Stats//
    public int bulletsMag = 30;
    public int bulletsTotal = 300;
    public float gunfireRate = 0.1f;
    public float fireTimer;
    public int gunATK = 10;
    public float gunRange = 100f;
    public Transform startingshootPoint;
    public int currentBullets;

    private AudioSource _AudioSource;
    public AudioClip shootSound;

    public GameObject bulletHole;
    [SerializeField] GameObject spark; //temp
    [SerializeField] Camera cam; //temp

    //an event that fires when the player shoots so the UI manager can see it and animate the gun
    //should maybe tie the gun image to player instead of UI. Will look at later.
    public delegate void ShootAniDelegate();
    public static ShootAniDelegate ShootAni;

    // Start is called before the first frame update
    void Start()
    {
        currentBullets = bulletsMag;
        _AudioSource = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("r"))
        {
            Reload();
        }

        if(Input.GetMouseButton(0))
        {
            if (currentBullets > 0)
            {
                Fire();
            }
            else if (currentBullets == 0)
            {
                Reload();
            }
        }

        if(fireTimer < gunfireRate)
        {
            fireTimer += Time.deltaTime;
        }

    }

    private void Fire()
    {
        if(fireTimer < gunfireRate || currentBullets <= 0)
        {
            return;
        }

        RaycastHit hit;

        //if (Physics.Raycast(startingshootPoint.position, startingshootPoint.transform.forward, out hit, gunRange))
        if (Physics.Raycast(startingshootPoint.position, startingshootPoint.transform.forward, out hit, gunRange))
        {
            Debug.Log(hit.transform.name + " was hit!");

            GameObject particle = Instantiate(spark, hit.point, Quaternion.identity);
            Destroy(particle, 1);

            if (hit.transform.tag != "Enemy")
            {
                GameObject hitObject = Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                hitObject.transform.parent = hit.transform;
                Destroy(hitObject, 2f);

            }

            particle.transform.LookAt(gameObject.transform);
            particle.transform.Translate(new Vector3(0, 0, 0.5f));


            if (hit.transform.tag == "Enemy")
            {
                EnemyObject = hit.collider.gameObject;
                DealDamage();
            }

            if (hit.collider.gameObject.GetComponent<Destructible>())
            {
                hit.collider.gameObject.GetComponent<Destructible>().TakeDamage();
            }
           
        }

        ShootAni();
        PlayShootSound();
        currentBullets--;
        fireTimer = 0.0f;
    }

    private void PlayShootSound()
    {
        _AudioSource.clip = shootSound;
        _AudioSource.Play();
    }

    private void DealDamage()
    {
        EnemyObject.GetComponent<Enemy>().TakeDamage(gunATK);
    }

    private void Reload()
    {
        if(bulletsTotal <= 0)
        {
            return;
        }

        int bulletsRequired = bulletsMag - currentBullets;
        int bulletsTaken;
        if (bulletsTotal >= bulletsRequired)
        {
            bulletsTaken = bulletsRequired;
        }
        else
        {
            bulletsTaken = bulletsTotal;
        }

        bulletsTotal -= bulletsTaken;
        currentBullets += bulletsTaken;
    }

}