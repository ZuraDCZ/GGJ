using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEditor.PackageManager;

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
    private AIPath aiPath;
    private Table usedTable;

    //Properties-----------------------------------------
    public ClientState state;
    [SerializeField] private float patienceLvl = 100f;
    [SerializeField] private float eatingTime = 30f;
    public float currentPatience;
    public float currentEatingTime;

    public delegate void OnSat(); //Client sat event
    public OnSat onSat;

    public delegate void OnServed(); //Client served event
    public OnServed onServed;

    private void Awake()
    {
        aiPath = new AIPath();
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

    public void SetDestination(Vector3 destination)
    {
        aiPath.destination = destination;
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
                SetDestination(LvlManager.instance.waitPosition.position + new Vector3(0, -1 * (LvlManager.instance.clientsWaiting.Count - 1))); //Sets target to wait on position and makes a line of them
                LvlManager.instance.clientsWaiting.Add(this);
                break;

            case ClientState.SAT:
                gameObject.layer = 3;
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
                else if (LvlManager.instance.clientsSat.Contains(this))
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
        SetDestination(LvlManager.instance.exitPosition.position);
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
