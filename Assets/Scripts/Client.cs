using Pathfinding;
using UnityEngine;

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
    [SerializeField] float patienceLvl = 100f;
    [SerializeField] float eatingTime = 30f;
    public float currentPatience;
    public float currentEatingTime;
    private bool orderDone = false;
    private bool served = false;

    //References-----------------------------------------
    private Table usedTable;
    private Food selectedFood;
    private int selectedFoodIndex;
    [SerializeField] ParticleSystem ps;

    public delegate void OnOrder();
    public OnOrder onOrder;

    //Movement requirements------------------------------
    [SerializeField] Vector3 target;
    [SerializeField] float speed = 200f;
    [SerializeField] float nextWaypointDist = 3f;
    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEnd = false;

    Seeker seeker;
    Rigidbody2D rb;
    Animator animator;

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        ps = GetComponent<ParticleSystem>();
        SetState(ClientState.NONE);
    }

    private void Start()
    {
        //Subscribe delegates
        onOrder += OrderDone;

        //Set timers
        currentPatience = patienceLvl;
        currentEatingTime = eatingTime;

        //Set the the ordered food ID
        int foodAmount = FoodSpawner.instance.GetSpritesLength();
        selectedFoodIndex = Random.Range(0, foodAmount);

        //Subscribe to path updating. Needed to move
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    private void Update()
    {
        HandleBehaviour();
    }

    private void FixedUpdate()
    {
        //if (GameManager.instance.GetGameState() == GameState.Playing)
        //{

        //}
        if (Vector2.Distance(transform.position, target) > 1)
        {
            HandleMovement();
        }
        //Animation logic
        if (rb.velocity.sqrMagnitude != 0)
        {
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }
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
        Vector2 force = direction * speed * Time.deltaTime;

        //Moves the agent towards the direction calculated
        rb.AddForce(force, ForceMode2D.Impulse);

        //Gets distance to next point to select the next one
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDist)
        {
            currentWaypoint++;
        }
    }

    /// <summary>
    /// Calculates path towards target
    /// </summary>
    /// <param name="targetPosition"></param>
    public void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target, OnPathComplete);
        }
    }

    /// <summary>
    /// Resets waypoint index on complete path
    /// </summary>
    /// <param name="p"></param>
    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    /// <summary>
    /// Sets new target position
    /// </summary>
    /// <param name="newTarget"></param>
    public void SetTarget(Vector3 newTarget)
    {
        target = newTarget;
    }

    /// <summary>
    /// Updates agent state on trigger
    /// </summary>
    /// <param name="newState"></param>
    public void SetState(ClientState newState)
    {
        state = newState;

        ///TODO: Animation logic

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
                    LvlManager.instance.clientsWaiting.Remove(this);

                else if (LvlManager.instance.clientsSat.Contains(this))
                    LvlManager.instance.clientsSat.Remove(this);

                else if (LvlManager.instance.clientsEating.Contains(this))
                    LvlManager.instance.clientsEating.Remove(this);


                if (usedTable != null)
                    usedTable.Unoccupy();


                if (Served())
                    LvlManager.instance.AddScore(this);
                else if (!Served())
                    LvlManager.instance.LoseLife();

                usedTable.Unoccupy();
                usedTable = null;

                break;

            default:
                break;
        }
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
                if (currentPatience <= 0)
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
                    selectedFood.gameObject.SetActive(false);
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
    /// Notifies the client to leave through the exit and updates score depending on served state
    /// </summary>
    private void Leave()
    {
        SetTarget(LvlManager.instance.exitPosition.position);
    }

    public ClientState GetState()
    {
        return state;
    }

    public void SetTable(Table table)
    {
        usedTable = table;
    }

    public Table GetTable()
    {
        return usedTable;
    }

    public int GetOrder()
    {
        return selectedFoodIndex;
    }

    public void SetFood(Food food)
    {
        selectedFood = food;
    }

    private void OrderDone()
    {
        orderDone = true;
    }

    public bool HasOrdered()
    {
        return orderDone;
    }

    public float GetPatience()
    {
        return currentPatience;
    }

    public void SetServed()
    {
        served = true;
    }

    public bool Served()
    {
        return served;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Killzone")
        {
            Debug.Log("Kill");
            Destroy(gameObject);
        }
    }
}