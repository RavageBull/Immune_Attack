using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Stats stats;

    //event for when the player takes damage
    public delegate void DamageDelegate();
    public static DamageDelegate DamageTaken;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        stats.health -= damage;

        //DamageTaken event fires whenever this function is triggered
        DamageTaken();
    }
}
