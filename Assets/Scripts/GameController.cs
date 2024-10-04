using System;
using UnityEngine;

public class GameController  : MonoBehaviour
{
    [SerializeField]
    PlayerHealthController playerHealthController;
    [SerializeField]
    private Transform spawnPoint;
    public event Action<bool> GameFinished;
    [SerializeField]
    StartLine StartLine;
    [SerializeField]
    FinishLine FinishLine;
    [SerializeField]
    ThirdPersonCamera ThirdPersonCamera;
    private void Awake()
    {
        FinishLine.FinishLineReached += ()=>FinishGame(true);
        playerHealthController.PlayerDied += () => FinishGame(false);
    }
    private void Update()
    {
        PlayerFallCheck();
    }
    private void PlayerFallCheck()
    {
        if (playerHealthController.transform.position.y < -10f)
        {
            playerHealthController.Die();
        }
    }
    private void TeleportPlayerToSpawn()
    {
        playerHealthController.transform.position = spawnPoint.position;
    }
    public void RevivePlayer()
    {
        TeleportPlayerToSpawn();
        playerHealthController.Revive();
        ThirdPersonCamera.LockCursor();
    }
    public void FinishGame(bool result )
    {
        Debug.Log("ThirdPersonCamera.UnlockCursor();");
        ThirdPersonCamera.UnlockCursor();
        GameFinished?.Invoke(result);
    }
}
public static class GamePause
{
    public  static bool isPaused;
        
}
