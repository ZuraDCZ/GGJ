using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement variables")]
    [SerializeField] float m_maxSpeed;
    [SerializeField] float m_moveForce;

    [Header("Food delivery Variables")]
    [SerializeField] int maxFood = 3;
    [SerializeField] LayerMask clientLayer, foodLayer;

    //[Header("Sound requirements")]
    //[SerializeField] AudioClip footestepsAudioClip;
    //[SerializeField] float footstepTimer = 0;
    public List<Food> foodList = new List<Food>();

    //Components
    private Rigidbody2D rb;
    private AudioSource footestepsAudioSource;
    private PlayerControls playerControls;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        footestepsAudioSource = GetComponent<AudioSource>();
        playerControls = new PlayerControls();
        playerControls.Gameplay.Enable(); //Enable gameplay control scheme

        //Subscribe to player actions
        playerControls.Gameplay.PickUpFood.performed += PickUp; 
        playerControls.Gameplay.DeliverFood.performed += Deliver;
        playerControls.Gameplay.PauseGame.performed += PauseGame;
    }

    private void Update()
    {
        if (GameManager.instance.GetGameState() == GameState.Playing)
        {
            CheckMovement(); 
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.GetGameState() == GameState.Playing)
        {
            Move();
        }
    }

    /// <summary>
    /// Move player by reading its input
    /// </summary>
    private void Move()
    {
        Vector2 inputVector = playerControls.Gameplay.Movement.ReadValue<Vector2>();
        Vector2 force = inputVector * m_moveForce;
        rb.AddForce(force, ForceMode2D.Impulse);

        //Clamp players velocity
        if (rb.velocity.sqrMagnitude > ((m_maxSpeed * m_maxSpeed)))
        {
            rb.velocity = rb.velocity.normalized * m_maxSpeed;
        }

        ///TODO: AnimationLogic
    }

    /// <summary>
    /// Check if the player is moving
    /// </summary>
    private bool CheckMovement()
    {
        Vector2 inputVector = playerControls.Gameplay.Movement.ReadValue<Vector2>();
        if (inputVector.x != 0 || inputVector.y != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Handles delivery action
    /// </summary>
    /// <param name="context"></param>
    private void Deliver(InputAction.CallbackContext context)
    {
        if (Physics2D.OverlapCircle(transform.position, 2.0f, clientLayer)) //Find a clients
        {
            GameObject c = Physics2D.OverlapCircleAll(transform.position, 2.0f, clientLayer)[0].gameObject; //Get the first one 
            if (c != null)
            {
                Client clientServed = c.GetComponent<Client>();
                if (clientServed != null)
                {
                    if (clientServed.GetState() == ClientState.SAT) ///Check if the client is sat on a table
                    {
                        foreach (Food food in foodList) //Look through each of the carried food
                        {
                            if (food.GetID() == clientServed.GetOrder()) //Find the one the client ordered
                            {
                                ServeClient(clientServed, food); //Serve client the ordered plate
                                UpdateFoodTransform(); //Reorganize the food placement on the plate
                                break;
                            }
                        }
                    }
                }
            } 
        }
    }

    /// <summary>
    /// Places food transform on table and notifies client to start eating 
    /// </summary>
    /// <param name="clientServed"></param>
    /// <param name="food"></param>
    private void ServeClient(Client clientServed, Food food)
    {
        food.transform.SetParent(null); //Clear the player as parent
        food.transform.SetPositionAndRotation(clientServed.GetTable().GetPlatePlace().position, Quaternion.identity); //Put the plate on the table
        foodList.Remove(food); //Remove from the carried food list
        clientServed.SetFood(food); //Sets food reference to be deactivated on consumed
        clientServed.SetServed(); //Set as fullfilled
        clientServed.SetState(ClientState.EATING); //Notify the client to start eating
    }

    /// <summary>
    /// Handles player pickup event on input
    /// </summary>
    /// <param name="context"></param>
    private void PickUp(InputAction.CallbackContext context)
    {
        if (Physics2D.OverlapCircle(transform.position, 2.0f, foodLayer) && foodList.Count < maxFood)
        {
            Debug.Log("Found food");
            GameObject c = Physics2D.OverlapCircleAll(transform.position, 2.0f, foodLayer)[0].gameObject;
            if (c != null)
            {
                Food pickedFood = c.GetComponent<Food>();
                if (pickedFood != null && !foodList.Contains(pickedFood))
                {
                    Debug.Log("Picking up food");
                    pickedFood.gameObject.layer = 0;
                    pickedFood.transform.SetParent(transform, false);
                    FoodSpawner.instance.EmptySpawn(pickedFood.GetSpawn());
                    foodList.Add(pickedFood);
                    UpdateFoodTransform();
                }
            }
        }
    }

    /// <summary>
    /// Rearranges food transforms on players plate
    /// </summary>
    private void UpdateFoodTransform()
    {
        foreach (Food food in foodList)
        {
            int index = foodList.IndexOf(food);
            food.transform.SetLocalPositionAndRotation(new Vector3(1, index * 0.25f, 0), Quaternion.identity);
        }
    }

    private void PauseGame(InputAction.CallbackContext context)
    {
        GameManager.instance.ChangeGameState(GameState.Pause);
    }
}
