using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{

    //Base Gun Stats//
    public int bulletsMag = 30;
    public int bulletsTotal = 300;
    public float gunfireRate = 0.1f;
    public float fireTimer;
    public float gunRange = 100f;
    public Transform startingshootPoint;
    public int currentBullets;

    private AudioSource _AudioSource;
    public AudioClip shootSound;

    [SerializeField] GameObject spark; //temp
    [SerializeField] Camera cam; //temp


    // Start is called before the first frame update
    void Start()
    {
        currentBullets = bulletsMag;
        _AudioSource = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        
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
            particle.transform.LookAt(gameObject.transform);
            Destroy(particle, 1);
        }

        PlayShootSound();

        currentBullets--;
        fireTimer = 0.0f;
    }

    private void PlayShootSound()
    {
        _AudioSource.clip = shootSound;
        _AudioSource.Play();
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