﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WhiteCell : MonoBehaviour
{
    enum State
    {
        Search,
        Attack,
    }
    State state;

    public Stats stats;
    public NavMeshAgent agent;

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
        stats.moveSpeed = 10;

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
        }

        //if enemy gets close enough to the player, switch to attack mode
        if (Vector3.Distance(gameObject.transform.position, GameManager.manager.player.transform.position) < 5f)
        {
            state = State.Attack;
        }
    }

    void Search()
    {
        NavMeshHit hit;
        Vector3 destination = Vector3.zero;
        if (NavMesh.SamplePosition(GameManager.manager.player.transform.position, out hit, 10f, 1))
        {
            destination = hit.position;
        }
        agent.SetDestination(destination);
    }

    void Attack()
    {
        //stand still and play attack animation and deal damage
        agent.SetDestination(gameObject.transform.position);

        /*PLAY ANIMATION*/

        






        //if the player moves too far away while in attack mode, switch back to search
        if (Vector3.Distance(gameObject.transform.position, GameManager.manager.player.transform.position) > 8f)
        {
            state = State.Search;
        }
    }

}
