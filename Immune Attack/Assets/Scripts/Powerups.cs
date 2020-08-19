using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Powerups : MonoBehaviour
{

    public GameObject powerupParticle;

    Player stats1;
    PlayerController stats2;
    PlayerShoot stats3;
    FirstPersonController stats4;

    GameObject player = null;
    

    public enum PowerUpType
    {
        //GENERAL LEVEL POWERUPS
        Health,     //increases current health
        Ammo,       //refreshes ammo

        //BOSS REWARD POWERUPS
        Regen,      //passive health regen
        MoveSpeed,  //increased movement speed
        DashBuff,   //improved dash distance and cooldown
        JumpUp, //extra jump
        DamageUp,   //extra damage
    }

    public PowerUpType powerUpType;

    //events for when health and ammo powerups are picked up so UI knows to update accordingly
    public delegate void UpdateDelegate();
    public static UpdateDelegate HealthUpdate;
    public static UpdateDelegate AmmoUpdate;


    //subscribes to the event when the GameManager finishes collecting and assigning some data
    private void OnEnable()
    {
        GameManager.FinishLoading += Load;
    }

    private void OnDisable()
    {
        GameManager.FinishLoading -= Load;
    }

    /*delegate void PowerUpFunctions();
    IEnumerator PowerUp()
    {
        Instantiate(powerupParticle, transform.position, transform.rotation);
        //CreateList
        List<PowerUpFunctions> getPower = new List<PowerUpFunctions>();
        getPower.Add(Health);
        getPower.Add(MoveSpeed);
        getPower.Add(Regen);
        getPower.Add(DashBuff);
        getPower.Add(TripleJump);
        getPower.Add(DamageUp);
        

        //Calls functions
        //Can be Random.Range
        //Maybe Remove the powerup from the list after its selected once
        //Or have it multiplicative/Additive/Stackable
        getPower[Random.Range(0, getPower.Count)]();

        Destroy(gameObject);

        yield return null;
    }*/

    //when the GameManager finishes collecting data
    void Load()
    {
        player = GameManager.manager.player;

        stats1 = player.GetComponent<Player>();
        stats2 = player.GetComponent<PlayerController>();
        stats3 = player.GetComponent<PlayerShoot>();
        stats4 = player.GetComponent<FirstPersonController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if(other.CompareTag("Player"))
        {
            StartCoroutine(PowerUp());
        }*/

        if (other.GetComponent<Player>())
        {
            switch (powerUpType)
            {
                case PowerUpType.Health:
                    Health();
                    break;
                case PowerUpType.Ammo:
                    Ammo();
                    break;
                case PowerUpType.Regen:
                    Regen();
                    break;
                case PowerUpType.MoveSpeed:
                    MoveSpeed();
                    break;
                case PowerUpType.DashBuff:
                    DashBuff();
                    break;
                case PowerUpType.JumpUp:
                    JumpUp();
                    break;
                case PowerUpType.DamageUp:
                    DamageUp();
                    break;
            }

            Destroy(this.gameObject);
        }
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
 
    public void Health()
    {
        stats1.stats.health = Mathf.Clamp(stats1.stats.health += 50, 0, 100);
        HealthUpdate();
    }

    void Ammo()
    {
        stats3.currentBullets = stats3.bulletsMag;
        AmmoUpdate();
    }

    void MoveSpeed()
    {
        stats4.m_WalkSpeed += 10;
        stats4.m_RunSpeed += 10;
    }

    void DashBuff()
    {
        stats2.dashSpeed += 50f;
        stats2.dashCooldown -= 1f;
    }

    void Regen()
    {
        stats1.stats.regen += 5;
        stats1.StopCoroutine("Regen");
        stats1.StartCoroutine("Regen");
    }

    void JumpUp()
    {
        stats4.jumpMax += 1;
    }

    void DamageUp()
    {
        stats3.gunATK += 10;
    }
}

