using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    //This script holds the details of each room and gives relevant information as appropriate

    List<GameObject> enemyList;

    [SerializeField] Transform portalPoint;
    [SerializeField] GameObject portalPrefab;

    private void OnEnable()
    {
        Enemy.EnemyDeath += EnemyUpdate;
    }

    private void OnDisable()
    {
        Enemy.EnemyDeath -= EnemyUpdate;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < enemies.Length; i++)
        {
            enemyList.Add(enemies[i]);
        }
        
    }

    void EnemyUpdate(GameObject enemy)
    {
        enemyList.Remove(enemy);
        Destroy(enemy);

        if (enemyList.Count <= 0)
        {
            SpawnPortal();
        }
    }

    void SpawnPortal()
    {
        Instantiate(portalPrefab, portalPoint.position, Quaternion.identity);
    }
}
