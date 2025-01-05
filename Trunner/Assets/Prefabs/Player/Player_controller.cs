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
    Animator anim;

    //#### MOVEMENT ####

    //   HORIZONTAL 
    private const float maxSpeed = 10f;
    Vector2 playerMove;
    //private float acceleration = 1.05f;
    //private float deceleration = 0.6f;
    //[SerializeField] private float moveSpeed = 1f;
    //bool isMoving = true;

    //   JUMPING 
    [SerializeField] private float jumpForce = 300f;
    LayerMask jumpRayMask;
    Collider ground_Detection;
    float lastY;
    bool triggerCollision;
    
    //   LOOK 
    GameObject player_Camera;
    float yaw_sensitivity = 0.1f;
    float pitch_sensitivity = 0.09f;
    float pitch;
    Vector2 mouseMove;


    //#### GAMEPLAY ####
    bool blockMovement = false;

    //   PRIMARY ATTACK
    bool off_primary_cooldown;
    float primary_range = 2f;
    float primary_cooldown = 2f;


    private void Awake()
    {
        try
        {
            player_Camera = transform.Find("Camera").gameObject;
        } 
        catch
        {
            throw new Exception("Player>Camera not found.");
        }
        try
        {
            ground_Detection = transform.GetComponent<BoxCollider>();
        }
        catch
        {
            throw new Exception("Ground detection box collider not found.");
        }
        try
        {
            anim = player_Camera.GetComponentInChildren<Animator>();
        }
        catch
        {
            throw new Exception("Animator Component not found!");
        }
    }
    private void Start()
    {
        // #### LOAD DEFAULT VALUES ####
        yaw_sensitivity = 0.1f;
        pitch_sensitivity = yaw_sensitivity - 0.05f;
        pitch = 0f;
        

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        transform.position = new Vector3(0f, 1f, 0f);           // UNLOCK
        jumpRayMask = LayerMask.GetMask("Environment");
    }


    private void Update()
    {
        mouseMove = controls.Player.Look.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        playerMove = controls.Player.Move.ReadValue<Vector2>();
        Movement();
    }


    // @@@@@@@@@@@@@@@ InputActions E/D @@@@@@@@@@@@@@@
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
    // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

    // #### ACTIONS ####
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

        pitch -= (mouseMove.y * pitch_sensitivity);
        float yaw = mouseMove.x * yaw_sensitivity;
        pitch = Math.Clamp(pitch, -80, 80);
        
        player_Camera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        
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
        // !!!! TEMPORARY !!!!
        if(Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        else if (off_primary_cooldown)
        {
            anim.Play("InitiateAttack");
        }
        StartCoroutine(AttackCooldown());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.1f, jumpRayMask);

        if (!triggerCollision)
        {
            Vector3 jump = new Vector3(0, jumpForce, 0);
            this.GetComponent<Rigidbody>().AddForce(jump);
        }
    }

    void OnTriggerStay(Collider other)
    {
        triggerCollision = false;
    }
    void OnTriggerExit(Collider other)
    {
        triggerCollision = true;
    }

    public void OnEnterMenu(InputAction.CallbackContext context)
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
    }

    // #### METHODS ####
    void Movement()
    {
        Vector3 moveVector = new Vector3(playerMove.x, 0f, playerMove.y);                   // UIS automatycznie normalizuje ten wektor
        moveVector *= maxSpeed;

        if (blockMovement)
        {
            moveVector = Vector3.zero;
        }
        transform.Translate(moveVector * Time.deltaTime);
    }

    void ApplyDamage()
    {
        _ = Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, primary_range);

        if (hit.collider != null)
        {
            if (hit.collider.tag == "Enemy") hit.collider.SendMessage("OnDamaged");
        }
    }
    IEnumerator AttackCooldown()
    {
        off_primary_cooldown = false;
        yield return new WaitForSeconds(primary_cooldown);
        off_primary_cooldown = true;
    }
    void Death()
    {
        blockMovement = true;
        
        // Death animation invoke here;
    }



    // #@#@ THE VOID #@#@   (unused methods)

    /*
    bool IsFalling()            // To execute every frame
    {
        float currentY = transform.position.y;
        if(currentY != lastY)
        {
            if(currentY < lastY)
            {
                // Player going down
                lastY = currentY;
                return true;
            }
            else
            {
                // Player going up
                lastY = currentY;
                return false;
            }
        } else
        {
            lastY = currentY;
            return false;
        }

        
    }

    float DetermineSpeed()  // TO DO
    {
        if (!isMoving && moveSpeed <= 0.1f) moveSpeed = 0.1f;
        else if (isMoving && moveSpeed < maxSpeed) moveSpeed *= acceleration;
        else if (isMoving && moveSpeed >= maxSpeed) moveSpeed = maxSpeed;
        else if (!isMoving && moveSpeed > 0.1f) moveSpeed *= deceleration;

        return moveSpeed;
    }
    */
}
