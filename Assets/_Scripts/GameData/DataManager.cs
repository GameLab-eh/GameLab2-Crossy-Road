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
    public int lastSkinUsedData;
    private void Awake()
    {
        AwakeInzializer();
    }
    private void AwakeInzializer()
    {
        LoadData();
    }
    private void OnEnable()
    {
        EventManager.OnPlayerDeath += SaveData;
        EventManager.OnReload += LoadData;
        EventManager.OnSkinChoice += SaveSkin;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerDeath -= SaveData;
        EventManager.OnReload -= LoadData; 
        EventManager.OnSkinChoice -= SaveSkin;
    }

    private void SaveData()
    {
        scoreData = UIManager._maxScore;
        coinData = UIManager._coins;
        lastSkinUsedData = GameManager.Instance.lastSkinUsed;
        skinsData = GameManager.Instance.skinsUnlocked;
        SaveSystem.SavePlayerData(this);
    }
    private void LoadData()
    {
        
        BinaryDataSaver data = SaveSystem.LoadPlayerData();
        if (data != null)
        {
            scoreData = data.maxScore;
            coinData = data.coins;
            skinsData = data.skins;
            lastSkinUsedData = data.lastSkinUsed;
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
        GameManager.Instance.skinsUnlocked = skinsData;
        GameManager.Instance.lastSkinUsed = lastSkinUsedData;

    }
    public void SaveSkin(int index)
    {
        StartCoroutine(SaveSkinRoutine());
    }
    private IEnumerator SaveSkinRoutine()
    {
        yield return new WaitForNextFrameUnit();
        SaveData();
    }

    

}
