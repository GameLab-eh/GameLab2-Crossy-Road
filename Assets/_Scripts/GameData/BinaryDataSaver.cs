using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BinaryDataSaver
{
    public int maxScore = 0;
    public int coins = 0;
    public bool[] skins;
    public int lastSkinUsed;

    public BinaryDataSaver()
    {
        maxScore = 0;
        coins = 0;
        lastSkinUsed = 0;
    }
    public BinaryDataSaver(DataManager data)
    {
        maxScore = data.scoreData;
        coins = data.coinData;
        skins = data.skinsData;
        lastSkinUsed = data.lastSkinUsedData;
    }
    
    
}
