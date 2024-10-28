using UnityEngine;
using UnityEngine.AI;

public class CustomerController : MonoBehaviour, IInteractable
{
    [SerializeField] private float fallOverTime;
    [SerializeField] private GameObject pickup;
    [SerializeField] private float waitTime;
    
    [SerializeField] private OrderType[] options;
    
    [HideInInspector] public TableController table;
    [HideInInspector] public Transform exit;

    private PlayerController player;
    private bool canFail = true;
    private NavMeshAgent agent;
    private Animator animator;
    private float timeFallen;
    private OrderType order;
    private State state;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        order = Util.PickRandom(options);
        SetState(State.Arriving);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (
            !IsSitting()
            || other.attachedRigidbody == null
            || !other.TryGetComponent(out Item item)
            || item.GetType() != order
        ) return;

        Destroy(other.gameObject);
        Succeed();
    }

    // A finite state machine
    private void Update()
    {
        switch (state)
        {
            case State.Arriving:
                agent.destination = table.sitAnchor.position;
                if (PathingComplete()) Sit();
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
                if (timeFallen < 0) SetState(State.Leaving);
                break;
        }
    }

    private void Sit()
    {
        God.instance.levelUIManager.AddNewOrder(Mathf.CeilToInt(waitTime), order.sprite, table.tableNumber);
        SetState(State.Sitting);
    }

    private void Sitting()
    {
        transform.SetPositionAndRotation(table.sitAnchor.position, table.sitAnchor.rotation);
        waitTime -= Time.deltaTime;
        if (waitTime < 0)
        {
            SetState(State.Leaving);
            Fail();
        }
    }

    private bool PathingComplete()
    {
        return !agent.pathPending && agent.remainingDistance < 0.5f;
    }

    private void Fail()
    {
        if (!canFail) return;
        God.instance.levelUIManager.LoseHeart();
        canFail = false;
    }

    private void Succeed()
    {
        canFail = false;
        SetState(State.Leaving);
        God.instance.levelUIManager.ModifyScore(order.points);
        God.instance.levelUIManager.RemoveOrder(table.tableNumber);
        for (int i = 0; i < Random.Range(1, 3); i++) Instantiate(
            pickup,
            transform.position + Vector3.up * 1,
            Quaternion.identity
        );
    }

    private void SetState(State state)
    {
        animator.SetTrigger(state.ToString());
        this.state = state;
    }

    public bool IsSitting()
    {
        return state == State.Sitting;
    }

    public bool HoldingCorrect()
    {
        return player.heldItem != null && player.heldItem.GetType() == order;
    }

    public void Collide(PlayerController player)
    {
        if (IsSitting() || state == State.Fallen) return;
        player.Fall(transform.position);
        timeFallen = fallOverTime;
        SetState(State.Fallen);
        Fail();
    }

    void IInteractable.Interact(PlayerController player)
    {
        if (!HoldingCorrect()) return;
        player.ClearHeld();
        Succeed();
    }

    bool IInteractable.IsActive()
    {
        return HoldingCorrect();
    }

    string IInteractable.GetText()
    {
        return (HoldingCorrect() ? "Serve" : order.label) + $" ({Mathf.RoundToInt(waitTime)})";
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
