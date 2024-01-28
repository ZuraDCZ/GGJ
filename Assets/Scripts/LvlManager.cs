using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LvlManager : MonoBehaviour
{
    public static LvlManager instance;
    [SerializeField] GameObject[] clientPrefab; //Clients prefabs array. Drag to inspector to add
    [SerializeField] Transform spawnPosition; //Transform whre the clients spawn. Not parented
    [SerializeField] Transform waitPosition; //Transform where the clients wait. Not parented
    public Transform exitPosition; //Transform where the clients exit the place. Not parented
    [SerializeField] float spawnRate; //Time it takes for a client to spawn
    [SerializeField] float sendRate; //Time it takes for a waitng client to be given a table
    private float currentSpawnTimer; //Keeps track of the time to spawn a client
    private float currentSendTimer; //Keeps track of the time to send a client
    List<Table> tables = new List<Table>(); //Tables on scene
    public List<Client> clientsWaiting = new List<Client>(); //List to keep track of the clients
    public List<Client> clientsSat = new List<Client>(); //List to keep track of the clients on table but not eating
    public List<Client> clientsEating = new List<Client>(); //List of the clients that are currently eating

    //Player score values
    private float score;
    [SerializeField] int maxLifes = 5;
    private int currentLifes;
    private void Awake()
    {
        InitializeLvl();
    }

    /// <summary>
    /// Sets properties and list for the game to run
    /// </summary>
    private void InitializeLvl()
    {
        instance = this;
        currentLifes = maxLifes;
        currentSpawnTimer = spawnRate; //Sets timers
        currentSendTimer = sendRate;
        foreach (Transform child in transform) //Gets the amount of tables on scene
        {
            Table table = child.GetComponent<Table>();
            tables.Add(table);
        }
    }

    private void Update()
    {
        if (GameManager.instance.GetGameState() == GameState.Playing)
        {
            GenerateClients();
            SendClients();
        }
    }

    /// <summary>
    /// Generates a random client from the clients array every certain time
    /// </summary>
    private void GenerateClients()
    {
        currentSpawnTimer -= Time.deltaTime; //Substracts time to spawn
        if (currentSpawnTimer <= 0 && clientsWaiting.Count < 1) //Checks if its time to spawn and if there is room to spawn another client
        {
            int randomIndex = Random.Range(0, clientPrefab.Length); //Selects a client to spawn from the prefab array
            Client newClient = Instantiate(clientPrefab[randomIndex], spawnPosition.position, Quaternion.identity).GetComponent<Client>(); //Spawns client at given location
            newClient.SetTarget(waitPosition.position + new Vector3(0, -1 * (clientsWaiting.Count - 1))); //Sets target to wait on position and makes a line of them
            newClient.SetState(ClientState.WAITING); //Sets waiting state
            currentSpawnTimer = spawnRate; //Resets timer
        }
    }

    /// <summary>
    /// Sends a client to Table
    /// </summary>
    private void SendClients()
    {
        if (clientsWaiting.Count > 0) //Checks if there are clients waiting
        {
            currentSendTimer -= Time.deltaTime; //Substracts time
            if (currentSendTimer <= 0) //Checks if its time to give a client a table
            {
                foreach (Table table in tables) //Look for each of the tables
                {
                    if (table != null)
                    {
                        if (!table.isOccupied()) //Get the first unnocupied one
                        {
                            Client clientToSit = clientsWaiting.ElementAt(0); //Gets the first one on queue
                            clientToSit.SetTarget(table.GetSit().position); //Set destination towards table sit
                            clientToSit.SetState(ClientState.SAT); //Set sitting state
                            clientToSit.SetTable(table); //Assign table
                            table.Occupy(); //Occupies the table
                            currentSendTimer = sendRate; //Resets timer
                            break;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Calculate score on client fullfilled
    /// </summary>
    /// <param name="clientServed"></param>
    public void AddScore(Client clientServed)
    {
        score += (5 * (clientServed.GetPatience() / 100));
    }

    /// <summary>
    /// GameOver Condition
    /// </summary>
    public void LoseLife()
    {
        currentLifes--;
        Debug.Log(currentLifes);
        if (currentLifes <= 0)
        {
            GameManager.instance.ChangeGameState(GameState.GameOver);
        }
    }
}
