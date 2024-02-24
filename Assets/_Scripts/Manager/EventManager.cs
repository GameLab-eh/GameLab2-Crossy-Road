using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EventManager : MonoBehaviour
{
    public static Action<int> OnCoinIncrease;
    public static Action OnPlayerDeath;
    public static Action OnReload;
    public static Action OnPlayerMoveUp;
    public static Action OnGameStart;
    public static Action<int> OnSkinChoice;
    public static Action OnskinObtained;
    public static Action OnBirdAction;
    public static Action OnBirdArrived;
    public static Action OnPlayerOutOfCam;
}
