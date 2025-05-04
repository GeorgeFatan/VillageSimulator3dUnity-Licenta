using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RutaNPC : MonoBehaviour
{
    public Transform[] waypoints; //pct traseu
    private NavMeshAgent agent;
    private int currentWaypoint = 0;
   

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        MoveToNextWaypoint();

    }

    private void Update()
    {
        if(!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            MoveToNextWaypoint();
        }
    }

    void MoveToNextWaypoint()
    {
        if(waypoints.Length == 0)
        
            return;
        agent.destination = waypoints[currentWaypoint].position;
        currentWaypoint = (currentWaypoint +1) % waypoints.Length;
    }
}
