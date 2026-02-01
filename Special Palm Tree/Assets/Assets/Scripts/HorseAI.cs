using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HorseAI : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;
    private LightController targetLight;
    private bool isOnALight = false;

    public float detectionRadius = 3f;
    public float chaseTime = 2f;
    public Transform player;
    public float horseSpeed = 3.5f; 

    private float chasingTImer = 0f;

    public LayerMask playerLayer;   
    public LayerMask obstacleLayer;

    void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = horseSpeed;
    }

    void Update()
    {
        HandlePlayerDetection();

        if (isOnALight) return;

        if (chasingTImer > 0)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            targetLight = null;
        } else
        {
            HandleCandleHunting();
        }
    }

    void HandlePlayerDetection()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);

        if (playerCollider != null)
        {
            Vector2 direction = (playerCollider.transform.position - transform.position).normalized;
            float distanceToPlayer = Vector2.Distance(transform.position, playerCollider.transform.position);

            LayerMask combinedMask = obstacleLayer | playerLayer;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRadius, combinedMask);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    chasingTImer = chaseTime;
                    player = playerCollider.transform;
                    return;
                }
            }
        }

        CountdownTimer();
    } 

    private void CountdownTimer()
{
    if (chasingTImer > 0)
    {
        chasingTImer -= Time.deltaTime;
    }
}
    void HandleCandleHunting()
    {
        if (targetLight == null || targetLight.isExtinguished || !targetLight.isOn)
        {
            FindNewTarget();
        } else {

                agent.SetDestination(targetLight.transform.position);

                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.1f)
            {
                StartCoroutine(ProcessLight());
            }
        }
    }

    void FindNewTarget()
    {
        LightController[] allLights = Object.FindObjectsByType<LightController>(FindObjectsSortMode.None);

        var validLights = allLights
        .Where(l => l.isOn && !l.isExtinguished)
        .OrderBy(l => Vector2.Distance(transform.position, l.transform.position))
        .ToList();

        if (validLights.Count > 0)
        {
            targetLight = validLights[0];
        }
    }

    IEnumerator ProcessLight()
    {
        isOnALight = true;
        agent.isStopped = true;

        float elapsed = 0;
        while (elapsed < 3f)
        {
            if (chasingTImer > 0)
            {
                isOnALight = false;
                agent.isStopped = false; 
                yield break; 
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (targetLight != null)
        {
            targetLight.Extinguish();
        }

        targetLight = null;
        agent.isStopped = false;
        isOnALight = false;
    }

    void LateUpdate()
    {
        Vector3 nextPos = agent.nextPosition;
        transform.position = new Vector3(nextPos.x, nextPos.y, 0f);
        agent.nextPosition = transform.position;
    }

    // Visual aid in the editor to see the detection range
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
