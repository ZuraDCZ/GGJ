using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlManager : MonoBehaviour
{
    [SerializeField] private GameObject[] clientPrefab; //Clients prefabs array. Drag to inspector to add
    [SerializeField] private Transform spawnPosition; //Transform whre the clients spawn. Not parented
    [SerializeField] private float spawnRate; //Time it takes for a client to spawn
    [SerializeField] private float sendRate; //Time it takes for a waitng client to be given a table
    private float currentSpawnTimer; //Keeps track of the time to spawn a client
    private float currentSendTimer; //Keeps track of the time to send a client
    [SerializeField]List<Table> tables = new List<Table>(); //Tables on scene
    [SerializeField]List<Client> clientsWaiting = new List<Client>(); //List to keep track of the clients
    private void Awake()
    {
        InitializeLvl();
    }

    /// <summary>
    /// Sets properties and list for the game to run
    /// </summary>
    private void InitializeLvl()
    {
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
        GenerateClients();
        SendClients();
    }

    /// <summary>
    /// Generates a random client from the clients array every certain time
    /// </summary>
    private void GenerateClients()
    {
        currentSpawnTimer -= Time.deltaTime; 
        if (currentSpawnTimer <= 0 && clientsWaiting.Count <= tables.Count)
        {
            Debug.Log("Spawning Client");
            int randomIndex = Random.Range(0, clientPrefab.Length);
            Client newClient = Instantiate(clientPrefab[randomIndex], spawnPosition.position, Quaternion.identity).GetComponent<Client>();
            newClient.SetTarget(spawnPosition.position + new Vector3(0, 5,0));
            clientsWaiting.Add(newClient);
            currentSpawnTimer = spawnRate;
        }
    }

    /// <summary>
    /// Sends a client to Table
    /// </summary>
    private void SendClients()
    {
        currentSendTimer -= Time.deltaTime;
        if (clientsWaiting.Count > 0)
        {
            if (currentSendTimer <= 0)
            {
                foreach (Table table in tables)
                {
                    if (table != null)
                    {
                        if (!table.isOccupied())
                        {
                            Debug.Log("Sending Client");
                            Client clientToSit = clientsWaiting[0];
                            int randomSit = Random.Range(0, table.Sits().Count);
                            clientToSit.SetTarget(table.Sits()[randomSit].position);
                            clientsWaiting.Remove(clientToSit);
                            table.Occupy();
                            currentSendTimer = sendRate;
                            break;
                        }
                    }
                }
            }
        }
    }
}
