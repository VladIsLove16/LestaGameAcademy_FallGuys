using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class ExplodeTrap : MonoBehaviour
{
    enum State
    {
        activated,
        reloading,
        idle,
        exploding
    }
    private State state;
    [SerializeField]
    private int damage;
    private List<IDamageable> objectsInContact = new List<IDamageable>();
    [SerializeField]
    private float ActivatingTime;
    private float currentActivatingTime;
    [SerializeField]
    private float ExplodingTime;
    private float currentExplodingTime;
    [SerializeField]
    private float ReloadTime;
    private float currentReloadTime;
    private MeshRenderer meshRenderer;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip ActivatedSound;
    [SerializeField]
    private AudioClip ExplodingSound;
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        switch(state)
        {
            case State.reloading:
                ReloadingProcess();
                break;
            case State.idle:
                break;
            case State.activated:
                ActivatingProcess();
                break;
            case State.exploding:
               ExplodingProcess();
               break;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if(damageable != null)
        {
            OnDamageableEnter(damageable);
        }
    }
    private void OnDamageableEnter(IDamageable damageable)
    {
        AddContact(damageable);
        if (CanActivate())
            Activate();
    }
    private bool CanActivate()
    {
        return state == State.idle;
    }

    public void Activate()
    {
        SetState(State.activated);
        audioSource.PlayOneShot(ActivatedSound); 
    }
    private void ActivatingProcess()
    {
        currentActivatingTime -= Time.deltaTime;
        if (currentActivatingTime < 0)
        {
            Explode();
        }
    }
    public void Explode()
    {
        foreach (IDamageable obj in objectsInContact)
        {
            obj.GetDamage(damage, DieReason.explode);
        }
        audioSource.PlayOneShot(ExplodingSound);
        SetState(State.exploding);
    }
    private void ExplodingProcess()
    {
        currentExplodingTime -= Time.deltaTime;
        if (currentExplodingTime < 0)
        {
            Reload();
        }
    }
    private void Reload()
    {
        SetState(State.reloading);
    }
    private void ReloadingProcess()
    {
        currentReloadTime-=Time.deltaTime;
        if(currentReloadTime < 0)
        {
            SetState(State.idle);
        }
    }
    

    private void AddContact(IDamageable damageable)
    {
        if (damageable != null)
        {
            if (!objectsInContact.Contains(damageable))
            {
                objectsInContact.Add(damageable);
            }
        }
    }

    private void SetState(State state)
    {
        this.state = state;
        switch (state)
        {
            case State.reloading:
                meshRenderer.material.color = Color.white;
                currentReloadTime = ReloadTime; 
                break;
            case State.idle:
                meshRenderer.material.color = Color.white;
                break;
            case State.activated:
                meshRenderer.material.color = ColorExtensions.orange;
                currentActivatingTime = ActivatingTime;
                break;
            case State.exploding:
                meshRenderer.material.color = Color.red;
                currentExplodingTime = ExplodingTime;
                break;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            if (objectsInContact.Contains(damageable))
            {
                objectsInContact.Remove(damageable);
            }
        }
    }
    //public override void Perform()
    //{
    //   //_ = Activate();
    //}
}
