using UnityEngine;
using UnityEngine.AI;

public class CustomerController : MonoBehaviour
{
    public TableController table;
    public Transform exit;

    private System.Action state;
    private NavMeshAgent agent;
    private Animator animator;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        SetState(Arriving);
    }
    
    // A finite state machine
    private void Update() { state(); }

    private void Arriving()
    {
        agent.destination = table.transform.position;
        if (PathingComplete())
        {
            SetState(Sitting);
        }
    }

    private void Sitting()
    {

    }

    private void Leaving()
    {
        agent.destination = exit.position;
        if (PathingComplete()) Destroy(gameObject);
    }

    private bool PathingComplete()
    {
        return !agent.pathPending && agent.remainingDistance < 0.5f;
    }

    private void SetState(System.Action state)
    {
        animator.SetTrigger(state.Method.Name);
        this.state = state;
    }
}
