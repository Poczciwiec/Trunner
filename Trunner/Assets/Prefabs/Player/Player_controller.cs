using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_controller : MonoBehaviour
{
    private float moveSpeed = 10f;

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float zAxis = Input.GetAxis("Vertical");

        this.transform.Translate(new Vector3(xAxis, 0, zAxis).normalized * moveSpeed * Time.deltaTime);
    }
}
