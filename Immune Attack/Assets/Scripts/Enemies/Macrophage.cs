using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//The Macrophage starts off Idle. While idle, it cannot be damaged.
//If the player gets too close, the Macrophage starts to move towards the player and attacks.
//While active, the Macrophage is vulnerable.
public class Macrophage : MonoBehaviour
{
    enum State
    {
        Idle,
        Search,
        Attack,
        Death,
    }
    State state;

    Stats stats;
    NavMeshAgent agent;
    Animator animator;

    float searchRange;
    bool canAttack;
    float attackRange;
    float attackCooldown;
    float prevHealth;

    [SerializeField] AudioClip attackClip;
    [SerializeField] AudioClip deathClip;

    AudioSource audioSource;



    public delegate void EnemyDeathDelegate(GameObject enemy);
    public static EnemyDeathDelegate EnemyDeath;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;

        stats = GetComponent<Stats>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        stats.health = 300;
        stats.damage = 30;
        stats.moveSpeed = 5;

        agent.speed = stats.moveSpeed;

        searchRange = 20f;
        canAttack = true;
        attackRange = 12f;
        attackCooldown = 2f;

        prevHealth = stats.health;
    }

    // Update is called once per frame
    void Update()
    {
        //switch cases for different AI states
        switch (state)
        {
            case State.Idle:
                Idle();
                break;
            case State.Search:
                Search();
                break;
            case State.Attack:
                Attack();
                break;
        }
    }

    void Idle()
    {
        if (stats.health < prevHealth)
        {
            stats.health = prevHealth; //temporary way of making Macrophage "invulnerable" while idle;
        }

        //if enemy gets close enough to the player, switch to search mode
        if (Vector3.Distance(gameObject.transform.position, GameManager.manager.player.transform.position) < searchRange)
        {
            state = State.Search;
            animator.SetTrigger("Search");
            audioSource.clip = attackClip;
            audioSource.Play();
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

        //if enemy gets too far from the player, switch to idle mode
        if (Vector3.Distance(gameObject.transform.position, GameManager.manager.player.transform.position) > searchRange)
        {
            prevHealth = stats.health; //
            state = State.Idle;
            animator.SetTrigger("Idle");
            agent.SetDestination(transform.position);
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
