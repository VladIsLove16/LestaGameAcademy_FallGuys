using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;  // Цель, за которой следует камера (например, игрок)
    public float distance = 5.0f;  // Расстояние камеры от игрока
    public float mouseSensitivity = 2.0f;  // Чувствительность мыши
    public float verticalMin = -40f;  // Минимальный угол по вертикали
    public float verticalMax = 80f;   // Максимальный угол по вертикали

    private float rotationX = 0.0f;  // Угол вращения по X (вокруг вертикальной оси)
    private float rotationY = 0.0f;  // Угол вращения по Y (вокруг горизонтальной оси)

    void Start()
    {
        // Начальная настройка угла вращения
        Vector3 angles = transform.eulerAngles;
        rotationX = angles.y;
        rotationY = angles.x;

        // Скрываем курсор
        LockCursor();
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    void LateUpdate()
    {
        // Получаем ввод с мыши
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Изменяем углы вращения
        rotationX += mouseX;
        rotationY -= mouseY;

        // Ограничиваем вертикальный угол вращения
        rotationY = Mathf.Clamp(rotationY, verticalMin, verticalMax);

        // Определяем направление камеры
        Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0);
        transform.position = target.position - (rotation * Vector3.forward * distance);
        transform.LookAt(target);
    }
}
