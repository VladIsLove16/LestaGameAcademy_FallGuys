using System;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [SerializeField]
    GameTimer gameTimer;
    public event Action FinishLineReached;
    public void OnTriggerEnter(Collider other)
    {
        CharacterController character = other.gameObject.GetComponent<CharacterController>();
        if (character!=null)
        {
            gameTimer.StopTimer();
            FinishLineReached.Invoke();
        }
    }
}