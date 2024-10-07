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
    //[SerializeField]
    //ThirdPersonCamera ThirdPersonCamera;
    private void Awake()
    {
        FinishLine.FinishLineReached += ()=>FinishGame(true);
        playerHealthController.PlayerDied += () => FinishGame(false);
    }
    private void Update()
    {
        //PlayerFallCheck();
    }
    private void PlayerFallCheck()
    {
        //if (playerHealthController.transform.position.y < -10f)
        //{
        //    playerHealthController.Die(DieReason.falling);
        //}
    }
    private void TeleportPlayerToSpawn()
    {
        playerHealthController.gameObject.transform.position = spawnPoint.position;
        Debug.Log("Playe teleport to spawn/ new cord" + playerHealthController.transform.position);

    }
    public void RevivePlayer()
    {
        TeleportPlayerToSpawn();
        playerHealthController.Revive();
    }
    public void FinishGame(bool result )
    {
        //GameFinished?.Invoke(result);
    }
}
public static class GamePause
{
    public  static bool isPaused;
        
}
