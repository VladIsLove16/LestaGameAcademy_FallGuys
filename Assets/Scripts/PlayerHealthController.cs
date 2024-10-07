using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour, IDamageable//можно сделать обычным с# классом для оптимизации, но так как игра маленькая, это неважно, а прокидывать кол-во хп удобнее из инспектора в отдельном компоненте
{
    [SerializeField]
    private int maxHealth;
    private int health;
    public event Action PlayerDied;
    public event Action PlayerRevive;
    private bool isDead;
    private float CanBeDamagedTime;
    private const float InvulnerabilityTimeConst = 0.1f;
    [SerializeField]
    public void GetDamage(int damage, DieReason reason)
    {
        if (CanBeDamagedTime > Time.timeSinceLevelLoad)
            return;
        health-=damage;
        CanBeDamagedTime = Time.timeSinceLevelLoad+ InvulnerabilityTimeConst;
        if (health < 0)
            Die(reason);
    }
    private void Die(DieReason reason)
    {
        if (isDead)
            return;
        isDead = true;
        Debug.Log("PlayerDie from " + reason.ToString());

        PlayerDied.Invoke();
    }
    public void Revive()
    {

        CanBeDamagedTime = Time.timeSinceLevelLoad + InvulnerabilityTimeConst;
        health = maxHealth; 
        isDead = false;
        PlayerRevive.Invoke();
        Debug.Log("PlayerRevive");

    }
}
