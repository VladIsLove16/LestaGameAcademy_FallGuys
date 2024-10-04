using System.Linq;
using UnityEngine;

public class WindTrap : MonoBehaviour
{
    private static readonly Vector2[] directions = {
        Vector2.up,            // 0° (север)
        new Vector2(1, 1).normalized, // 45° (северо-восток)
        Vector2.right,        // 90° (восток)
        new Vector2(1, -1).normalized, // 135° (юго-восток)
        Vector2.down,         // 180° (юг)
        new Vector2(-1, -1).normalized, // 225° (юго-запад)
        Vector2.left,         // 270° (запад)
        new Vector2(-1, 1).normalized  // 315° (северо-запад)
    };
    [SerializeField]
    protected float force;
    [SerializeField]
    float directionChangeTime;
    float currentDirectionChangeTime;
    protected Vector2 direction;
    [SerializeField]
    Transform Visual;
    private void Update()
    {
        currentDirectionChangeTime -= Time.deltaTime;
        if (currentDirectionChangeTime < 0)
            ChangeDirection(GetRandomDirection());

    }
    protected virtual void OnTriggerStay(Collider other)
    {
        IWindForceable windForceable = other.gameObject.GetComponent<IWindForceable>();
        Debug.Log(other.gameObject.name);
        if (windForceable !=null)
        {
            Debug.Log("Add force");
            other.attachedRigidbody.AddForce(direction.x*force, 0, direction.y*force,ForceMode.VelocityChange);
        }
    }
    private Vector2 GetRandomDirection()
    {
        int directionNum = UnityEngine.Random.Range(0,directions.Count());
        return directions[directionNum];
    }
    private void ChangeDirection(Vector2 direction)
    {
        this.direction = direction;
        RotateBasedOnDirection(direction);
        currentDirectionChangeTime = directionChangeTime;
    }
    private void ChangeDirection(int x, int y)
    {
        ChangeDirection(new Vector2(x, y));
    }
    public void RotateBasedOnDirection(Vector2 direction)
    {
        float angle = -Vector2.SignedAngle(Vector2.up, direction);   
        //float angle180 = Vector2.SignedAngle(Vector2.right, Vector2.down);
        Debug.Log(direction+ " " + angle);
        if(Visual)
            Visual.rotation = Quaternion.Euler(0f, angle, 0f);
    }
}
