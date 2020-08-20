using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    //This script holds the details of each room and gives relevant information as appropriate

    public List<GameObject> enemyList;

    public Transform playerSpawn;
    [SerializeField] GameObject portalDoor = null;

    public delegate void RoomSpawnDelegate(GameObject room);
    public static RoomSpawnDelegate RoomSpawn;

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
        //finds all enemies in current room and adds to a list
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < enemies.Length; i++)
        {
            enemyList.Add(enemies[i]);
        }

        //disables portal door
        portalDoor.SetActive(false);


        //sends an event when spawned which should notify the game manager who the current room manager is
        RoomSpawn(gameObject);

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
        //Instantiate(portalPrefab, portalPoint.position, Quaternion.identity);
        portalDoor.SetActive(true);
    }
}
