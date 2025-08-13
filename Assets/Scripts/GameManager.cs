using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CoinManager coinManager;
    [SerializeField] private UiManager uiManager;

    [SerializeField] private TMP_InputField pseudoInput;
    private string savedPseudo;

    private bool savedLastScore;
    [SerializeField] private TextMeshProUGUI errorText;

    public enum GameState
    {
        Menu,
        Playing,
        Result
    }
    

    private void Start()
    {
        pseudoInput.onEndEdit.AddListener(OnInputEndEdit);
        SetGameState(GameState.Menu);
    }

    public void StartGame()
    {
        if (savedPseudo != null && savedPseudo != "" )
        {
            errorText.text = "";
            SetGameState(GameState.Playing);
        }
        else
        {
            errorText.text = "Please enter a Pseudo";
        }
    }

    private void OnInputEndEdit(string pseudo)
    {
        savedPseudo = pseudo;
    }

    private void SetGameState(GameState state)
    {
        switch (state)
        {
            case GameState.Menu:
                uiManager.ShowMenu();
                break;

            case GameState.Playing:
                uiManager.ShowGame();
                coinManager.OnSend += OnGetCoinManager;
                break;
            case GameState.Result:
                break;
        }
    }

    private void OnGetCoinManager(bool win)
    {
        SetGameState(GameState.Result);
        uiManager.ShowResult(win);
        savedLastScore = win;
    }

    public void Reset()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void Retry()
    {
        SetGameState(GameState.Playing);
        uiManager.uiRetry();
    }
}
