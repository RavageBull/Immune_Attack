using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BCell : MonoBehaviour
{
    enum State
    {
        Search,
        Attack,
        Flee,
    }
    State state;

    public Stats stats;
    public NavMeshAgent agent;

    [SerializeField] GameObject projectilePrefab;

    public delegate void EnemyDeathDelegate(GameObject enemy);
    public static EnemyDeathDelegate EnemyDeath;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Search;

        stats = GetComponent<Stats>();
        agent = GetComponent<NavMeshAgent>();

        stats.health = 50;
        stats.damage = 10;
        stats.moveSpeed = 20;

        agent.speed = stats.moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //switch cases for different AI states
        switch (state)
        {
            case State.Search:
                Search();
                break;
            case State.Attack:
                Attack();
                break;
            case State.Flee:
                Flee();
                break;
        }
    }

    void Search()
    {
        //moves towards the player
        NavMeshHit hit;
        Vector3 destination = Vector3.zero;
        if (NavMesh.SamplePosition(GameManager.manager.player.transform.position, out hit, 10f, 1))
        {
            destination = hit.position;
        }
        agent.SetDestination(destination);

        //once the player is in sight, switch to attck
        RaycastHit rayHit;
        if (Physics.Raycast(transform.position, GameManager.manager.player.transform.position - transform.position, out rayHit, 50f))
        {
            if (rayHit.transform.tag == "Player")
            {
                state = State.Attack;
            }
        }
    }

    void Attack()
    {
        agent.SetDestination(transform.position);

        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
    }

    void Flee()
    {

    }

}

