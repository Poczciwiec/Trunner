using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_controller : MonoBehaviour
{
    private const float maxSpeed = 10f;
    private float acceleration = 1.01f;
    private float deceleration = 0.8f;
    [SerializeField] private float moveSpeed = 0.1f;
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

        this.transform.Translate(directionalInput * DetermineSpeed() * Time.deltaTime);
    }

    float DetermineSpeed()
    {
        if (!isMoving && moveSpeed <= 0.1f) moveSpeed = 0.1f;
        else if (isMoving && moveSpeed < maxSpeed) moveSpeed = Mathf.Pow(moveSpeed, acceleration) + 0.1f;
        else if (isMoving && moveSpeed >= maxSpeed) moveSpeed = maxSpeed;
        else if (!isMoving && moveSpeed > 0.1f) moveSpeed *= deceleration;

        return moveSpeed;
    }
}
