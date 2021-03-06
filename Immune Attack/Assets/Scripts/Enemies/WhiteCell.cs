﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//The White Cell moves towards the player. Once in melee range, the white cell attacks.
public class WhiteCell : MonoBehaviour
{
    enum State
    {
        Search,
        Attack,
        Death,
    }
    State state;

    Stats stats;
    NavMeshAgent agent;
    Animator animator;

    bool canAttack;
    float attackRange;
    float attackCooldown;

    [SerializeField] AudioClip attackClip = null;
    [SerializeField] AudioClip deathClip = null;

    AudioSource audioSource;

    public delegate void EnemyDeathDelegate(GameObject enemy);
    public static EnemyDeathDelegate EnemyDeath;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Search;

        stats = GetComponent<Stats>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        stats.health = 50;
        stats.damage = 10;
        stats.moveSpeed = 10;

        agent.speed = stats.moveSpeed;

        canAttack = true;
        attackRange = 8f;
        attackCooldown = 2f;
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

        //if enemy gets close enough to the player, switch to attack mode
        if (Vector3.Distance(gameObject.transform.position, GameManager.manager.player.transform.position) < attackRange)
        {
            state = State.Attack;
        }
    }

    void Attack()
    {
        //stand still and play attack animation and deal damage
        agent.SetDestination(gameObject.transform.position);

        if (canAttack)
        {
            canAttack = false;
            animator.SetTrigger("Attack");
            StartCoroutine("AttackCooldown");

            audioSource.clip = attackClip;
            audioSource.Play();
        }

        //if the player moves too far away while in attack mode, switch back to search
        if (Vector3.Distance(gameObject.transform.position, GameManager.manager.player.transform.position) > attackRange)
        {
            state = State.Search;
        }
    }

    //this function is triggered by an event in the animation of this object
    //this happens right as the animation is biting down
    void Damage()
    {
        //if the player is still close enough when the damage point happens
        if (Vector3.Distance(gameObject.transform.position, GameManager.manager.player.transform.position) < attackRange)
        {
            GameManager.manager.player.GetComponent<Player>().TakeDamage(stats.damage);
        }
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    //this is triggered when the start of death animation is played.
    //this switches the ai state and makes it stop in its tracks.
    public void DeathStop()
    {
        state = State.Death;
        agent.SetDestination(gameObject.transform.position);

        audioSource.clip = deathClip;
        audioSource.Play();
    }

}
