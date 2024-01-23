using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Components
    private Rigidbody2D rb;
    private PlayerControls playerControls;

    //Properties
    [SerializeField] private float m_maxSpeed;
    [SerializeField] private float m_moveForce;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        playerControls = new PlayerControls();
        playerControls.Gameplay.Enable(); //Enable gameplay control scheme
    }

    private void Update()
    {
        CheckMovement();
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
        rb.AddForce(new Vector2(inputVector.x, inputVector.y) * m_moveForce, ForceMode2D.Impulse);
        
        //Clamp players velocity
        if (rb.velocity.sqrMagnitude > ((m_maxSpeed * m_maxSpeed)))
        {
            rb.velocity = rb.velocity.normalized * m_maxSpeed;
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
}
