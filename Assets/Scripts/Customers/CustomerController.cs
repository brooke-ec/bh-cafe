using UnityEngine;
using UnityEngine.AI;

public class CustomerController : MonoBehaviour, ICollidable
{
    [SerializeField] private float fallOverTime;
    public TableController table;
    public Transform exit;

    private System.Action state;
    private NavMeshAgent agent;
    private Animator animator;
    private float timeFallen;

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
        agent.destination = table.sitAnchor.position;
        if (PathingComplete())
        {
            SetState(Sitting);
        }
    }

    private void Sitting()
    {
        transform.SetPositionAndRotation(table.sitAnchor.position, table.sitAnchor.rotation);
    }

    private void Leaving()
    {
        agent.destination = exit.position;
        if (PathingComplete()) Destroy(gameObject);
    }

    private void Fallen()
    {
        agent.ResetPath();
        timeFallen -= Time.deltaTime;
        if (timeFallen < 0) SetState(Leaving);
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

    public bool IsSitting()
    {
        return state == Sitting;
    }

    public void Collide(PlayerController player)
    {
        if (IsSitting() || state == Fallen) return;
        player.Fall(transform.position);
        timeFallen = fallOverTime;
        God.instance.levelUIManager.LoseHeart();
        Leave();
    }

    public void Leave()
    {
        SetState(Leaving);
    }
}
