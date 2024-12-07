using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Trunner.Input;
using System;
using static Trunner.Input.InputActions;

public class Player_controller : MonoBehaviour, IPlayerActions
{
    InputActions controls;
    private const float maxSpeed = 10f;
    private float acceleration = 1.05f;
    private float deceleration = 0.6f;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float jumpForce = 5f;
    Vector2 mouseMove;
    Vector2 playerMove;
    bool isMoving = true;
    float mouse_sensitivity = 0.1f;


    private void Start()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        transform.position = new Vector3(0f, 1f, 0f);
    }

    private void Update()
    {
        mouseMove = controls.Player.Look.ReadValue<Vector2>();
        playerMove = controls.Player.Move.ReadValue<Vector2>();
        
        Movement();
    }


    // -------------- InputActions E/D ----------------
    public void OnEnable()
    {
        if (controls == null)
        {
            controls = new InputActions();
            controls.Player.SetCallbacks(this);
        }
        controls.Player.Enable();
        controls.Player.Move.Enable();
        controls.Player.Look.Enable();

        controls.Player.Fire.performed += OnFire;
        controls.Player.Move.performed += OnMove;
        controls.Player.Look.performed += OnLook;
    }

    public void OnDisable()
    {
        controls.Player.Disable();
    }

    // -------------------------------------------------

    public void OnLook(InputAction.CallbackContext context)
    {
        //float w = mouseMove.magnitude * mouse_sensitivity;
        //Vector3 rotAxis = new Vector3(-mouseMove.y, mouseMove.x, 0f);
        //Vector2 moveVector = mouseMove.normalized;
        //Quaternion rotation = new Quaternion(w, 0f, moveVector.x, moveVector.y);
        //Debug.Log($"Rotation: {rotation.normalized}");
        //Quaternion inversed = new Quaternion(rotation.w, -rotation.x, -rotation.y, -rotation.z);

        //Quaternion point = transform.Find("Camera").rotation;
        //Debug.Log($"Point: {point}");
        //rotation *= point * inversed;

        float pitch = mouseMove.y * mouse_sensitivity;
        float yaw = mouseMove.x * mouse_sensitivity;

        transform.Find("Camera").rotation *= Quaternion.AngleAxis(pitch, Vector3.left);
        transform.rotation *= Quaternion.AngleAxis(yaw, Vector3.up);

        //Vector2 lookInput = context.ReadValue<Vector2>();
        //Debug.Log($"Look Input: {lookInput}");
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        //Debug.Log("Moved");
        

        
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnEnterMenu(InputAction.CallbackContext context)
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
    }

    void Movement()
    {
        Vector3 moveVector = new Vector3(playerMove.x, 0f, playerMove.y);                   // UIS automatycznie normalizuje ten wektor
        moveVector *= maxSpeed;
        this.transform.Translate(moveVector * Time.deltaTime);
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
