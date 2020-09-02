using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//The Natural Killer Cell flies in the air. Its path is random.
//If the player is in its sight, it will periodically shoot a projectile at the player.
//If attacked, it will try to move to another area in an attempt to dodge.
public class NatKillerCell : MonoBehaviour
{
    enum State
    {
        Search,
        Attack,
        Flee,
        Death,
    }
    State state;

    Stats stats;
    NavMeshAgent agent;
    Animator animator;


    [SerializeField] GameObject projectilePrefab = null;
    bool canAttack;
    float attackRange;
    float attackCooldown;
    bool isFleeing;
    float fleeDuration;

    bool canPatrol;
    float patrolCooldown;

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
        attackRange = 100f;
        attackCooldown = 3f;
        fleeDuration = 2f;

        canPatrol = true;
        patrolCooldown = 4f;
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
        if (canPatrol)
        {
            canPatrol = false;
            StartCoroutine(Patrol());
        }

        if (canAttack)
        {
            canAttack = false;
            animator.SetTrigger("Attack");
            StartCoroutine("AttackCooldown");
        }

    }

    IEnumerator Patrol()
    {
        //finds a random position in the NavMesh
        Vector3 randomDirection = Random.insideUnitSphere * 50f;
        randomDirection += Vector3.zero;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, 50f, 1))
        {
            finalPosition = hit.position;
        }
        agent.SetDestination(finalPosition);

        


        yield return new WaitForSeconds(Random.Range(patrolCooldown - 1, patrolCooldown + 1));
        Debug.Log("Patrol Attempt");
        canPatrol = true;
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

        /*//if the player is too close, flee
        if (Vector3.Distance(gameObject.transform.position, GameManager.manager.player.transform.position) < 10f)
        {
            state = State.Flee;
        }*/

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

    //this is triggered when the start of death animation is played.
    //this switches the ai state and makes it stop in its tracks.
    public void DeathStop()
    {
        state = State.Death;
        agent.SetDestination(gameObject.transform.position);
    }

}

