using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyPathFindingBehaviour : MonoBehaviour
{
    public bool isRabid = false;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float nextWaypointDistance;
    [SerializeField] private Animator anim;
    [SerializeField] private float chaseRange;
    private Seeker seeker;
    private PlayerController playerRef;
    private int currWayPoint;
    private bool reachedEoP, followForDuration;
    private Rigidbody2D RB;
    private Transform target, currentTarget;
    private Vector2 moveDirection;
    private float duration;
    private Path path;
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        RB = GetComponent<Rigidbody2D>();
        InvokeRepeating("updatePath", 0f, 0.5f);
        playerRef = PlayerController.instance;
        if (PlayerStatusSystem.instance.gameType == GameMode.ArenaMode)
        {
            // In Arena mode, Monster can always come to you
            chaseRange = float.PositiveInfinity;
        }
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
        if (!isRabid)
        {
            if (playerRef.gameObject.activeInHierarchy)
            {
                if (followForDuration)
                {
                    duration -= Time.deltaTime;
                    if (duration <= 0)
                    {
                        followForDuration = false;
                        //target = currentTarget;
                    }
                }
                else
                {
                    if (Vector2.Distance(transform.position, currentTarget.position) < chaseRange)
                    {
                        // Only chase the Player when within range

                        target = currentTarget;
                    }
                    else
                    {
                        target = null;

                    }
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
                moveDirection = ((Vector2)path.vectorPath[currWayPoint] - RB.position).normalized; // Calculates the path from current position to the next point in the Path
                RB.velocity = moveDirection * moveSpeed;

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
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
            }
            else
            {
                target = null;
                RB.velocity = Vector2.zero;
            }
            //Animating the enemy
            if (target != null)
            {
                anim.SetBool("isMoving", true);

            }
            else
            {
                anim.SetBool("isMoving", false);
            }
        }
        else
        {
            // If a monster is rabid/crazy, it can detect the player further
            if(Vector2.Distance(transform.position, currentTarget.position) < chaseRange + 3f) 
            { 
                moveDirection = currentTarget.position - transform.position;
            }
            else
            {
                moveDirection = Vector2.zero;
            }
            moveDirection.Normalize();
            // Rabid enemies move faster
            RB.velocity = moveDirection * moveSpeed * 1.1f;
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
    public void SetFollowTarget(Vector3 position, float followDuration)
    {
        if (!followForDuration)
        {
            target.position = position;
            followForDuration = true;
            duration = followDuration;
        }
    }
    public void SetFollowTarget(Transform followee)
    {
        currentTarget = followee;
    }
    public void ChangeMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }
}
