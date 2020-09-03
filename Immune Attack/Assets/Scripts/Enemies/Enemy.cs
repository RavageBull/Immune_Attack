using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    Stats stats;

    public delegate void EnemyDeathDelegate(GameObject enemy);
    public static event EnemyDeathDelegate EnemyDeath;

    public delegate void BossDelegate(float currentHealth, float maxHealth);
    public static event BossDelegate BossDamaged;

    public delegate void BossDeathDelegate();
    public static event BossDeathDelegate BossDeath;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<Stats>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    //returns a random navmesh location around the object
    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += Vector3.zero;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }

        return finalPosition;
    }

    public void TakeDamage(float dmg)
    {
        stats.health -= dmg;
        //Debug.Log("Enemy took " + dmg + " damage");

        if (GetComponent<Boss>())
        {
            BossDamaged(stats.health, stats.maxHealth);
        }

        if (stats.health <= 0)
        {
            if (GetComponent<Boss>())
            {
                BossDeath();
                Death();
            }
            else if (GetComponent<Animator>())
            {
                GetComponent<Animator>().SetTrigger("Death");
            }
            else
            {
                Death();
            }
            
        }

        


    }

    public void Death()
    {
        EnemyDeath(gameObject);
    }

}
