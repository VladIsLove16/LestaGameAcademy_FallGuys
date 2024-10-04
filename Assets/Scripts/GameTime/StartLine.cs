using System;
using UnityEngine;

public class StartLine : MonoBehaviour
{
    [SerializeField]
    GameTimer gameTimer;
    public event Action StartLineReached;
    private void OnTriggerEnter(Collider other)
    {
        CharacterController character = other.gameObject.GetComponent<CharacterController>();
        if (character!=null)
        {
            gameTimer.StartTimer();
            StartLineReached?.Invoke();
        }
    }
}
