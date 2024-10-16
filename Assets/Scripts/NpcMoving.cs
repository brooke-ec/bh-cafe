using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcMoving : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform[] waypoints;
    private int destWaypoints = 0;
    private NavMeshAgent agent;
    private Animator animator;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
   
    
    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            moveToPoint();
        }
    }

    void moveToPoint()
    { 
        if(waypoints.Length <= destWaypoints)
        {
            animator.SetFloat("Speed", 0f);
            return;
        }

        agent.destination = waypoints[destWaypoints].position;
        destWaypoints++;
        animator.SetFloat("Speed", 1.1f);
    }
}
