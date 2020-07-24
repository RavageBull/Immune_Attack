using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileATTACK4 : MonoBehaviour
{
    public float damage;
    public GameObject player1;
    public int projectileMoveSpeed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            other.GetComponent<Player>().TakeDamage(damage);

            Destroy(gameObject);
        }
    }

    void Start()
    {
        damage = 10f;

        player1 = GameManager.manager.player;
        StartCoroutine(Shots());
    }

    IEnumerator Shots()
    {
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(Random.Range(0f, 2f));

        Vector3 projectileDirection = (player1.transform.position - transform.position).normalized * projectileMoveSpeed;
        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(projectileDirection.x, projectileDirection.y, projectileDirection.z);
        yield return null;
    }
}
