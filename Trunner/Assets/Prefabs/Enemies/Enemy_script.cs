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
    Vector3 target;

    //  GAMEPLAY

    float attack_range = 3f;
    float attack_cooldown = 2f;
    bool can_attack;

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

        agent.stoppingDistance = attack_range;

        // #### OTHER ####

        StartCoroutine(AttackCooldown());
    }

    void Update()
    {
        target = player.transform.position;
        if (Vector3.Distance(target, transform.position) > agent.stoppingDistance)
        {
            agent.destination = target;
            if (anim.GetBool("isWalking") == false && can_attack)
            {
                // Need to apply rotation here, so that it is in the appropriate direction
                anim.Play("InitiateAttack");
            }
        }
        
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

    void Attack()
    {

        _ = Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, attack_range);

        if(hit.collider != null)
        {
            hit.collider.SendMessage("Death");          
        }
    }

    void StartCooldown()
    {
        StartCoroutine(AttackCooldown());
    }
    IEnumerator AttackCooldown()
    {
        can_attack = false;
        yield return new WaitForSeconds(attack_cooldown);
        can_attack = true;
    }
}
