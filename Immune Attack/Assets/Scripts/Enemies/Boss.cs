using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public delegate void BossDelegate(string name);
    public static event BossDelegate BossAppear;

    [SerializeField] GameObject powerUpPrefab = null;
    [SerializeField] Transform powerUpPoint = null;

    private void OnEnable()
    {
        Enemy.BossDeath += SpawnPowerUp;
    }

    private void OnDisable()
    {
        Enemy.BossDeath -= SpawnPowerUp;
    }

    void Start()
    {
        BossAppear(gameObject.name);
    }

    void SpawnPowerUp()
    {
        if (powerUpPrefab != null)
        {
            GameObject obj = Instantiate(powerUpPrefab, powerUpPoint.position, Quaternion.identity);
            obj.GetComponent<Powerups>().RandomBossPowerUp();
        }
        
    }

}
