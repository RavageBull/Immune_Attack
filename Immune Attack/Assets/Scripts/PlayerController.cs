using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;

    public bool isDashing;
    Vector3 dashDir;
    float dashSpeed;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        dashSpeed = 70f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("left shift") && !isDashing)
        {
            isDashing = true;
            dashDir = new Vector3(controller.velocity.x, 0, controller.velocity.z);
            dashDir = dashDir.normalized;
            StartCoroutine("Dash");
        }

        if (isDashing)
        {
            controller.Move(dashDir * dashSpeed * Time.deltaTime);
        }
    }

    IEnumerator Dash()
    {
        yield return new WaitForSeconds(0.2f);
        isDashing = false;
    }
}
