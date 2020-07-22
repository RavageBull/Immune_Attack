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
        player1 = GameManager.manager.player;
        StartCoroutine(Shots());
    }

    IEnumerator Shots()
    {
        yield return new WaitForSeconds(0.5f);
        Vector3 projectileDirection = (player1.transform.position - transform.position).normalized * projectileMoveSpeed;
        GetComponent<Rigidbody>().velocity = new Vector3(projectileDirection.x, projectileDirection.y, projectileDirection.z);

    }
}
