using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyPathFindingBehaviour : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float nextWaypointDistance;
    [SerializeField] private Animator anim;
    [SerializeField] private float chaseRange;
    private Seeker seeker;
    
    private int currWayPoint;
    private bool reachedEoP;
    private Rigidbody2D RB;
    private Transform target;

    private Path path;
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        RB = GetComponent<Rigidbody2D>();
        InvokeRepeating("updatePath", 0f, 0.5f);
        
    }
    private void updatePath()
    {
        if (seeker.IsDone() && target != null) // Avoids calculation of new path when it already has one
        seeker.StartPath(RB.position, target.position, OnPathComplete);
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
        if (Vector2.Distance(transform.position, PlayerController.instance.transform.position) < chaseRange)
        {
            // Only chase the Player when within range

            target = PlayerController.instance.transform;
        }
        else
        {
            target = null;
        }
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

        Vector2 moveDirection = ((Vector2)path.vectorPath[currWayPoint] - RB.position).normalized; // Calculates the path from current position to the next point in the Path
        RB.velocity = moveDirection * moveSpeed * Time.deltaTime;

        float distance = Vector2.Distance(RB.position, path.vectorPath[currWayPoint]);
        if (distance < nextWaypointDistance)
        {
            currWayPoint++;
        }

        if (RB.velocity.x >= 0.01f)
        {
            transform.localScale = Vector3.one;
        }
        else if (RB.velocity.x <= -0.01f)
        {
            transform.localScale = new Vector3 (-1f, 1f, 1f);
        }

        //Animating the enemy
        if (RB.velocity != Vector2.zero)
        {
            anim.SetBool("isMoving", true);

        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }
    
}
