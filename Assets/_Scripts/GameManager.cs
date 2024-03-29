using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] List<DefinitionLayout> layouts = null;
    [SerializeField, Min(0)] int layoutName;

    [SerializeField] Language language;

    [SerializeField] MapManager mapManager;
    [SerializeField] public GameObject[] _skins;
    [SerializeField] private GameObject _playerMeshParent;

    private AudioManager audioManager;

    public bool[] skinsUnlocked = new bool[]
    {
        true, false, false, false, false, false, false, false, false, false, false, false, false, false, false
    };
    public int lastSkinUsed;

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

        audioManager = GetComponent<AudioManager>();

        StartCoroutine(skinSpawenerRoutine());
    }
    
    private void SkinSpawner(int skinIndex)
    {
        GameObject oldSkin = GameObject.FindGameObjectWithTag("Skin");
        Destroy(oldSkin);
        Transform playerTransform = _playerMeshParent.transform;
        Instantiate(_skins[skinIndex], new Vector3(0f, 0.3f, 0f), Quaternion.identity, playerTransform);
        lastSkinUsed = skinIndex;
        
    }
    private IEnumerator skinSpawenerRoutine()
    {
        yield return new WaitForNextFrameUnit();
        SkinSpawner(lastSkinUsed);
    }


    public DefinitionLayout CurrentLayout => layouts[layoutName];

    public MapManager MapManager => mapManager;

    public AudioManager AudioManager => audioManager;
}
