using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Stats stats;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<Stats>();
        stats.health = 100;
        stats.moveSpeed = 20f;  //this does nothing at the moment since movement speed is controlled in the first person controller script
                                //will integrate this at a later date
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
