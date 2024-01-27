using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    //Singleton
    public static FoodSpawner instance;

    //Food pooling requirements
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] int foodPoolSize;
    [SerializeField] Sprite[] foodSprites;
    private List<Food> foodList = new List<Food>();

    //Food spawning requirements
    [SerializeField] float spawnRate;
    [SerializeField] List<Transform> foodSpawns;
    private float currentTimer;
    public List<Transform> spawnsOccupied = new List<Transform>();
    private void Awake()
    {
        instance = this;
        currentTimer = spawnRate;

        //Initialize food pool
        for (int i = 0; i < foodPoolSize; i++)
        {
            GameObject go = Instantiate(foodPrefab, Vector3.zero, Quaternion.identity);
            go.SetActive(false);
            Food newFood = go.GetComponent<Food>();
            foodList.Add(newFood);
        }
    }

    private void Update()
    {
        if (GameManager.instance.GetGameState() == GameState.Playing)
        {
            HandleFoodSpawns();
        }
    }

    /// <summary>
    /// Manages all food pool update and rearranging 
    /// </summary>
    private void HandleFoodSpawns()
    {
        if (LvlManager.instance.clientsSat.Count > 0) //Check if there are clients sat down
        {
            currentTimer -= Time.deltaTime;
            if (currentTimer < 0 && spawnsOccupied.Count <= foodSpawns.Count) //Check if there is enough space to spawn food
            {
                foreach (Client client in LvlManager.instance.clientsSat)
                {
                    if (!client.HasOrdered()) //Look for the first client that hasnt ordered and take it
                    {
                        foreach (Transform t in foodSpawns) //Look for the first unoccupied spawn
                        {
                            if (!spawnsOccupied.Contains(t))
                            {
                                foreach (Food food in foodList)
                                {
                                    if (!food.gameObject.activeSelf) //Look for the first deactivated food 
                                    {
                                        food.UpdateFoodData(foodSprites[client.GetOrder()], client.GetOrder(), t.position); //Updates its data according to the client order
                                        food.SetSpawnTracker(t); //Sets its spawn reference to be cleared on pickup
                                        food.gameObject.SetActive(true);
                                        FillSpawn(t); //Marks its spawn as occupied
                                        client.onOrder.Invoke(); //Notifies client that the order is done
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                        break;
                    }
                }
            }
        }
    }

    private void FillSpawn(Transform foodSpawn)
    {
        spawnsOccupied.Add(foodSpawn);
    }

    public void EmptySpawn(Transform foodSpawn)
    {
        if (spawnsOccupied.Contains(foodSpawn))
        {
            spawnsOccupied.Remove(foodSpawn);
        }
    }

    public int GetSpritesLength()
    {
        return foodSprites.Length;
    }
}
