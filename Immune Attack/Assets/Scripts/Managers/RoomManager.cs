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

    [SerializeField] GameObject portalSound;

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
        //disables portal door
        portalDoor.SetActive(false);

        //finds all enemies in current room and adds to a list
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length > 0)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                enemyList.Add(enemies[i]);
            }
        }
        else
        {
            SpawnPortal();
        }

        //sends an event stating that the room manager is ready which the game manager should pick up
        RoomSpawn(gameObject);
    }

    //triggers this when an enemy death event happens
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
        portalDoor.SetActive(true);
        GameObject obj = Instantiate(portalSound, portalDoor.transform.position, Quaternion.identity);
        Destroy(obj, 1);
    }
}
