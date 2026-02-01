using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;


public class HorseAI : MonoBehaviour
{
    [Header("Calibration")]
    public float detectionRadius = 3f;
    public float chaseTime = 2f;    
    public float horseSpeed = 3.5f; 
    public float chasingHorseSpeed = 4.5f;
    public float chasingEnragedHorseSpeed = 6.0f;

    private UnityEngine.AI.NavMeshAgent agent;
    private LightController targetLight;
    private bool isOnALight = false;

    
    public Transform player;
    

    private float chasingTImer = 0f;

    public LayerMask playerLayer;   
    public LayerMask obstacleLayer;

    public int maxHealth = 5;
    public int health = 5;

    private HorseVisuals visuals;

    private bool isRetreating = false;

    public void Retreat()
    {
        isRetreating = true;
        chasingTImer = 0; 
        
        Transform furthestCandle = FindFurthestCandle();
        if (furthestCandle != null)
        {
            agent.isStopped = false;
            agent.speed = horseSpeed; 
            agent.SetDestination(furthestCandle.position);
        }
        
        StartCoroutine(ResetRetreat(5f)); //Run away for only five seconds
    }

    private Transform FindFurthestCandle()
    {
        LightController[] allCandles = Object.FindObjectsByType<LightController>(FindObjectsSortMode.None);
        if (allCandles.Length == 0) return null;

        return allCandles
            .OrderByDescending(c => Vector2.Distance(transform.position, c.transform.position))
            .First().transform;
    }

    private IEnumerator ResetRetreat(float delay)
    {
        yield return new WaitForSeconds(delay);
        isRetreating = false;
    }

    void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        visuals = GetComponent<HorseVisuals>();

        if (player == null)
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }

    }

        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = horseSpeed;
    }

    void Update()
    {
        if (isRetreating) 
        {
            UpdateSpriteDirection();
            return; 
        }

        HandlePlayerDetection();
        UpdateSpriteDirection();

        if (isOnALight) return;
        
        bool isEnraged = (health == 1);

        if (chasingTImer > 0)
        {
            agent.speed = isEnraged ? chasingEnragedHorseSpeed : chasingHorseSpeed;
            agent.isStopped = false;
            agent.SetDestination(player.position);
            targetLight = null;
        } else
        {
            agent.speed = isEnraged ? chasingHorseSpeed : horseSpeed;
            HandleCandleHunting();
        }
    }

    void UpdateSpriteDirection()
{
    Vector2 velocity = agent.velocity;

    if (velocity.magnitude < 0.1f) return;

    if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
    {
        visuals.sideHorse.SetActive(true);
        visuals.upHorse.SetActive(false);

        float flipX = velocity.x > 0 ? 1f : -1f * Mathf.Abs(visuals.sideHorse.transform.localScale.x);
        visuals.sideHorse.transform.localScale = new Vector3(flipX, visuals.sideHorse.transform.localScale.y, 1f);
    }
    else
    {
        visuals.sideHorse.SetActive(false);
        visuals.upHorse.SetActive(true);

        float flipY = velocity.y > 0 ? 1f : -1f * Mathf.Abs(visuals.upHorse.transform.localScale.x);
        visuals.upHorse.transform.localScale = new Vector3(flipY,visuals.upHorse.transform.localScale.y, 1f);
    }
}

    private void Start()
    {
        health = maxHealth;
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
    }

    if (agent.hasPath)
    {
        agent.isStopped = false;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        {
            if (targetLight == null)
            {
               print("arrived at previous destinatiom");
            }
            else
            {
                StartCoroutine(ProcessLight());
            }
        }
    }
}

    void FindNewTarget()
    {
        List<LightController> allCandles = Object.FindObjectsByType<LightController>(FindObjectsSortMode.None)
            .Where(l => l.isOn && !l.isExtinguished)
            .ToList();

        HA2CharacterController playerController = player.GetComponent<HA2CharacterController>();
        
        Vector3 bestTargetPos = Vector3.zero;
        float closestDistance = float.MaxValue;
        targetLight = null; 

        foreach (var candle in allCandles)
        {
            float dist = Vector2.Distance(transform.position, candle.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                targetLight = candle;
                bestTargetPos = candle.transform.position;
            }
        }

        if (playerController != null)
        {
            if (playerController.IsLightOn()) 
            {
                float distToPlayer = Vector2.Distance(transform.position, player.position);
                if (distToPlayer < closestDistance)
                {
                    closestDistance = distToPlayer;
                    targetLight = null; 
                    bestTargetPos = player.position;
                }
            }
        }

        if (bestTargetPos != Vector3.zero)
        {
            agent.SetDestination(bestTargetPos);
        } else
        {
            agent.isStopped = true;
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

    public void TakeDamage()
    {
        // Audio cue + delay
        health--;
        var gd = HUD.Data;
        gd.horseHealth = health/maxHealth;
        HUD.Data = gd;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (PersistentGameData.Instance != null)
        {
            PersistentGameData.Instance.accessories.Clear();
        }

        var gd = HUD.Data;
        gd.horseHealth = 1f; 
        gd.playerHealth = 3; 
        gd.playerSanity = 1f;
        HUD.Data = gd;
        
        SceneManager.LoadScene("");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<HA2CharacterController>(out var playerController))
            {
                playerController.TakeDamage();
            }

            Retreat();
        }
    }

    // Visual aid in the editor to see the detection range
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
