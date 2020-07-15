using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum EnemyType
    {
        Melee,
        Ranged,
    }

    public Stats stats;

    public EnemyType enemyType;

    //Enemy Type//
    public bool redAggro;


    public NavMeshAgent agent;
    public GameObject marker; //for testing purposes

    public delegate void EnemyDeathDelegate(GameObject enemy);
    public static EnemyDeathDelegate EnemyDeath;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<Stats>();

        if (redAggro == true)
        {
            stats.health = 50;
            stats.damage = 10;
            stats.moveSpeed = 10;   //this does nothing at the moment as enemy is moved via navmesh agent
                                    //will have to adjust the speed in the navmesh agent component
                                    //can be done at a later date
        }
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stats.health <= 0) //this can be better. preferably not in update.
        {
            Death();
        }

        switch (enemyType)
        {
            case EnemyType.Melee:
                WalkToPlayer();
                break;
            case EnemyType.Ranged:
                break;
        }

        if (Vector3.Distance(gameObject.transform.position, GameManager.manager.player.transform.position) < 10f
            && enemyType == EnemyType.Ranged)
        {
            Vector3 newPos;
            newPos = transform.position;
            newPos += transform.position - GameManager.manager.player.transform.position;
            /*int offset = 2;
            newPos *= offset;*/

            NavMeshHit hit;
            if (NavMesh.SamplePosition(newPos, out hit, 20f, 1))
            {
                newPos = hit.position;
            }

            agent.SetDestination(newPos);
            
            /*GameObject obj = Instantiate(marker, newPos, Quaternion.identity);
            Destroy(obj, 2f);*/
        }
    }

    void WalkToPlayer()
    {
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(GameManager.manager.player.transform.position, out hit, 10f, 1))
        {
            finalPosition = hit.position;
        }
        agent.SetDestination(finalPosition);
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

        //creates a marker where the agent will move to for testing purposes
        GameObject obj = Instantiate(marker, hit.position, Quaternion.identity);
        Destroy(obj, 10f);

        return finalPosition;
    }

    public void TakeDamage(int dmg)
    {
        if (stats.health > 0)
        {
            stats.health -= dmg;
            Debug.Log("Enemy took " + dmg + " damage");
        }
        
    }

    public void Death()
    {
        EnemyDeath(gameObject);
    }

}
