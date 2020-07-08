using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public Stats stats;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<Stats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage()
    {
        Destroy(gameObject);
    }
}
