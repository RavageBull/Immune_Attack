using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Stats stats;

    //Enemy Type//
    public bool redAggro;


    public NavMeshAgent agent;
    public GameObject marker; //for testing purposes

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
        InvokeRepeating("Patrol", 1f, 10f); //temporary way of setting a patrol point
    }

    // Update is called once per frame
    void Update()
    {
        if (stats.health <= 0)
        {
            EnemyDeath();
        }
    }

    void Patrol()
    {
        agent.SetDestination(RandomNavmeshLocation(50f));
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

    public void EnemyDeath()
    {
        Destroy(gameObject,1);
    }

}
