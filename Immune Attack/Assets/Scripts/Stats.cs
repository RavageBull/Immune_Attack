using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public float moveSpeed;
    public float damage;
    public float regen;
    public float regenDelay;

    public Transform origin;    //the origin where Raycast calculations should happen from
                                //this is so that we can determine the positions of projectile origin/destination and other things
                                //this can be set by adding an empty gameobject to this object or a prefab, and then manually
                                //attaching it to this value
                                //so far this is only used by the player

    // Start is called before the first frame update
    void Start()
    {
        regenDelay = 4f;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
