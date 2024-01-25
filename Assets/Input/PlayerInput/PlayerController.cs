using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Properties
    [SerializeField] private float m_maxSpeed;
    [SerializeField] private float m_moveForce;
    [SerializeField] private float tiltForce = 40f;
    [SerializeField] private Transform mainPlate;

    //Components
    private Rigidbody2D rb;
    private PlayerControls playerControls;

    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        playerControls = new PlayerControls();
        playerControls.Gameplay.Enable(); //Enable gameplay control scheme
        playerControls.Gameplay.DeliverFood.performed += Deliver;//Subscribe to deliver action
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
    }

    private void TiltPlate()
    {
        Vector2 inputVector = playerControls.Gameplay.Movement.ReadValue<Vector2>();
        Vector3 tiltDirection = new Vector3(0, 0, -inputVector.x);
        if (CheckMovement())
        {
            
            Vector3 TiltVector = tiltDirection * tiltForce * Time.deltaTime;
            mainPlate.Rotate(TiltVector);
        }
        else if(!CheckMovement())
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
        Debug.Log("Delivering");
        if (Physics2D.OverlapCircle(transform.position, 1.0f))
        {

        }
    }
}
