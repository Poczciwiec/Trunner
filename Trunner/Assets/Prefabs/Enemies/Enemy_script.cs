using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_script : MonoBehaviour
{
    //#### VARIABLES ####

    NavMeshAgent agent;
    GameObject player;
    Animator anim;
    Vector3 target;             // used for navigation
    Vector3 lookTarget;         // used for rotation

    //  GAMEPLAY

    float attack_range = 3f;
    float attack_cooldown = 2f;
    float attack_rotation_speed = 12f;
    bool off_cooldown;
    bool is_attacking;

    void Awake()
    {
        try
        {
            agent = GetComponent<NavMeshAgent>();
        }
        catch
        {
            throw new Exception("Missing NavMeshAgent component!");
        }   // NavMeshAgent Component
        try
        {
            player = GameObject.Find("Player");
        }
        catch
        {
            throw new Exception("Player not found!");
        }   // Player GameObject
        try
        {
            anim = transform.GetChild(0).GetComponent<Animator>();
        }
        catch
        {
            throw new Exception("Missing Animator component!");
        }   // Animator Component
    }

    private void Start()
    {
        // #### NAVIGATION SETTINGS ####

        agent.stoppingDistance = attack_range - 0.5f;

        // #### OTHER ####

        StartCoroutine(AttackCooldown());
    }

    void Update()
    {
        target = player.transform.position;
        if (Vector3.Distance(target, transform.position) > agent.stoppingDistance)
        {
            agent.updateRotation = true;
            agent.destination = target;

            if (is_attacking) agent.isStopped = true;
            else agent.isStopped = false;
        }
        else
        {
            agent.isStopped = true;
        }

        if (InRange()) Attack();
        
    }

    private void FixedUpdate()
    {

        if (agent.velocity.magnitude > 0f)
        {
            anim.SetBool("isWalking", true);
            anim.speed = agent.velocity.magnitude / 1.4f;
        }
        else
        {
            anim.SetBool("isWalking", false);
            anim.speed = 1f;
        }
    }


    // #### METHODS ####

    bool InRange()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= attack_range)
        {
            return true;
        }
        else return false;
    }

    void Attack()
    {
        agent.updateRotation = false;
        if (off_cooldown && !is_attacking)
        {
            anim.Play("InitiateAttack");
            is_attacking = true;
        }

        Vector3 playerXZ = new Vector3(player.transform.position.x, 0f, player.transform.position.z);
        lookTarget = -(transform.position - playerXZ);
        Quaternion targetRotation = Quaternion.LookRotation(lookTarget, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, attack_rotation_speed * Time.deltaTime);

    }

    void ApplyDamage()
    {

        _ = Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, attack_range);

        if(hit.collider != null)
        {
            hit.collider.SendMessage("Death", SendMessageOptions.DontRequireReceiver);          
        }
    }

    void StartCooldown()
    {
        StartCoroutine(AttackCooldown());
        is_attacking = false;
    }
    IEnumerator AttackCooldown()
    {
        off_cooldown = false;
        yield return new WaitForSeconds(attack_cooldown);
        off_cooldown = true;
    }
}
