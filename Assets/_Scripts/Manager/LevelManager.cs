#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField, Min(5)] int chunckLength = 10;
    [SerializeField, Min(3)] int chunckWidth = 21;
    [SerializeField] int visibleChunks = 3;

    [SerializeField] Layout layout;

    public static LevelManager Instance { get; private set; }
    
    private void OnEnable()
    {
        EventManager.OnReload += AwakeInizializer;
        EventManager.OnReload -= StartInizializer;
    }
    private void OnDisable()
    {
        EventManager.OnReload -= AwakeInizializer;
        EventManager.OnReload -= StartInizializer;
    }
    void Awake()
    {
        AwakeInizializer();
    }

    private void Start()
    {
        StartInizializer();
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
    private void StartInizializer()
    {
        // Example of interaction with a layout and its components
        //Debug.Log(layout.Name);
        //Debug.Log(layout.Theme[0]);
        //Debug.Log(layout.Theme[0].Name);
        //Debug.Log(layout.Theme[0].Terrain[0]);
        //Debug.Log(layout.Theme[0].Terrain[0].Props[0].Size);
    }

    public int ChunckWidth
    {
        get { return chunckWidth; }
        set { chunckWidth = Mathf.Max(3, value); }
    }
    public int ChunckLength => chunckLength;
    public int VisibleChunks => visibleChunks;
    public Layout Layout
    {
        get { return layout; }
        set { layout = value; }
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelManager levelManager = (LevelManager)target;

        EditorGUI.BeginChangeCheck();

        int newChunkWidth = EditorGUILayout.IntField("Chunk Width (Odd only)", levelManager.ChunckWidth);

        if (EditorGUI.EndChangeCheck())
        {
            levelManager.ChunckWidth = ((newChunkWidth % 2 == 0) ? newChunkWidth + 1 : newChunkWidth);
            EditorUtility.SetDirty(levelManager);
        }

        DrawDefaultInspector();
    }
}
#endif