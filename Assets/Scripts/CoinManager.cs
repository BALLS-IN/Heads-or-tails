using UnityEngine;
using static Coin;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private Coin coin;
    private Coin.CoinResult betCoinResult;
    private bool waitingForResult = false;

    // Un event pour notifier au GameManager
    public delegate void SendResultToGameManager(bool win);
    public event SendResultToGameManager OnSend;

    // parie sur le pile 
    public void BetPile()
    {
        betCoinResult = Coin.CoinResult.Pile;
        FlipCoin();
    }

    // parie sur la face
    public void BetFace()
    {
        betCoinResult = Coin.CoinResult.Face;
        FlipCoin();
    }

    public void FlipCoin()
    {
        UiManager.Instance.HideAll();
        waitingForResult = true;
        coin.OnFlipEnd += OnCoinFlipEnd; // S'abonner � l'�v�nement
        coin.Flip();
    }

    private void OnCoinFlipEnd(Coin.CoinResult result)
    {
        coin.OnFlipEnd -= OnCoinFlipEnd; // D�sabonner pour �viter les doublons
        waitingForResult = false;
        bool win;
        if (result == betCoinResult) { 
            win = true; 
        }
        else
        {
            win = false;
        }
        OnSend?.Invoke(win);
    }
}
