using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    //Stats//
    public int HP;
    public int ATK;
    public float SPD;
    

    //Enemy Type//
    public bool redAggro;


    public NavMeshAgent agent;
    public GameObject marker; //for testing purposes
    // Start is called before the first frame update
    void Start()
    {
        if (redAggro == true)
        {
            HP = 50;
            ATK = 10;
            SPD = 10;
        }

        agent = GetComponent<NavMeshAgent>();
        InvokeRepeating("Patrol", 1f, 10f); //temporary way of setting a patrol point
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0)
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
        if (HP > 0)
        {
            HP -= dmg;
            Debug.Log("Enemy took " + dmg + " damage");
        }
        
    }

    public void EnemyDeath()
    {
        Destroy(gameObject,1);
    }

}
