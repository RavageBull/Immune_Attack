using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBloodCell : MonoBehaviour
{
    Stats stats;

    Rigidbody rb;
    Vector3 lastVelocity;

    Animator animator;

    [SerializeField]
    GameObject trailPrefab = null;
    GameObject trail;

    [SerializeField] AudioClip attackClip = null;
    [SerializeField] AudioClip deathClip = null;

    AudioSource audioSource;

    float activeRange;
    float attackRange;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<Stats>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        stats.health = 50;
        stats.damage = 10;
        stats.moveSpeed = 20f;

        activeRange = 20f;
        attackRange = 10f;

        rb.AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * 1200f);


        if (trailPrefab != null)
        {
            trail = Instantiate(trailPrefab, transform.position, Quaternion.identity);
            trail.transform.parent = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        lastVelocity = rb.velocity;

        if (Vector3.Distance(gameObject.transform.position, GameManager.manager.player.transform.position) < activeRange)
        {
            animator.SetTrigger("Attack");

            if (!audioSource.isPlaying)
            {
                audioSource.clip = attackClip;
                audioSource.Play();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        float speed = lastVelocity.magnitude;
        Vector3 direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

        rb.velocity = direction * Mathf.Max(speed, 0f);
    }

    //this function is triggered by an event in the animation of this object
    void Damage()
    {
        //if the player is still close enough when the damage point happens
        if (Vector3.Distance(gameObject.transform.position, GameManager.manager.player.transform.position) < attackRange)
        {
            GameManager.manager.player.GetComponent<Player>().TakeDamage(stats.damage);
        }
    }

    //this is triggered when the start of death animation is played.
    //this makes it stop in its tracks.
    public void DeathStop()
    {
        Destroy(trail.gameObject);
        rb.velocity = Vector3.zero;

        audioSource.clip = deathClip;
        audioSource.Play();
    }
}
