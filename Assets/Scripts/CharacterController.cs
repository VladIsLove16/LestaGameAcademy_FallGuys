using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour, IWindForceable
{
    private PlayerActions�S playerActions;
    [SerializeField]
    private float MaxMoveSpeed;
    [SerializeField]
    private float Acceleration;
    [SerializeField]
    private float FlyingAcceleration;
    [SerializeField]
    private float JumpForce;
    [SerializeField] 
    private float groundCheckDist;
    [SerializeField]
    //private Transform groundCheck;
    //[SerializeField] // ������ �������� �����
    private LayerMask groundMask;  // ����� ��� �����
    [SerializeField]
    private float Turnspeed = 0.1f;
    [SerializeField ]
    private float TurnTime = 0.1f;

    private bool isGrounded;
    private bool isJumping= false;
    Rigidbody rb;

    [SerializeField]
    private Transform cameraTransform; // ������ �� ������
    private PlayerHealthController PlayerHealthController;
    [SerializeField]
    ThirdPersonCamera ThirdPersonCamera;
    Vector2 moveVector;

    private void Awake()
    {
        SetupRB();
        SetupPlayerActions();
        PlayerHealthController = GetComponent<PlayerHealthController>();
        PlayerHealthController.PlayerDied += OnPlayer_Died;
        PlayerHealthController.PlayerRevive +=()=> StartCoroutine( OnPlayer_Revive());
        
    }

    void Update()
    {
        moveVector = playerActions.Player.Move.ReadValue<Vector2>();
    }
    private void FixedUpdate()
    {
        Move();
        Jump();
        Rotate();
        GroundCheck();
        ResetVerticalVelocityOnLanding();
    }
    private void Move()
    {
        Vector3 moveDirection = CameraRelativeMovement(moveVector);
        float force = isGrounded ? Acceleration : FlyingAcceleration;
        rb.AddForce(moveDirection * force * Time.deltaTime, ForceMode.VelocityChange);
    }

    private void OnPlayer_Died()
    {
        rb.isKinematic = true;
        HideMesh();
        ThirdPersonCamera.UnlockCursor();
        ThirdPersonCamera.LockCamera();
    }

    private void HideMesh()
    {
        foreach (MeshRenderer child in transform.GetComponentsInChildren<MeshRenderer>())
        {
            child.enabled = false;
        }
    }

    private IEnumerator  OnPlayer_Revive()
    {
        yield return new WaitForSeconds(0.1f);
        rb.isKinematic = false;
        ShowMesh();
        ThirdPersonCamera.LockCursor();
        ThirdPersonCamera.UnLockCamera();
    }

    private void ShowMesh()
    {
        foreach (MeshRenderer child in transform.GetComponentsInChildren<MeshRenderer>())
        {
            child.enabled = true;
        }
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
        playerActions.Player.Jump.performed += OnJumpPerformed;
        playerActions.Player.Jump.canceled += OnJumpCanceled;
    }

    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        isJumping = false;
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        isJumping = true;
    }

    private void SetupRB()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxLinearVelocity = MaxMoveSpeed;
    }

    private void GroundCheck()
    {
        //isGrounded = Physics.CheckSphere(groundCheck.position, transform.localScale.x, groundMask);
        isGrounded = Physics.Raycast(transform.position, -Vector3.up, groundCheckDist+transform.localScale.y, groundMask);
    }

    private void Jump()
    {
        Debug.Log("jump");
        if (isJumping)
        {
            if (isGrounded)
            {
                //isJumping = false;
                rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);

            }
        }
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
