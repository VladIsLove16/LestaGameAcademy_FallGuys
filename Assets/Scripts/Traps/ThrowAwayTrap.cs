using UnityEngine;

public class ThrowAwayTrap : WindTrap
{
    protected void OnCollisionEnter(Collision collision)
    {
        IWindForceable windForceable = collision.gameObject.GetComponent<IWindForceable>();
        Debug.Log(collision.gameObject.name);
        if (windForceable != null)
        {
            Debug.Log("Add force");
            collision.rigidbody.AddForce(direction.x*force, 10f, direction.y*force, ForceMode.VelocityChange);
        }
    }
    protected override void OnTriggerStay(Collider other)
    {
        return;
    }
}