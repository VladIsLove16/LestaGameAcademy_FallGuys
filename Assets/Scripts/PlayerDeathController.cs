using UnityEngine;

public class PlayerDeathController : MonoBehaviour, IDamageable
{
    
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private Transform spawnPoint;
    private int health;
    public void GetDamage(int damage)
    {
        health-=damage;
        if (health < 0)
            Die();
    }

    private void Die()
    {
        gameObject.transform.position = spawnPoint.position;
        health = maxHealth;
    }
}