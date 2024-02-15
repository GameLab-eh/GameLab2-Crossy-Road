using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BinaryDataSaver
{
    public int maxScore;
    public int coins;
    public int[] skins;

    public BinaryDataSaver(DataManager data)
    {
        maxScore = data.scoreData;
        coins = data.coinData;
    }
    
    
}
