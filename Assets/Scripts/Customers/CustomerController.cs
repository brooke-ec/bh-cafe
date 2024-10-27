using UnityEngine;
using UnityEngine.AI;

public class CustomerController : MonoBehaviour, IInteractable
{
    [SerializeField] private float fallOverTime;
    [SerializeField] private GameObject pickup;
    public TableController table;
    public Transform exit;
    public float waitTime;

    private PlayerController player;
    private NavMeshAgent agent;
    private Animator animator;
    private float timeFallen;
    private State state;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        state = State.Arriving;
    }
    
    // A finite state machine
    private void Update()
    {
        animator.SetTrigger(state.ToString());
        switch (state)
        {
            case State.Arriving:
                agent.destination = table.sitAnchor.position;
                if (PathingComplete()) state = State.Sitting;
                break;
            case State.Sitting:
                Sitting();
                break;
            case State.Leaving:
                agent.destination = exit.position;
                if (PathingComplete()) Destroy(gameObject);
                break;
            case State.Fallen:
                agent.ResetPath();
                timeFallen -= Time.deltaTime;
                if (timeFallen < 0) state = State.Leaving;
                break;
        }
    }

    private void Sitting()
    {
        transform.SetPositionAndRotation(table.sitAnchor.position, table.sitAnchor.rotation);
        waitTime -= Time.deltaTime;
        if (waitTime < 0)
        {
            state = State.Leaving;
            Fail();
        }
    }

    private bool PathingComplete()
    {
        return !agent.pathPending && agent.remainingDistance < 0.5f;
    }
    private void Fail()
    {
        if (waitTime < 0) return;
        waitTime = -1;
    }

    public bool IsSitting()
    {
        return state == State.Sitting;
    }

    public bool HeldCorrect()
    {
        return player.heldItem != null;
    }

    public void Collide(PlayerController player)
    {
        if (IsSitting() || state == State.Fallen) return;
        player.Fall(transform.position);
        timeFallen = fallOverTime;
        state = State.Fallen;
        Fail();
    }

    void IInteractable.Interact(PlayerController player)
    {
        if (!HeldCorrect()) return;
        state = State.Leaving;
        player.ClearHeld();

        for (int i = 0; i < 5; i++) Instantiate(
            pickup,
            transform.position + Vector3.up * 1,
            Quaternion.identity
        );
    }

    bool IInteractable.IsActive()
    {
        return HeldCorrect();
    }

    string IInteractable.GetText()
    {
        return (HeldCorrect() ? "Serve" : "Coffee") + $" ({Mathf.RoundToInt(waitTime)})";
    }

    bool IInteractable.IsVisible()
    {
        return IsSitting();
    }

    Vector3 IInteractable.GetOffset()
    {
        return Vector3.up * 3;
    }

    enum State
    {
        Arriving,
        Sitting,
        Leaving,
        Fallen
    }
}
