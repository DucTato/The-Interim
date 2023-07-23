using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class ClairvoyanceScript : MonoBehaviour
{
    private Path path;
    private Seeker seeker;
    private Rigidbody2D objectRB;
    private Transform target;
    private int currWayPoint;
    private bool reachedEoP;
    private Vector2 moveDirection;
    // Start is called before the first frame update
    void Start()
    {
        objectRB = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        target = transform.root;
        InvokeRepeating("updatePath", 0f, 0.5f);
    }
    private void updatePath()
    {
        if (seeker.IsDone() && target != null) // Avoids calculation of new path when it already has one
            seeker.StartPath(objectRB.position, target.position, OnPathComplete);
    }
    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currWayPoint = 0;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null)
        {
            return;
        }
        if (currWayPoint >= path.vectorPath.Count)
        {
            reachedEoP = true;
            return;
        }
        else
        {
            reachedEoP = false;
        }

        moveDirection = ((Vector2)path.vectorPath[currWayPoint] - objectRB.position).normalized; // Calculates the path from current position to the next point in the Path
        objectRB.velocity = moveDirection * 12f;

        float distance = Vector2.Distance(objectRB.position, path.vectorPath[currWayPoint]);
        if (distance < 0.3f)
        {
            currWayPoint++;
        }
    }
}
