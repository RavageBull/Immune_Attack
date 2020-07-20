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

    Stats stats;
    NavMeshAgent agent;
    Animator animator;


    [SerializeField] GameObject projectilePrefab;
    bool canAttack;
    float attackRange;
    float attackCooldown;
    bool isFleeing;
    float fleeDuration;

    public delegate void EnemyDeathDelegate(GameObject enemy);
    public static EnemyDeathDelegate EnemyDeath;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Search;

        stats = GetComponent<Stats>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        stats.health = 50;
        stats.damage = 10;
        stats.moveSpeed = 20;

        agent.speed = stats.moveSpeed;

        canAttack = true;
        attackRange = 50f;
        attackCooldown = 3f;
        fleeDuration = 2f;
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
        //moves towards the player to try and find an angle
        NavMeshHit hit;
        Vector3 destination = Vector3.zero;
        if (NavMesh.SamplePosition(GameManager.manager.player.transform.position, out hit, 10f, 1))
        {
            destination = hit.position;
        }
        agent.SetDestination(destination);

        //once the player is in sight, switch to attack
        RaycastHit rayHit;
        if (Physics.Raycast(transform.position, GameManager.manager.player.GetComponent<Stats>().origin.position - transform.position, out rayHit, attackRange))
        {
            if (rayHit.transform.tag == "Player")
            {
                state = State.Attack;
            }
        }

        //if the player is too close, flee
        if (Vector3.Distance(gameObject.transform.position, GameManager.manager.player.transform.position) < 10f)
        {
            state = State.Flee;
        }

    }

    void Attack()
    {
        //stops in position and initiates attack animation
        agent.SetDestination(transform.position);
        if (canAttack)
        {
            canAttack = false;
            animator.SetTrigger("Attack");
            StartCoroutine("AttackCooldown");
        }


        //if the player can no longer be seen, start searching
        RaycastHit rayHit;
        if (Physics.Raycast(transform.position, GameManager.manager.player.GetComponent<Stats>().origin.position - transform.position, out rayHit, attackRange))
        {
            if (rayHit.transform.tag != "Player")
            {
                state = State.Search;
            }
        }

        //if the player is too close, flee
        if (Vector3.Distance(gameObject.transform.position, GameManager.manager.player.transform.position) < 10f)
        {
            state = State.Flee;
        }

    }

    //this function is triggered by an event in the animation cycle of this object
    //this happens right as the animation shoots
    //still deciding whether or not to put some of this code onto the projectile script itself rather than here
    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().damage = GetComponent<Stats>().damage;

        Vector3 direction = Vector3.zero;
        direction = GameManager.manager.player.GetComponent<Stats>().origin.position - transform.position;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = direction.normalized * 50f;

        Destroy(projectile, 10f);
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    void Flee()
    {
        if (isFleeing == false)
        {
            isFleeing = true;

            Vector3 newPos;
            newPos = transform.position;
            newPos += transform.position - GameManager.manager.player.transform.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(newPos, out hit, 20f, 1))
            {
                newPos = hit.position;
            }

            agent.SetDestination(newPos);

            StartCoroutine("FleeDuration");
        }
        
    }

    //the duration of fleeing. After it is over, checks to see if the object should search for or attack the player
    IEnumerator FleeDuration()
    {
        yield return new WaitForSeconds(fleeDuration);
        isFleeing = false;

        RaycastHit rayHit;
        if (Physics.Raycast(transform.position, GameManager.manager.player.GetComponent<Stats>().origin.position - transform.position, out rayHit, attackRange))
        {
            if (rayHit.transform.tag == "Player")
            {
                state = State.Attack;
            }
            else
            {
                state = State.Search;
            }
        }

    }

}

