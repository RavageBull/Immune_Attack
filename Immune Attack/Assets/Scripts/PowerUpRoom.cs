using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpRoom : MonoBehaviour
{
    [SerializeField] GameObject powerUpPrefab = null;
    //[SerializeField] Transform powerUpPoint = null;

    // Start is called before the first frame update
    void Start()
    {
        GameObject obj = Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
        obj.GetComponent<Powerups>().RandomBossPowerUp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
