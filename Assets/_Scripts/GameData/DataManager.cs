using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public int scoreData;
    public int coinData;
    public bool[] skinsData;
    private void OnEnable()
    {
        EventManager.OnPlayerDeath += SaveData;
        EventManager.OnReload += AwakeInizializer;
    }
    private void OnDisable()
    {
        EventManager.OnPlayerDeath -= SaveData;
        EventManager.OnReload -= AwakeInizializer;
    }
    private void Awake()
    {
        AwakeInizializer();
    }
    private void AwakeInizializer()
    {
        LoadData();
    }
    private void SaveData()
    {
        scoreData = UIManager._maxScore;
        coinData = UIManager._coins;
        SaveSystem.SavePlayerData(this);
    }
    private void LoadData()
    {
        
        BinaryDataSaver data = SaveSystem.LoadPlayerData();
        if (data != null)
        {
            scoreData = data.maxScore;
            coinData = data.coins;
        }
        else
        {
            SaveData();
        }
        GiveValue();
    }
    private void GiveValue()
    {
        UIManager._maxScore = scoreData;
        UIManager._coins = coinData;
    }

    

}
