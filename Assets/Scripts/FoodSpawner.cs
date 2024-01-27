using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public static FoodSpawner instance;
    [SerializeField] Food[] foodPrefabs;
    [SerializeField] List<Transform> foodSpawns;
    public List<Transform> spawnsOccupied = new List<Transform>(); //Spawns that are occupied
    [SerializeField] float spawnRate; //Time it takes for food to be spawned
    private float currentTimer;

    private void Awake()
    {
        instance = this;
        currentTimer = spawnRate;
    }

    private void Update()
    {
        if (LvlManager.instance.clientsSat.Count > 0)
        {
            currentTimer -= Time.deltaTime;
            if (currentTimer < 0 && spawnsOccupied.Count <= foodSpawns.Count)
            {
                foreach (Client client in LvlManager.instance.clientsSat)
                {
                    if (!client.HasOrdered())
                    {
                        foreach (Transform t in foodSpawns)
                        {
                            if (!spawnsOccupied.Contains(t))
                            {
                                Instantiate(foodPrefabs[client.GetOrder()], t.position, Quaternion.identity);
                                FillSpawn(t);
                                client.onOrder.Invoke();
                                break;
                            }
                            continue;
                        }
                        break;
                    }
                }
            }
        }
    }

    public int GetFoodLenght()
    {
        return foodPrefabs.Length;
    }

    public void FillSpawn(Transform foodSpawn)
    {
        spawnsOccupied.Add(foodSpawn);
    }

    public void EmptySpawn(Transform foodSpawn)
    {
        spawnsOccupied.Remove(foodSpawn);
    }

    public Food[] GetFoodArray()
    {
        return foodPrefabs;
    }
}
