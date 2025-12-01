using UnityEngine;

using System.Collections;
using UnityEngine.AI;
using Unity.XR.CoreUtils;
using TMPro;
public class PetBehaviour : MonoBehaviour
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
    void Update()
    {
        if(!petAwake)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            petUI.SetActive(false);
            restUI.SetActive(true);
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            petUI.SetActive(true);
            restUI.SetActive(false);

        }
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
    public void UpdateRestUI(string timeTillWake)
    {
        restUI.GetComponentInChildren<TextMeshProUGUI>().text = GameManager.instance.currentPlayerPets[gameObject.transform.parent.name].petName + " is asleep! They will wake up in " + timeTillWake.Substring(0, 8);
        
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
