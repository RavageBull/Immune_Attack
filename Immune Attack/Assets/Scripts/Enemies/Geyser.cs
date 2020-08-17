using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geyser : MonoBehaviour
{
    float currentLerpTime;
    public float lerpTime;
    public Vector3 startScale;
    public Vector3 endScale;
    // Start is called before the first frame update
    void Start()
    {
        startScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        currentLerpTime += Time.deltaTime;
        if (currentLerpTime >= lerpTime)
        {
            currentLerpTime = lerpTime;
        }

        float Perc = currentLerpTime / lerpTime;
        //Lerps in y direction Only
        transform.localScale = Vector3.Lerp(startScale, endScale, Perc);
    }
}
