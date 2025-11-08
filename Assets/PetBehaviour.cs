using UnityEngine;

using System.Collections;
using UnityEngine.AI;
public class PetBehaviour : MonoBehaviour
{
    
    private NavMeshAgent npcAgent;
    public string currentState;

    public Vector3 patrolPoint;
    [SerializeField]
    float walkRadius;

    void Start()
    {
        npcAgent = GetComponent<NavMeshAgent>();
        currentState = "Idle";
        StartCoroutine(SwitchStates(currentState));
    }

    IEnumerator Walking()
    {
        while (currentState == "Walking")
        {
            if (npcAgent.remainingDistance <= npcAgent.stoppingDistance)
            {

                StartCoroutine(SwitchStates("Idle"));
            }
            npcAgent.SetDestination(patrolPoint);
            yield return null;
        }

    }


    IEnumerator Idle()
    {
        Debug.Log("Started Idle");
        StartCoroutine(IdleTimer());
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
        patrolPoint = hit.position;
        while (currentState == "Idle")
        {
            npcAgent.isStopped = true;
            yield return null; // Wait for the next frame
        }

    }
    IEnumerator IdleTimer()
    {
        yield return new WaitForSeconds(5);
        if (currentState == "Idle")
        {
            
                StartCoroutine(SwitchStates("Walking"));

        }
    }
        public IEnumerator SwitchStates(string newState)
    {
        npcAgent.updateRotation = true; // Re-enable automatic rotation
        npcAgent.isStopped = false; // Resume the agent's movement
        StopCoroutine(currentState);
        currentState = newState;
        StartCoroutine(currentState);
        yield return null;
    }
    
}
