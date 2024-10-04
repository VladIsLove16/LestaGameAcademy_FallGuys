using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;  // ����, �� ������� ������� ������ (��������, �����)
    public float distance = 5.0f;  // ���������� ������ �� ������
    public float mouseSensitivity = 2.0f;  // ���������������� ����
    public float verticalMin = -40f;  // ����������� ���� �� ���������
    public float verticalMax = 80f;   // ������������ ���� �� ���������

    private float rotationX = 0.0f;  // ���� �������� �� X (������ ������������ ���)
    private float rotationY = 0.0f;  // ���� �������� �� Y (������ �������������� ���)

    void Start()
    {
        // ��������� ��������� ���� ��������
        Vector3 angles = transform.eulerAngles;
        rotationX = angles.y;
        rotationY = angles.x;

        // �������� ������
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
        // �������� ���� � ����
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // �������� ���� ��������
        rotationX += mouseX;
        rotationY -= mouseY;

        // ������������ ������������ ���� ��������
        rotationY = Mathf.Clamp(rotationY, verticalMin, verticalMax);

        // ���������� ����������� ������
        Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0);
        transform.position = target.position - (rotation * Vector3.forward * distance);
        transform.LookAt(target);
    }
}
