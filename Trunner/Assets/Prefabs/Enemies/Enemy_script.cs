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
        }
        try
        {
            player = GameObject.Find("Player");
        }
        catch
        {
            throw new Exception("Player not found!");
        }
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


    // #### METHODS ####

    void Attack()
    {
        // attack animation invoke here;
        

        
    }
}
