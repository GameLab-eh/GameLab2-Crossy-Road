using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EventManager : MonoBehaviour
{
    public static Action<int> OnCoinIncrease;
    public static Action OnPlayerDeath;
    
}
