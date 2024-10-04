using System;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour, IDamageable
{
    [SerializeField]
    private int maxHealth;
    private int health;
    public event Action PlayerDied;
    public event Action PlayerRevive;
    public void GetDamage(int damage)
    {
        health-=damage;
        if (health < 0)
            Die();
    }
    public void Die()
    {
        PlayerDied.Invoke();
    }
    public void Revive()
    {
        health = maxHealth;
        PlayerRevive.Invoke();
    }
}
