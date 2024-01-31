using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObstacleManager : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField, Min(0)] float carSpeed;
    [SerializeField, Min(0)] float trainSpeed;
    [SerializeField, Min(0)] float boatSpeed;

    public static DynamicObstacleManager Instance { get; private set; }

    void Awake()
    {

        #region Singleton

        if (Instance != null)
        {
            Destroy(transform.root.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(transform.root.gameObject);

        #endregion
    }

    public float GetSpeed(string value) {
        return value.ToLower() switch
        {
            "car" => carSpeed,
            "train" => trainSpeed,
            "boat" => boatSpeed,
            _ => 0,
        };
    }
}
