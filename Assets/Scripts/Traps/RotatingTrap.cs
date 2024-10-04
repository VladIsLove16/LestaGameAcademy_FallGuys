using UnityEngine;

public class RotatingTrap : MonoBehaviour
{
    [SerializeField]
    Vector3 direction;
    [SerializeField]
    Vector3 point;
    [SerializeField]
    float angle;
    [SerializeField]
    bool method;
    private void Update()
    {
        if (method)
            gameObject.transform.RotateAround(point, direction, angle);
        else
            gameObject.transform.Rotate(direction, angle);
    }
}
