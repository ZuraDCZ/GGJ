using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Pathfinding;

public class Client : MonoBehaviour
{
    //Properties
    //private float patienceLvl = 100f;
    private float eatDuration = 20f;
    //private bool served = false;
    //private bool doneEating = false;

    //Movement requirements
    [SerializeField]private Transform target;
    [SerializeField]private float speed = 200f;
    [SerializeField]private float nextWaypointDist = 3f;
    Path path;
    int currentWaypoint = 0;
    bool reachedEnd = false;

    private delegate void OnReachedGoal();
    private OnReachedGoal onReachedGoal;

    Seeker seeker;
    Rigidbody2D rb;

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEnd = true;
            onReachedGoal.Invoke();
            return;
        }
        else
        {
            reachedEnd = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.fixedDeltaTime;

        rb.AddForce(force, ForceMode2D.Impulse);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDist)
        {
            currentWaypoint++;
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    public void SetTarget(Vector3 targetPosition)
    {
        seeker.StartPath(rb.position, targetPosition, OnPathComplete);
    }
}
