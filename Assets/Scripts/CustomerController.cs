using UnityEngine;
using UnityEngine.AI;

public class CustomerController : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform[] waypoints;
    private int destWaypoints = 0;
    private NavMeshAgent agent;
    private Animator animator;
    public GameObject Table;
    public int x;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
   
    
    void Update()
    {
        if (agent.enabled && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            moveToPoint();
        }
        
    }

    void moveToPoint()
    { 
        if(waypoints.Length <= destWaypoints)
        {
            agent.ResetPath();
            animator.SetFloat("Speed", 0f);
            Table.GetComponent<tableController>().whenArrive(this);
            GetComponent<CapsuleCollider>().enabled = false;
            agent.enabled = false;
            animator.SetInteger("Sit", 1);
            x++;
            return;
        }

        agent.destination = waypoints[destWaypoints].position;
        destWaypoints++;
        animator.SetFloat("Speed", 1.1f);
    }
}
