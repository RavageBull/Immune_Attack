using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

[ExecuteInEditMode]
public class Powerups : MonoBehaviour
{

    public GameObject powerupParticle; //not sure what this is for
    GameObject player = null;
    Player stats1;
    PlayerController stats2;
    PlayerShoot stats3;
    FirstPersonController stats4;

    [SerializeField] GameObject powerUpSound;

    [System.Serializable]
    public struct PowerUpSprites
    {
        public Sprite health;
        public Sprite ammo;
        public Sprite regen;
        public Sprite moveSpeed;
        public Sprite dashBuff;
        public Sprite jumpUp;
        public Sprite damageUp;
    }

    public enum PowerUpType
    {
        //GENERAL LEVEL POWERUPS
        Health,     //increases current health
        Ammo,       //refreshes ammo

        //BOSS REWARD POWERUPS
        Regen,      //passive health regen
        MoveSpeed,  //increased movement speed
        DashBuff,   //improved dash distance and cooldown
        JumpUp,     //extra jump
        DamageUp,   //extra damage
    }

    public PowerUpType powerUpType;
    public PowerUpSprites powerUpSprites;

    //events for when health and ammo powerups are picked up so UI knows to update accordingly
    public delegate void UpdateDelegate();
    public static event UpdateDelegate HealthUpdate;
    public static event UpdateDelegate AmmoUpdate;

    public delegate void PowerUpDelegate(string text);
    public static event PowerUpDelegate PowerUpPicked;


    //subscribes to the event when the GameManager finishes collecting and assigning some data
    private void OnEnable()
    {
        GameManager.FinishLoading += Load;
    }

    private void OnDisable()
    {
        GameManager.FinishLoading -= Load;
    }

    void Update()
    {
        switch (powerUpType)
        {
            case PowerUpType.Health:
                GetComponent<SpriteRenderer>().sprite = powerUpSprites.health;
                break;
            case PowerUpType.Ammo:
                GetComponent<SpriteRenderer>().sprite = powerUpSprites.ammo;
                break;
            case PowerUpType.Regen:
                GetComponent<SpriteRenderer>().sprite = powerUpSprites.regen;
                break;
            case PowerUpType.MoveSpeed:
                GetComponent<SpriteRenderer>().sprite = powerUpSprites.moveSpeed;
                break;
            case PowerUpType.DashBuff:
                GetComponent<SpriteRenderer>().sprite = powerUpSprites.dashBuff;
                break;
            case PowerUpType.JumpUp:
                GetComponent<SpriteRenderer>().sprite = powerUpSprites.jumpUp;
                break;
            case PowerUpType.DamageUp:
                GetComponent<SpriteRenderer>().sprite = powerUpSprites.damageUp;
                break;
        }
    }

    public void RandomBossPowerUp()
    {
        powerUpType = (PowerUpType)Random.Range(2, System.Enum.GetValues(typeof(PowerUpType)).Length);
    }

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
        if (other.GetComponent<Player>())
        {
            switch (powerUpType)
            {
                case PowerUpType.Health:
                    Health();
                    PowerUpPicked("HEALED!");
                    break;
                case PowerUpType.Ammo:
                    Ammo();
                    PowerUpPicked("AMMO REFRESH!");
                    break;
                case PowerUpType.Regen:
                    Regen();
                    PowerUpPicked("HEALTH REGENERATION UP!");
                    break;
                case PowerUpType.MoveSpeed:
                    MoveSpeed();
                    PowerUpPicked("MOVEMENT SPEED UP!");
                    break;
                case PowerUpType.DashBuff:
                    PowerUpPicked("DASH IMPROVED!");
                    DashBuff();
                    break;
                case PowerUpType.JumpUp:
                    JumpUp();
                    PowerUpPicked("EXTRA JUMP!");
                    break;
                case PowerUpType.DamageUp:
                    DamageUp();
                    PowerUpPicked("DAMAGE INCREASED!");
                    break;
            }

            GameObject obj = Instantiate(powerUpSound, transform.position, Quaternion.identity);
            Destroy(obj, 1);
            Destroy(this.gameObject);
        }
    }
 
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

