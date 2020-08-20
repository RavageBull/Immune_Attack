using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Stats stats;

    //event for when the player's health changes
    public delegate void HealthDelegate();
    public static HealthDelegate Healed;
    public static HealthDelegate Damaged;
    public static HealthDelegate Death;

    //event for when the player is created so the GameManager take reference this object
    public delegate void PlayerSpawnDelegate(GameObject player);
    public static PlayerSpawnDelegate PlayerSpawn;

    //this is set to awake in order for it to happen before anything searches for it in a start function
    void Awake()
    {
        stats = GetComponent<Stats>();
        stats.health = 100;
        stats.moveSpeed = 20f;  //this does nothing at the moment since movement speed is controlled in the first person controller script
                                //will integrate this at a later date

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        PlayerSpawn(gameObject);
        stats = GetComponent<Stats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        stats.health = Mathf.Clamp(stats.health -= damage, 0, 100);

        //event fires whenever this function is triggered
        Damaged();

        if (stats.health <= 0)
        {
            Death();
        }    
    }

    public IEnumerator Regen()
    {
        while (stats.regen > 0)
        {
            yield return new WaitForSeconds(stats.regenDelay);
            stats.health = Mathf.Clamp(stats.health += stats.regen, 0, 100);
            Healed();
        }

    }
}
