using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladderFloor : MonoBehaviour
{

    //Lerp of Rising floor/Boss
    [Header("Lerp Settings")]
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float distance = 30f;
    public float lerpTime = 5f;
    private float currentLerpTime = 0;
    public bool canHurt;
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        //LerpStuff
        startPosition = transform.position;
        endPosition = transform.position + Vector3.up * distance;
    }

    // Update is called once per frame
    void Update()
    {
        //LerpStuff

        currentLerpTime += Time.deltaTime;
        if (currentLerpTime >= lerpTime)
        {
            currentLerpTime = lerpTime;
        }

        float Perc = currentLerpTime / lerpTime;
        //Lerps in y direction Only
        transform.position = Vector3.Lerp(startPosition, endPosition, Perc);
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Trigger entered");
        if (canHurt == true && other.gameObject.GetComponent<Player>())
        {
            other.gameObject.GetComponent<Player>().TakeDamage(damage);
            StartCoroutine(DealDamage());
        }
    }

    IEnumerator DealDamage()
    {
        canHurt = false;
        yield return new WaitForSeconds(1f);
        canHurt = true;
    }
}
