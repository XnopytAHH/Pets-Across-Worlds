using UnityEngine;

using System.Collections;
using UnityEngine.AI;
using Unity.XR.CoreUtils;
using TMPro;
using UnityEngine.UIElements;
using UnityEditor.Analytics;
public class PetLocationRef : MonoBehaviour
{
    
    private NavMeshAgent npcAgent;
    public string currentState;

    public Vector3 patrolPoint;
    [SerializeField]
    float walkRadius;
    public bool petAwake = true;
    [SerializeField]
    private GameObject petUI;
    [SerializeField]
    private GameObject restUI;

    void Start()
    {
        npcAgent = GetComponent<NavMeshAgent>();
        currentState = "Idle";
        StartCoroutine(SwitchStates(currentState));
        
    }

    IEnumerator Walking()
    {
        Debug.Log("Started Walking");
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

    // 1. Generate a random point on the ground only
    Vector2 circle = Random.insideUnitCircle * walkRadius;
    Vector3 randomDirection = new Vector3(circle.x, transform.position.y, circle.y);

    // 2. Convert to world position
    randomDirection += transform.position;

    // 3. Sample the NavMesh correctly (ALL AREAS)
    NavMeshHit hit;
    if (!NavMesh.SamplePosition(randomDirection, out hit, walkRadius, NavMesh.AllAreas))
    {
        Debug.LogWarning("SamplePosition FAILED — no NavMesh nearby");
        yield break;
    }

    patrolPoint = hit.position;
    Debug.Log("New Patrol Point: " + patrolPoint);

    // 4. Idle loop
    while (currentState == "Idle")
    {
        npcAgent.isStopped = true;
        yield return null;
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
    public Vector3 GetPetPosition(float smallScale)
    {
        float scaleFactor = smallScale/GameObject.Find("Plane Ref").transform.localScale.x;
        Vector3 gridPosition = GameObject.Find("Plane Ref").transform.position;
        Vector3 petWorldPosition = transform.position;
        gridPosition = petWorldPosition - gridPosition;
        Vector2 scaleOffset = new Vector2(gridPosition.x * scaleFactor, gridPosition.z * scaleFactor);
        return new Vector3(scaleOffset.x, 0, scaleOffset.y);

    }
}
