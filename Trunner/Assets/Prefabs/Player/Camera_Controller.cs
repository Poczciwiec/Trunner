using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Camera_Controller : MonoBehaviour
{
    InputAction LookAction;
    float mouse_sensitivity = 0.1f;
    private void Start()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        
        Rotate();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetMouseButtonDown(0))
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void Rotate()
    {
        Vector2 mouseMove = new Vector2(0f, 0f);
        float w = mouseMove.magnitude * mouse_sensitivity;
        Quaternion rotation = new Quaternion(w, Math.Sign(mouseMove.x), Math.Sign(mouseMove.y), 0f);
        Quaternion inversed = new Quaternion(rotation.w, -rotation.x, -rotation.y, -rotation.z);

        this.transform.parent.transform.rotation *= rotation.normalized*inversed;
    }

}
