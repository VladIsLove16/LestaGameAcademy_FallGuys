using System;
using System.Collections;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    private PlayerActions�S playerActions;
    [SerializeField]
    private float MaxMoveSpeed;
    [SerializeField]
    private float Acceleration;
    [SerializeField]
    private float JumpForce;
    [SerializeField] // ����� ��� �������� �����
    private float groundDistance = 0.4f;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField] // ������ �������� �����
    private LayerMask groundMask;  // ����� ��� �����
    [SerializeField]
    private float Turnspeed = 0.1f;
    [SerializeField ]
    private float TurnTime = 0.1f;

    private bool isGrounded;
    Rigidbody rb;

    [SerializeField]
    private Transform cameraTransform; // ������ �� ������

    private void Awake()
    {
        SetupRB();
        SetupPlayerActions();
    }

    void Update()
    {
        Move(); 
        Rotate();
        GroundCheck();
        ResetVerticalVelocityOnLanding();
        
    }

    private void Rotate()
    {
        // �������� ������ �������� �� ������
        Vector2 moveVector = playerActions.Player.Move.ReadValue<Vector2>();

        // ���� �������� ������������ (����� �������� ������� �� ����)
        if (moveVector.sqrMagnitude > 0.01f)
        {
            // �������� forward � right ����������� ������
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            // ������� ������������ ������������ ������ (����� �������� �������� ������ �� ���������)
            cameraForward.y = 0f;
            cameraRight.y = 0f;

            // ����������� ������� ����������� (������ �� ������ 1)
            cameraForward.Normalize();
            cameraRight.Normalize();

            // ������������ ����������� �������� � ����������� �� ������
            Vector3 moveDirection = (cameraRight * moveVector.x + cameraForward * moveVector.y).normalized;

            // ��������� ���� �������� �� ������ ����������� ��������
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;

            // ������ ������������ ��������� � �������������� SmoothDampAngle
            float rotationAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref Turnspeed, TurnTime);

            // ��������� ������� � ���������
            transform.rotation = Quaternion.Euler(0f, rotationAngle, 0f);
        }
    }


    private void Move()
    {
        // �������� ����������� �������� �� ������ (��� X � Y � �������� �� ���������)
        Vector2 moveVector = playerActions.Player.Move.ReadValue<Vector2>();

        // ����������� ����������� �������� ������������ ������
        Vector3 moveDirection = CameraRelativeMovement(moveVector);

        // ��������� ���� ��� ����������� ���������
        rb.AddForce(moveDirection * Acceleration * Time.deltaTime, ForceMode.VelocityChange);
    }

    private Vector3 CameraRelativeMovement(Vector2 moveVector)
    {
        // �������� forward � right ����������� ������
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // ������� ������������ ������������ ������ (����� �������� �������� ������ �� ���������)
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        // ����������� ������� ����������� (������ �� ������ 1)
        cameraForward.Normalize();
        cameraRight.Normalize();

        // ������������ ����������� �������� � ����������� �� ������
        Vector3 moveDirection = (cameraRight * moveVector.x + cameraForward * moveVector.y);

        return moveDirection;
    }

    private void SetupPlayerActions()
    {
        playerActions = new PlayerActions�S();
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
        // ��������� "���������������", ������� �����������, ���������� ��������
        if (isGrounded && rb.velocity.y < 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        }
    }
}