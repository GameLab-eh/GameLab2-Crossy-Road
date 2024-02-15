using System;
using UnityEngine;


public class CoinScoreManager : MonoBehaviour
{
    private int _coinCounter;
    private void OnEnable()
    {
        EventManager.OnCoinIncrease += AddCoins;
    }
    private void OnDisable()
    {
        EventManager.OnCoinIncrease -= AddCoins;
    }
    private void AddCoins(int coin)
    {
        _coinCounter += coin;
        
    }


}

