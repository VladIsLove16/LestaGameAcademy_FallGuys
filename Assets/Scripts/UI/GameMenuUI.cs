using System;
using TMPro;
using UnityEngine;

public class GameMenuUI : MonoBehaviour
{
    [SerializeField]
    GameObject FinishMenu;
    [SerializeField]
    TextMeshProUGUI ResultText;
    [SerializeField]
    PlayerHealthController PlayerHealthController;
    [SerializeField]
    GameController GameController;
    [SerializeField]
    FinishLine FinishLine;
    private void Awake()
    {
        PlayerHealthController.PlayerDied += OnPlayer_Died;
        PlayerHealthController.PlayerRevive += OnPlayer_Revive;
        FinishLine.FinishLineReached += OnFinishLine_Reached ;
    }

    private void OnFinishLine_Reached()
    {
        SetWinText();
        ToggleMenu(true);
    }

    private void ToggleMenu(bool toggle)
    {
        FinishMenu.gameObject.SetActive(toggle);
    }
    private void OnPlayer_Died()
    {
        SetLosingText();
        ToggleMenu(true);
    }
    private void OnPlayer_Revive()
    {
        ToggleMenu(false);
    }
    private void SetLosingText()
    {
        ResultText.text = "U lost!";
    }
    private void SetWinText()
    {
        ResultText.text = "U won!";
    }
}