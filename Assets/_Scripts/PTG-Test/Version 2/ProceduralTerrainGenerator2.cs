using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

[System.Serializable]
public class ProceduralTerrainGenerator2 : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] Transform _player;

    private List<GameObject> _activeChunks = new();
    private float _lastPlayerPosition;
    private Dictionary<GameObject, List<GameObject>> _chunkObstacles = new();
    private readonly int _voxelSize = 1;

    private int chunckID;

    private LevelManager levelManager;

    void Start()
    {
        levelManager = LevelManager.Instance;

        _player.position = new(_player.position.x, _player.position.y, levelManager.ChunckWidth / 2f);

        //GenerateInitialChunks();

        //Debug.Log(GetNormalizedSpawnPercentage(levelManager.Layout.Theme(0).TerrainList, 0, 1, GetTerrainFrequency));
        //Debug.Log(GetNormalizedSpawnPercentage(levelManager.Layout.Theme(0).Terrain(0).PropList, 0, 1, GetPropsFrequency));

        GetNormalizedNumberTerrain(0, 1);
    }

    void GetNormalizedNumberTerrain(int currentThemeIndex, int chunkCounter)
    {
        int[] terrainSpawnPercentage = new int[levelManager.Layout.Theme(currentThemeIndex).TerrainList.Count];
        int[][] propSpawnPercentage = new int[levelManager.Layout.Theme(currentThemeIndex).TerrainList.Count][];
        for (int i = 0; i < levelManager.Layout.Theme(currentThemeIndex).TerrainList.Count; i++)
        {
            terrainSpawnPercentage[i] = Mathf.CeilToInt(GetNormalizedSpawnPercentage(levelManager.Layout.Theme(currentThemeIndex).TerrainList, i, chunkCounter, GetTerrainFrequency) / levelManager.ChunckLength);
            propSpawnPercentage[i] = new int[levelManager.Layout.Theme(currentThemeIndex).Terrain(i).PropList.Count + levelManager.Layout.Theme(currentThemeIndex).Terrain(i).RarityList.Count];
            List<Props> allProps = levelManager.Layout.Theme(currentThemeIndex).Terrain(i).PropList.Concat(levelManager.Layout.Theme(currentThemeIndex).Terrain(i).RarityList).ToList();
            for (int j = 0; j < allProps.Count; j++)
            {
                propSpawnPercentage[i][j] = Mathf.CeilToInt(GetNormalizedSpawnPercentage(allProps, j, chunkCounter, GetPropsFrequency) / levelManager.ChunckWidth);
            }
        }

        //string text = null;
        //for (int i = 0; i < terrainSpawnPercentage.Length; i++)
        //{
        //    text = ($"{levelManager.Layout.Theme(currentThemeIndex).Terrain(i).name} = {terrainSpawnPercentage[i]}");
        //    for (int j = 0; j < propSpawnPercentage[i].Length; j++)
        //    {
        //        int tp = (j - levelManager.Layout.Theme(currentThemeIndex).Terrain(i).PropList.Count);
        //        string name = j < levelManager.Layout.Theme(currentThemeIndex).Terrain(i).PropList.Count ? levelManager.Layout.Theme(currentThemeIndex).Terrain(i).PropList[j].name : levelManager.Layout.Theme(currentThemeIndex).Terrain(i).RarityList[tp].name;
        //        text += ($" | {name} = {propSpawnPercentage[i][j]}");
        //    }
        //    Debug.Log(text);
        //}



        List<Pool> pool = new();

        for (int i = 0; i < terrainSpawnPercentage.Length; i++)
        {
            ObjectPooler<Terrain> objectPooler;
            Terrain tmp = levelManager.Layout.Theme(currentThemeIndex).Terrain(i);
            objectPooler = new ObjectPooler<Terrain>(tmp, terrainSpawnPercentage[i] * levelManager.VisibleChunks);
            Pool pool1 = new(objectPooler);
            for (int j = 0; j < propSpawnPercentage[i].Length; j++)
            {
                ObjectPooler<Props> objectPooler2;
                int num = propSpawnPercentage[i][j] * terrainSpawnPercentage[i] * levelManager.VisibleChunks;
                objectPooler2 = new(j < tmp.PropList.Count ? tmp.PropList[j] : tmp.RarityList[j - tmp.PropList.Count], num);
                pool1.Add(objectPooler2);
            }
            pool.Add(pool1);
        }

        //example
        Terrain tp = pool[0].Terrain.GetObject();
        tp.transform.localScale = new(levelManager.ChunckWidth, 1, 1);
        pool[0].Terrain.ReturnObject(tp);

    }

    /// <summary>
    /// Calculates the normalized spawn percentage for a specific object based on the provided object list, index, and chunk counter.
    /// </summary>
    /// <typeparam name="T">Type of the object in the list.</typeparam>
    /// <param name="objectList">List of objects for which to calculate the spawn percentage.</param>
    /// <param name="index">Index of the object for which to calculate the spawn percentage.</param>
    /// <param name="chunkCounter">Current chunk counter.</param>
    /// <param name="frequencyFunc">Delegate to obtain the spawn frequency for an object.</param>
    /// <returns>The normalized spawn percentage for the specified object.</returns>
    float GetNormalizedSpawnPercentage<T>(List<T> objectList, int index, int chunkCounter, Func<T, int, float> frequencyFunc)
    {
        float total = 0;

        // Calculate the total sum of spawn frequencies for all objects
        for (int i = 0; i < objectList.Count; i++)
        {
            total += frequencyFunc(objectList[i], chunkCounter);
        }

        // Return the normalized spawn percentage, ensuring no division by zero
        return total > 0 ? (frequencyFunc(objectList[index], chunkCounter) * 100) / total : 0;
    }

    /// <summary>
    /// Calculates the spawn frequency for a specific terrain based on the provided terrain and chunk counter.
    /// </summary>
    /// <param name="terrain">The terrain object for which to calculate the spawn frequency.</param>
    /// <param name="chunkCounter">The current chunk counter.</param>
    /// <returns>The spawn frequency for the specified terrain.</returns>
    float GetTerrainFrequency(Terrain terrain, int chunkCounter) => terrain.Frequency(chunkCounter);

    /// <summary>
    /// Calculates the spawn frequency for specific props based on the provided props and chunk counter.
    /// </summary>
    /// <param name="props">The props object for which to calculate the spawn frequency.</param>
    /// <param name="chunkCounter">The current chunk counter.</param>
    /// <returns>The spawn frequency for the specified props.</returns>
    float GetPropsFrequency(Props props, int chunkCounter) => props.Frequency;





    //void Update()
    //{
    //    UpdateChunks();
    //}

    //void GenerateInitialChunks()
    //{
    //    for (int i = 0; i < levelManager.VisibleChunks; i++)
    //    {
    //        SpawnChunk(i * levelManager.ChunckLength);
    //    }
    //}

    //void UpdateChunks()
    //{
    //    if (_player.position.x < 0) return;
    //    int playerChunkIndex = Mathf.FloorToInt(_player.position.x / levelManager.ChunckLength);

    //    if (playerChunkIndex * levelManager.ChunckLength != Mathf.FloorToInt(_lastPlayerPosition / levelManager.ChunckLength) * levelManager.ChunckLength)
    //    {
    //        // Carica nuovi chunk
    //        for (int i = playerChunkIndex - levelManager.VisibleChunks; i <= playerChunkIndex + levelManager.VisibleChunks; i++)
    //        {
    //            if (!IsChunkActive(i))
    //            {
    //                SpawnChunk(i * levelManager.ChunckLength);
    //            }
    //        }

    //        // Scarica chunk non più visibili
    //        for (int i = _activeChunks.Count - 1; i >= 0; i--)
    //        {
    //            int chunkIndex = Mathf.FloorToInt(_activeChunks[i].transform.position.x / levelManager.ChunckLength);
    //            if (Mathf.Abs(playerChunkIndex - chunkIndex) > levelManager.VisibleChunks)
    //            {
    //                UnloadChunk(_activeChunks[i]);
    //            }
    //        }

    //        _lastPlayerPosition = _player.position.x;
    //    }
    //}

    //void SpawnChunk(float positionX)
    //{
    //    // Creazione di un nuovo oggetto per il chunk
    //    GameObject newChunk = new GameObject("Chunk");
    //    newChunk.transform.position = new Vector3(positionX, 0f, 0f);

    //    chunckID = Mathf.CeilToInt(positionX / length);

    //    _activeChunks.Add(newChunk);

    //    // Generazione di righe fino a riempire il chunk
    //    for (float z = 0; z < length; z += _voxelSize)
    //    {
    //        // Seleziona randomicamente un prefab da _terrainPrefabs
    //        GameObject terrainPrefab = _terrainPrefabs[Random.Range(0, _terrainPrefabs.Count)];
    //        GameObject terrainObject = Instantiate(terrainPrefab, new Vector3(0f, 0f, positionX + z), Quaternion.identity, newChunk.transform);  // Inverti X e Z

    //        // Modifica la scala del terreno
    //        float scaleMultiplier = difficultyCurve.Evaluate(z / width);
    //        terrainObject.transform.localScale = new Vector3(width, 1f, scaleMultiplier);  // Inverti X e Z

    //        // Se il terreno è un'aiuola, aggiungi ostacoli
    //        switch (terrainPrefab.tag)
    //        {
    //            case "Grass":
    //                GenerateObstacles(terrainObject, _obstacleGrass, false, chunckID);
    //                break;
    //            case "Road":
    //                bool isFast = Random.Range(0, 2) == 0;
    //                GenerateObstacles(terrainObject, isFast ? _obstacleRoad : _obstacleRoad2, false, chunckID);
    //                break;
    //            case "Railroad":
    //                GenerateObstacles(terrainObject, _obstacleRailroad, true, chunckID);
    //                break;
    //            case "River":
    //                GenerateObstacles(terrainObject, _obstacleRiver, false, (100f - chunckID));
    //                break;
    //            case "Lake":
    //                GenerateObstacles(terrainObject, _obstacleLake, false, (100f - chunckID));
    //                break;
    //            case "Highway":
    //                GenerateObstacles(terrainObject, _obstacleHighway, false, (100f - chunckID));
    //                break;
    //        }
    //    }
    //}

    //void GenerateObstacles(GameObject terrainObject, List<GameObject> obstacleList, bool single, float factor)
    //{
    //    bool isReverse = Random.Range(0, 2) == 0;

    //    bool isDynamic = obstacleList[0].GetComponent<ObstacleMotion>() != null;

    //    List<GameObject> obstacles = new List<GameObject>();
    //    _chunkObstacles.Add(terrainObject, obstacles);

    //    float rowWidth = terrainObject.transform.localScale.z * _voxelSize;  // Inverti X e Z

    //    float _factor = factor / 100f;
    //    float difficultyPercentage = difficultyCurve.Evaluate(_factor);
    //    float spawnPercentage = spawnCurve.Evaluate(_factor);

    //    int size = Mathf.Clamp(Mathf.CeilToInt(obstacleList[0].transform.GetChild(0).transform.localScale.x), 1, 2);  // Inverti X e Z

    //    int obstacleCount = (int)Mathf.Clamp(Mathf.CeilToInt(difficultyPercentage * rowWidth * _obstacleDensity), 0, rowWidth - Mathf.CeilToInt(rowWidth / 8) * 2);

    //    obstacleCount /= size;

    //    if (single) obstacleCount = 1;

    //    bool[] @bool = new bool[(int)rowWidth];

    //    for (int i = 0; i < obstacleCount; i++)
    //    {
    //        int tp;
    //        do
    //        {
    //            tp = Mathf.CeilToInt(Random.Range(terrainObject.transform.position.x - 1, terrainObject.transform.position.x - 1 + rowWidth));  // Inverti X e Z
    //        } while (@bool[tp] || @bool[(int)(tp + (size / 2)) > (terrainObject.transform.position.x - 1 + rowWidth) ? 0 : (int)(tp + (size / 2))]);

    //        @bool[tp] = true;
    //        @bool[(int)(tp + (size / 2)) > (terrainObject.transform.position.x - 1 + rowWidth) ? 0 : (int)(tp + (size / 2))] = true;

    //        GameObject obstaclePrefab = obstacleList[Random.Range(0, obstacleList.Count)];

    //        if (terrainObject.CompareTag("Grass") && Random.value < spawnPercentage) obstaclePrefab = _takeable[Random.Range(0, _takeable.Count)];

    //        GameObject obstacle = Instantiate(obstaclePrefab, new Vector3(tp, 0f, terrainObject.transform.position.x), Quaternion.identity, terrainObject.transform);  // Inverti X e Z
    //        obstacle.transform.localScale = new Vector3(1f / width, 1f, 1f);  // Inverti X e Z

    //        if (isDynamic) if (isReverse) obstacle.GetComponent<ObstacleMotion>().Reverse();

    //        obstacles.Add(obstacle);
    //    }
    //}

    //void UnloadChunk(GameObject chunk)
    //{
    //    _activeChunks.Remove(chunk);
    //    Destroy(chunk);

    //    // Rimuovi ostacoli dal dizionario
    //    if (_chunkObstacles.ContainsKey(chunk))
    //    {
    //        _chunkObstacles.Remove(chunk);
    //    }
    //}

    //bool IsChunkActive(int index)
    //{
    //    foreach (GameObject chunk in _activeChunks)
    //    {
    //        int chunkIndex = Mathf.FloorToInt(chunk.transform.position.x / length);
    //        if (chunkIndex == index)
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}
}

public class Pool
{
    ObjectPooler<Terrain> terrain;
    List<ObjectPooler<Props>> propsList = new();

    public Pool(ObjectPooler<Terrain> pooler)
    {
        terrain = pooler;
    }

    public void SetTerrain(ObjectPooler<Terrain> pooler) => terrain = pooler;
    public void Add(ObjectPooler<Props> pooler) => propsList.Add(pooler);

    public ObjectPooler<Terrain> Terrain { get { return terrain; } }
    public List<ObjectPooler<Props>> PropsList { get { return propsList; } }

    public ObjectPooler<Terrain> ObjectPooler { get; }

    public ObjectPooler<Props> Props(int index) { return propsList[index]; }
}