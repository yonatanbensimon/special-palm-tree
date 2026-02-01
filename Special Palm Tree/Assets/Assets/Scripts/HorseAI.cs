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

    void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        if (isOnALight) return;

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

        yield return new WaitForSeconds(3f);

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
}
