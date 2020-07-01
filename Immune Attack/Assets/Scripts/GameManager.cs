using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    GameObject player;

    void Awake()
    {
        if (manager == null) manager = this;
        else Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);

        player = GameObject.FindGameObjectWithTag("Player");
    }
}
