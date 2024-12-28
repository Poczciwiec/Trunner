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
        
    }

    void Update()
    {
        target = player.transform.position;
        if(Vector3.Distance(target, transform.position) > agent.stoppingDistance) agent.destination = target;
        
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
        // attack animation invoke here;
        

        
    }
}
