using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    private PlayerActionsСS playerActions;
    [SerializeField]
    private float MaxMoveSpeed;
    [SerializeField]
    private float Acceleration;
    [SerializeField]
    private float JumpForce;
    [SerializeField]// Точка для проверки земли
    private float groundDistance = 0.4f;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]// Радиус проверки земли
    private LayerMask groundMask;  // Маска для земли

    private bool isGrounded; 
    Rigidbody rb;
    // Update is called once per frame
    private void Awake()
    {
        SetupRB();
        SetupPlayerActions();
    }
    void Update()
    {
        Move();
        GroundCheck(); 
        ResetVerticalVelocityOnLanding();
    }

    private void Move()
    {
        Vector2 movevector = playerActions.Player.Move.ReadValue<Vector2>();
        rb.AddForce(new Vector3(movevector.x,0, movevector.y) * Acceleration*Time.deltaTime,ForceMode.VelocityChange);
    }

    private void SetupPlayerActions()
    {
        playerActions = new();
        playerActions.Enable();
        playerActions.Player.Jump.performed += (callback) => Jump();
    }

    private void SetupRB()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = MaxMoveSpeed;
    }
    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }
    private void Jump()
    {
        if (isGrounded)
            rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
    }
    private void ResetVerticalVelocityOnLanding()
    { 
        //Избежание "проскальзывания",Плавное приземление,Физический контроль
        if (isGrounded && rb.velocity.y < 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        }
    }

}
