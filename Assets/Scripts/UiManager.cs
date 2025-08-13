using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameObject homeScreen;
    [SerializeField] private GameObject choiceScreen;
    [SerializeField] private GameObject resultScreen;
    [SerializeField] private GameObject resultWin;
    [SerializeField] private GameObject resultLose;
    [SerializeField] private GameObject confettiFx;

    public static UiManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    public void ShowMenu()
    {
        HideAll();
        homeScreen.SetActive(true);
    }

    public void ShowGame()
    {
        HideAll();
        choiceScreen.SetActive(true);
    }

    public void ShowResult(bool isWinning)
    {
        HideAll();
        // Gagner
        if (isWinning) {
            resultScreen.SetActive(true);
            resultLose.SetActive(false);
            resultWin.SetActive(true);
            confettiFx.SetActive(true);
        }
        // Perdu
        else
        {
            resultScreen.SetActive(true);
            resultWin.SetActive(false);
            resultLose.SetActive(true);
        }
    }

    public void uiRetry()
    {
        HideAll();
        choiceScreen.SetActive(true);
    }

    public void HideAll()
    {
        homeScreen.SetActive(false);
        choiceScreen.SetActive(false);
        resultScreen.SetActive(false);
        confettiFx.SetActive(false) ;
    }
}
