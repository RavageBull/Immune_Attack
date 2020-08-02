using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;

    bool isDashing;
    bool canDash;
    Vector3 dashDir;
    float dashSpeed;
    float dashCooldown;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        dashSpeed = 70f;
        dashCooldown = 2f;
        canDash = true;
    }

    void Update()
    {
        if (Input.GetKeyDown("left shift") && canDash)
        {
            isDashing = true;
            canDash = false;
            dashDir = new Vector3(controller.velocity.x, 0, controller.velocity.z);
            dashDir = dashDir.normalized;
            StartCoroutine("Dash");
            StartCoroutine("DashCooldown");
        }
    }

    void FixedUpdate()
    {
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

    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
