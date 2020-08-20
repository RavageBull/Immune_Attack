using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public delegate void BossDelegate();
    public static event BossDelegate BossAppear;

    void Start()
    {
        BossAppear();
    }

}
