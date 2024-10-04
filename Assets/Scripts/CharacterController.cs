using System;
using System.Collections;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
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
    [SerializeField] // Точка для проверки земли
    private float groundDistance = 0.4f;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField] // Радиус проверки земли
    private LayerMask groundMask;  // Маска для земли
    [SerializeField]
    private float Turnspeed = 0.1f;
    [SerializeField ]
    private float TurnTime = 0.1f;

    private bool isGrounded;
    Rigidbody rb;

    [SerializeField]
    private Transform cameraTransform; // Ссылка на камеру

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
        // Получаем вектор движения от игрока
        Vector2 moveVector = playerActions.Player.Move.ReadValue<Vector2>();

        // Если движение присутствует (чтобы избежать деления на ноль)
        if (moveVector.sqrMagnitude > 0.01f)
        {
            // Получаем forward и right направления камеры
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            // Очищаем вертикальную составляющую камеры (чтобы персонаж двигался только по плоскости)
            cameraForward.y = 0f;
            cameraRight.y = 0f;

            // Нормализуем вектора направления (делаем их длиной 1)
            cameraForward.Normalize();
            cameraRight.Normalize();

            // Рассчитываем направление движения в зависимости от камеры
            Vector3 moveDirection = (cameraRight * moveVector.x + cameraForward * moveVector.y).normalized;

            // Вычисляем угол поворота на основе направления движения
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;

            // Плавно поворачиваем персонажа с использованием SmoothDampAngle
            float rotationAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref Turnspeed, TurnTime);

            // Применяем поворот к персонажу
            transform.rotation = Quaternion.Euler(0f, rotationAngle, 0f);
        }
    }


    private void Move()
    {
        // Получаем направление движения от игрока (ось X и Y — движение на плоскости)
        Vector2 moveVector = playerActions.Player.Move.ReadValue<Vector2>();

        // Преобразуем направление движения относительно камеры
        Vector3 moveDirection = CameraRelativeMovement(moveVector);

        // Применяем силу для перемещения персонажа
        rb.AddForce(moveDirection * Acceleration * Time.deltaTime, ForceMode.VelocityChange);
    }

    private Vector3 CameraRelativeMovement(Vector2 moveVector)
    {
        // Получаем forward и right направления камеры
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // Очищаем вертикальную составляющую камеры (чтобы персонаж двигался только по плоскости)
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        // Нормализуем вектора направления (делаем их длиной 1)
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Рассчитываем направление движения в зависимости от камеры
        Vector3 moveDirection = (cameraRight * moveVector.x + cameraForward * moveVector.y);

        return moveDirection;
    }

    private void SetupPlayerActions()
    {
        playerActions = new PlayerActionsСS();
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
        // Избежание "проскальзывания", плавное приземление, физический контроль
        if (isGrounded && rb.velocity.y < 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        }
    }
}