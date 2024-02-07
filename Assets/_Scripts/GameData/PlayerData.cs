using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1), Serializable]
public class PlayerData : ScriptableObject
{
    public int score;
}
