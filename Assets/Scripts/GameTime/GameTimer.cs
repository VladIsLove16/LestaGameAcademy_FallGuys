using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI TextMeshProUGUI;
    private bool isPlaying;
    private float currentTime;
    public event Action TimerStarted;
    public event Action TmerStoped;
    [SerializeField]
    PlayerHealthController PlayerHealthController;
    private void Awake()
    {
        PlayerHealthController.PlayerDied += StopTimer;
    }
    private void Update()
    {
        if (isPlaying)
        {
            currentTime += Time.deltaTime;
            TextMeshProUGUI.text = currentTime.ToString().Substring(0,4);
        }
    }
    public void StartTimer()
    {
        if (isPlaying)
            return;
        ReStartTimer();
    }
    public void ReStartTimer()
    {
        isPlaying = true;
        ResetTime();
        TimerStarted?.Invoke();
    }

    internal void StopTimer()
    {
        isPlaying = false;
        TmerStoped?.Invoke();
    }
    public void ResetTime()
    {
        currentTime = 0f;
    }
}
