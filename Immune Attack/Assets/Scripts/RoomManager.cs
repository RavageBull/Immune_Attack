using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    //This script holds the details of each room and gives relevant information as appropriate

    [SerializeField] List<GameObject> enemyList;
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
        //Could probably find all enemies in room and fill up the enemyList that way instead of having to manually input enemies
        //Don't know which is more efficient
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
