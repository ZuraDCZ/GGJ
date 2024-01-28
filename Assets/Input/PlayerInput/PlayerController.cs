using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement variables")]
    [SerializeField] float m_maxSpeed;
    [SerializeField] float m_moveForce;

    [Header("Food delivery Variables")]
    [SerializeField] Transform plateTransform;
    [SerializeField] int maxFood = 3;
    [SerializeField] LayerMask clientLayer, foodLayer;
    [SerializeField] float distance;
    public List<Food> foodList = new List<Food>();

    [Header("Sound requirements")]
    [SerializeField] AudioSource getPlate;
    [SerializeField] AudioSource servePlate;

    //[Header("Sound requirements")]
    //[SerializeField] AudioClip footestepsAudioClip;
    //[SerializeField] float footstepTimer = 0;

    //Components
    private Rigidbody2D rb;
    private PlayerControls playerControls;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerControls = new PlayerControls();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
            CheckLayer();
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
        animator.SetFloat("MovementY", rb.velocity.y);
        animator.SetFloat("MovementX", rb.velocity.x);
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
        if (Physics2D.OverlapCircle(transform.position, distance, clientLayer)) //Find a clients
        {
            GameObject c = Physics2D.OverlapCircleAll(transform.position, distance, clientLayer)[0].gameObject; //Get the first one 
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
        food.gameObject.GetComponent<SpriteRenderer>().sortingOrder = clientServed.GetTable().gameObject.GetComponent<SpriteRenderer>().sortingOrder + 1;
        foodList.Remove(food); //Remove from the carried food list
        clientServed.SetFood(food); //Sets food reference to be deactivated on consumed
        clientServed.SetServed(); //Set as fullfilled
        servePlate.Play();
        clientServed.SetState(ClientState.EATING); //Notify the client to start eating
    }

    /// <summary>
    /// Handles player pickup event on input
    /// </summary>
    /// <param name="context"></param>
    private void PickUp(InputAction.CallbackContext context)
    {
        if (Physics2D.OverlapCircle(transform.position, distance, foodLayer) && foodList.Count < maxFood)
        {
            GameObject c = Physics2D.OverlapCircleAll(transform.position, distance, foodLayer)[0].gameObject;
            if (c != null)
            {
                Food pickedFood = c.GetComponent<Food>();
                if (pickedFood != null && !foodList.Contains(pickedFood))
                {
                    getPlate.Play();
                    pickedFood.gameObject.layer = 0;
                    pickedFood.transform.SetParent(plateTransform, false);
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
            food.transform.SetLocalPositionAndRotation(new Vector3(0, index * 0.2f, 0), Quaternion.identity);
        }
    }

    private void PauseGame(InputAction.CallbackContext context)
    {
        GameManager.instance.ChangeGameState(GameState.Pause);
    }

    /// <summary>
    /// Changes sprite sort in layer depending on height
    /// </summary>
    private void CheckLayer()
    {
        if (transform.position.y > 0.7f && transform.position.y < 3f)
        {
            spriteRenderer.sortingOrder = 2;
        }
        else if (transform.position.y > 0f && transform.position.y < 0.7f)
        {
            spriteRenderer.sortingOrder = 4;
        }
        
        else if (transform.position.y < 0f && transform.position.y > - 1.5f)
        {
            spriteRenderer.sortingOrder = 5;
        }
        else if (transform.position.y < -1.5f && transform.position.y > -4f)
        {
            spriteRenderer.sortingOrder = 7;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distance);
    }
}