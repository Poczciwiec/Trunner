using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_controller : MonoBehaviour
{
    private const float maxSpeed = 10f;
    private float acceleration = 1.05f;
    private float deceleration = 0.6f;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float jumpForce = 5f;
    bool isMoving;

    void Update()
    {
        Movement();
        
    }

    void Movement()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float zAxis = Input.GetAxis("Vertical");

        if (xAxis != 0 || zAxis != 0) isMoving = true;
        else isMoving = false;

        Vector3 directionalInput = new Vector3(xAxis, 0, zAxis).normalized;
        directionalInput *= DetermineSpeed();

        if (Input.GetKey(KeyCode.Space)) // AIM: the longer space is pressed the higher and longer the jump
        {
            Jump();
        }

        this.transform.Translate(directionalInput * Time.deltaTime);
    }

    void Jump()
    {
        Vector3 jump = new Vector3(0, jumpForce, 0);
        this.transform.Translate(jump * Time.deltaTime);
    }

    float DetermineSpeed()
    {
        if (!isMoving && moveSpeed <= 0.1f) moveSpeed = 0.1f;
        else if (isMoving && moveSpeed < maxSpeed) moveSpeed *= acceleration;
        else if (isMoving && moveSpeed >= maxSpeed) moveSpeed = maxSpeed;
        else if (!isMoving && moveSpeed > 0.1f) moveSpeed *= deceleration;

        return moveSpeed;
    }
}
