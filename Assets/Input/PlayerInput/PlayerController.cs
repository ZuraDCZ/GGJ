using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Properties
    [SerializeField] float m_maxSpeed;
    [SerializeField] float m_moveForce;
    [SerializeField] int maxFood = 3;
    [SerializeField] float tiltForce = 40f;
    [SerializeField] Transform mainPlate;
    [SerializeField] LayerMask clientLayer, foodLayer;
    public List<Food> foodList = new List<Food>();

    //Components
    private Rigidbody2D rb;
    private PlayerControls playerControls;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        playerControls = new PlayerControls();
        playerControls.Gameplay.Enable(); //Enable gameplay control scheme

        //Subscribe to player actions
        playerControls.Gameplay.PickUpFood.performed += PickUp; 
        playerControls.Gameplay.DeliverFood.performed += Deliver;
    }

    private void Update()
    {
        CheckMovement();
        TiltPlate();
    }

    private void FixedUpdate()
    {
        Move();
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
    /// Handles players plate tit on move
    /// </summary>
    private void TiltPlate()
    {
        Vector2 inputVector = playerControls.Gameplay.Movement.ReadValue<Vector2>();
        Vector3 tiltDirection = new Vector3(0, 0, -inputVector.x);
        if (CheckMovement())
        {

            Vector3 TiltVector = tiltDirection * tiltForce * Time.deltaTime;
            mainPlate.Rotate(TiltVector);
        }
        else if (!CheckMovement())
        {
            Vector3 currentRotation = new Vector3(mainPlate.rotation.x, mainPlate.rotation.y, 0);
            Vector3 resetRotation = Vector3.Lerp(currentRotation, Vector3.zero, 0.5f);
            mainPlate.Rotate(resetRotation);
        }
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
                                food.transform.SetParent(null);
                                ServeClient(clientServed, food);
                                UpdateFoodTransform(); //Reorganize the food placement on the plate
                                break;
                            }
                        }
                    }
                }
            } 
        }
    }

    private void ServeClient(Client clientServed, Food food)
    {
        food.transform.SetPositionAndRotation(clientServed.GetTable().GetPlatePlace().position, Quaternion.identity); //Put the plate on the table
        foodList.Remove(food); //Remove from the carried food list
        clientServed.SetState(ClientState.EATING); //Notify the client to start eating
    }

    private void PickUp(InputAction.CallbackContext context)
    {
        if (Physics2D.OverlapCircle(transform.position, 2.0f, foodLayer) && foodList.Count <= maxFood)
        {
            GameObject c = Physics2D.OverlapCircleAll(transform.position, 2.0f, foodLayer)[0].gameObject;
            if (c != null)
            {
                Food pickedFood = c.GetComponent<Food>();
                if (pickedFood != null && !foodList.Contains(pickedFood))
                {
                    pickedFood.gameObject.layer = 0;
                    pickedFood.transform.SetParent(transform, false);
                    foodList.Add(pickedFood);
                    UpdateFoodTransform();
                }
            }
        }
    }

    private void UpdateFoodTransform()
    {
        
        foreach (Food food in foodList)
        {
            int index = foodList.IndexOf(food);
            food.transform.SetLocalPositionAndRotation(new Vector3(1, index, 0), Quaternion.identity);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 2.0f);
    }
}
