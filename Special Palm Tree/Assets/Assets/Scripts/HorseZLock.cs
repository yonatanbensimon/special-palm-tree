using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class HorseChaseTest : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform playerTransform; // Drag the Player object here
    
    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        // IMPORTANT: We take manual control of the position
        agent.updatePosition = false; 
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        if (playerTransform != null)
        {
            // Tell the advisor (Agent) where we want to go
            agent.SetDestination(playerTransform.position);
        }
    }

    void LateUpdate()
    {
        // 1. Get the 3D position the Agent calculated
        Vector3 nextPos = agent.nextPosition;

        // 2. Apply it to our transform, but FORCING Z to 0
        transform.position = new Vector3(nextPos.x, nextPos.y, 0f);

        // 3. Keep the Agent in sync with our actual position
        agent.nextPosition = transform.position;
    }
}