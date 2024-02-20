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
    [SerializeField] private GameObject[] _skins;
    [SerializeField] private GameObject _playerMeshParent;

    private void OnEnable()
    {
        EventManager.OnReload += AwakeInizializer;
        EventManager.OnSkinChoice += SkinSpawner;
    }
    private void OnDisable()
    {
        EventManager.OnReload -= AwakeInizializer;
        EventManager.OnSkinChoice -= SkinSpawner;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();//change for open menu
        }
    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    private void SkinSpawner(int skinIndex)
    {
        GameObject oldSkin = GameObject.FindGameObjectWithTag("Skin");
        Destroy(oldSkin);
        Transform playerTransform = _playerMeshParent.transform;
        Instantiate(_skins[skinIndex], new Vector3(0f, 0.3f, 0f), Quaternion.identity, playerTransform);
        
    }


    public DefinitionLayout CurrentLayout => layouts[layoutName];

    public MapManager MapManager => mapManager;
}
