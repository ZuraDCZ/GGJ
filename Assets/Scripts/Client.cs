using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public enum ClientState
{
    NONE,
    WAITING,
    SAT,
    EATING,
    DONE
}

public class Client : MonoBehaviour
{
    //Properties-----------------------------------------
    public ClientState state;
    [SerializeField] private float patienceLvl = 100f;
    [SerializeField] private float eatingTime = 30f;
    public float currentPatience;
    public float currentEatingTime;
    private Table usedTable;

    public delegate void OnSat(); //Client sat event
    public OnSat onSat;

    public delegate void OnServed(); //Client served event
    public OnServed onServed;

    //Movement requirements------------------------------
    [SerializeField]private Transform target;
    [SerializeField]private float speed = 200f;
    [SerializeField]private float nextWaypointDist = 3f;
    Path path;
    int currentWaypoint = 0;
    bool reachedEnd = false;

    Seeker seeker;
    Rigidbody2D rb;
    //--------------------------------------------------------

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        SetState(ClientState.NONE);
    }

    private void Start()
    {
        //Set timers
        currentPatience = patienceLvl;
        currentEatingTime = eatingTime;
        
        //Subscribe to delegates
        if (onServed != null)
        {
            onServed += StartEating;
        }

    }

    private void Update()
    {
        HandleBehaviour();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        //If there is no path, do nothing
        if (path == null) 
            return;

        //Checks if the agent reached its destination 
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEnd = true;
            return;
        }
        else
        {
            reachedEnd = false;
        }

        //Gets the direction where the agent needs to go
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.fixedDeltaTime;

        //Moves the agent towrads the direction calculated
        rb.AddForce(force, ForceMode2D.Impulse);

        //Gets distance to next point to select the next one
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

    /// <summary>
    /// Sets agent target to go towards to
    /// </summary>
    /// <param name="targetPosition"></param>
    public void UpdatePath(Vector3 targetPosition)
    {
        //if (seeker.IsDone())
        //{
        //    seeker.StartPath(rb.position, targetPosition, OnPathComplete);
        //}
        seeker.StartPath(rb.position, targetPosition, OnPathComplete);
    }


    /// <summary>
    /// Updates agent state
    /// </summary>
    /// <param name="newState"></param>
    public void SetState(ClientState newState)
    {
        state = newState;

        //Handles the start of each state
        switch (state)
        { 
            case ClientState.NONE:
                break;

            case ClientState.WAITING:
                LvlManager.instance.clientsWaiting.Add(this);
                break;

            case ClientState.SAT:
                LvlManager.instance.clientsWaiting.Remove(this);
                LvlManager.instance.clientsSat.Add(this);
                break;

            case ClientState.EATING:
                LvlManager.instance.clientsSat.Remove(this);
                LvlManager.instance.clientsEating.Add(this);
                break;

            case ClientState.DONE:
                if (LvlManager.instance.clientsWaiting.Contains(this))
                {
                    LvlManager.instance.clientsWaiting.Remove(this);
                }
                else if(LvlManager.instance.clientsSat.Contains(this))
                {
                    LvlManager.instance.clientsSat.Remove(this);
                }
                else if (LvlManager.instance.clientsEating.Contains(this))
                {
                    LvlManager.instance.clientsEating.Remove(this);
                }
                usedTable.Unoccupy();
                usedTable = null;
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Returns agent state
    /// </summary>
    /// <returns></returns>
    public ClientState GetState()
    {
        return state;
    }

    /// <summary>
    /// Handles agent states during Update
    /// </summary>
    public void HandleBehaviour()
    {
        switch (state)
        { 
            case ClientState.NONE:
                break;

            case ClientState.WAITING:
                currentPatience -= Time.deltaTime;
                if(currentPatience <= 0)
                {
                    SetState(ClientState.DONE);
                }
                break;

            case ClientState.SAT:
                currentPatience -= Time.deltaTime;
                if (currentPatience <= 0)
                {
                    SetState(ClientState.DONE);
                }
                break;

            case ClientState.EATING:
                currentEatingTime -= Time.deltaTime;
                if (currentEatingTime <= 0)
                {
                    SetState(ClientState.DONE);
                }
                break;

            case ClientState.DONE:
                Leave();
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Notifies the client to start eating
    /// </summary>
    public void StartEating()
    {
        SetState(ClientState.EATING);
    }

    /// <summary>
    /// Notifies the client to leave through the exit
    /// </summary>
    private void Leave()
    {
        UpdatePath(LvlManager.instance.exitPosition.position);
    }

    public void SetTable(Table table)
    {
        usedTable = table;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Killzone"))
        {
            Destroy(gameObject);
        }
    }
}
