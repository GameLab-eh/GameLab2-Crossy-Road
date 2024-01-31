using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField, Min(1)] int chunckWidth;


    public static LevelManager Instance { get; private set; }

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

    public int GetChunckWidth() { return chunckWidth; }
}
