using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] List<DefinitionLayout> layouts = null;
    [SerializeField, Min(0)] int layoutName;

    [SerializeField] Language language;

    [SerializeField] MapManager mapManager;

    private void OnEnable()
    {
        EventManager.OnReload += AwakeInizializer;
    }
    private void OnDisable()
    {
        EventManager.OnReload -= AwakeInizializer;
    }
    void Awake()
    {
        AwakeInizializer();
    }
    private void AwakeInizializer()
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


    public DefinitionLayout CurrentLayout => layouts[layoutName];

    public MapManager MapManager => mapManager;
}
