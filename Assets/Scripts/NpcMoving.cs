using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMoving : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed = 5.0f;
    public Transform[] waypoints;
    private Transform waypoint;
    public int destWaypoints = 0;
    void Start()
    {
        waypoint = waypoints[destWaypoints];
        transform.LookAt(new Vector3(waypoint.position.x, transform.position.y, waypoint.position.z));
    }

    // Update is called once per frame
    void Update()
    {

        if(new Vector3(waypoint.position.x, transform.position.y, waypoint.position.z)==transform.position &&destWaypoints<waypoints.Length)
        {
            switchPoint();
        }
        else if(new Vector3(waypoint.position.x, transform.position.y, waypoint.position.z) == transform.position && destWaypoints==waypoints.Length)
        {
            transform.Translate(0, 0, 0);

        }
        else
        {
            moveToPoint();
        }
    }

    void moveToPoint()
    {
        transform.LookAt(new Vector3(waypoint.position.x, transform.position.y, waypoint.position.z));
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
    
    void switchPoint()
    {
        destWaypoints += 1;
        if (waypoints.Length > destWaypoints)
        {
            waypoint = waypoints[destWaypoints];
        }
    }
}
