/*
* Author: Lim En Xu Jayson
* Date: 8 December 2025
* Description: Controls pet NavMesh movement and state machine; provides location refs.
*/
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

    /// <summary>
    /// Initializes NavMesh agent and starts the current state.
    /// </summary>
    void Start()
    {
        npcAgent = GetComponent<NavMeshAgent>();
        currentState = "Idle";
        StartCoroutine(SwitchStates(currentState));

    }

    /// <summary>
    /// Moves the pet toward a patrol point while in Walking state.
    /// </summary>
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
    /// <summary>
    /// Faces the camera and pauses NavMesh rotation while playing.
    /// </summary>
    IEnumerator Playing()
    {
        Debug.Log("Started Playing");
        npcAgent.isStopped = true;
        npcAgent.updateRotation = false; // Disable automatic rotation
        // Face the camera
        Transform cameraTransform = Camera.main.transform;
        Vector3 directionToCamera = cameraTransform.position - transform.position;
        directionToCamera.y = 0; // Keep only the horizontal direction
        Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
        while (currentState == "Playing")
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f);
            yield return null;
        }
    }

    /// <summary>
    /// Idles, samples a new patrol point on NavMesh, and waits.
    /// </summary>
    IEnumerator Idle()
    {
        npcAgent.isStopped = true;
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
            Debug.LogWarning("SamplePosition failed");
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
    /// <summary>
    /// After a delay, transitions from Idle to Walking.
    /// </summary>
    IEnumerator IdleTimer()
    {
        yield return new WaitForSeconds(3f);
        if (currentState == "Idle")
        {

            StartCoroutine(SwitchStates("Walking"));

        }
    }
    /// <summary>
    /// Stops the current state coroutine and starts the new one.
    /// </summary>
    public IEnumerator SwitchStates(string newState)
    {
        npcAgent.updateRotation = true; // Re-enable automatic rotation
        npcAgent.isStopped = false; // Resume the agent's movement
        StopCoroutine(currentState);
        currentState = newState;
        StartCoroutine(currentState);
        Debug.Log("Switched to state: " + currentState);
        yield return null;
    }
    /// <summary>
    /// Converts world position to scaled grid offset for AR plane.
    /// </summary>
    public Vector3 GetPetPosition(float smallScale)
    {
        float scaleFactor = smallScale / GameObject.Find("Plane Ref").transform.localScale.x;
        Vector3 gridPosition = GameObject.Find("Plane Ref").transform.position;
        Vector3 petWorldPosition = transform.position;
        gridPosition = petWorldPosition - gridPosition;
        Vector2 scaleOffset = new Vector2(gridPosition.x * scaleFactor, gridPosition.z * scaleFactor);
        return new Vector3(scaleOffset.x, 0, scaleOffset.y);

    }
    /// <summary>
    /// Returns the pet's current rotation.
    /// </summary>
    public Quaternion GetRotation()
    {
        return transform.rotation;
    }
    /// <summary>
    /// Turns pet toward food smoothly.
    /// </summary>
    public IEnumerator FaceFood(Quaternion foodRotation)
    {
        StartCoroutine(SwitchStates("Idle"));
        npcAgent.destination = transform.position; // Stop moving
        while (Quaternion.Angle(transform.rotation, foodRotation) > 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, foodRotation, Time.deltaTime * 2f);
            yield return null;
        }
        yield return null;
        
    }
}
