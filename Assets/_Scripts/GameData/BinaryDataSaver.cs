using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BinaryDataSaver
{
    public int maxScore = 0;
    public int coins = 0;
    public int[] skins;

    public BinaryDataSaver()
    {
        maxScore = 0;
        coins = 0;
        skins = null;
    }
    public BinaryDataSaver(DataManager data)
    {
        maxScore = data.scoreData;
        coins = data.coinData;
    }
    
    
}
