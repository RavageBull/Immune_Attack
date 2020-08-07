using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerups : MonoBehaviour
{

    public GameObject powerupParticle;
    public GameObject Player1;
    public Player stats1;
    public PlayerController stats2;
    public PlayerShoot stats3;
    //public FirstPersonController stats4;
    

    delegate void PowerUpFunctions();
    IEnumerator PowerUp()
    {
        Instantiate(powerupParticle, transform.position, transform.rotation);
        //CreateList
        List<PowerUpFunctions> getPower = new List<PowerUpFunctions>();
        getPower.Add(HealthUp);
        getPower.Add(MoveSpeed);
        getPower.Add(FireRate);
        getPower.Add(DashBuff);
        getPower.Add(PassiveHPRegen);
        getPower.Add(TripleJump);
        getPower.Add(DamageUp);
        

        //Calls functions
        //Can be Random.Range
        //Maybe Remove the powerup from the list after its selected once
        //Or have it multiplicative/Additive/Stackable
        getPower[Random.Range(0, getPower.Count)]();

        Destroy(gameObject);

        yield return null;
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(PowerUp());
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        Player                  stats1 = Player1.GetComponent<Player>();
        PlayerController        stats2 = Player1.GetComponent<PlayerController>();
        PlayerShoot             stats3 = Player1.GetComponent<PlayerShoot>();
        //FirstPersonController   stats4 = Player1.GetComponent<FirstPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    ///////////////////////////////////////////////////////
    ///////////////////////PowerUps////////////////////////
    ///////////////////////////////////////////////////////
        /// <summary>
        /// Healthup
        /// MoveSpeed
        /// FireRate
        /// DashBuff
        /// PassiveHPRegen
        /// TripleJump
        /// DamageUp
        /// </summary>
 
    public void HealthUp()
    {
        stats1.stats.health += 50;
    }

    void MoveSpeed()
    {
        stats1.stats.moveSpeed += 20;
    }

    void FireRate()
    {
        stats3.gunfireRate -= 0.1f;
    }

    void DashBuff()
    {
        //stats2.dashSpeed += 30f;
        //stats2.dashCooldown -= 0.5f;

        //need to change PlayerController to PUBLIC
    }

    void PassiveHPRegen()
    {
        stats1.stats.health += 100;
    }

    void TripleJump()
    {
        //stats4.jumpMax += 1;

        //Need to fix the reference GetComponent since its in a different folder
    }

    void DamageUp()
    {
        stats3.gunATK += 10;
    }
}

