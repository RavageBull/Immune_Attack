using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public delegate void BossDelegate();
    public static event BossDelegate BossAppear;

    [SerializeField] GameObject powerUpPrefab = null;

    void Start()
    {
        BossAppear();
    }

}
