using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] AudioClip bossClip = null;

    public delegate void BossDelegate(string name, AudioClip clip);
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
        BossAppear(gameObject.name, bossClip);
    }

    void SpawnPowerUp()
    {
        GameObject obj = Instantiate(powerUpPrefab, powerUpPoint.position, Quaternion.identity);
        obj.GetComponent<Powerups>().RandomBossPowerUp();
    }

}
