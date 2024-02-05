using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class ProceduralTerrainGenerator2 : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] Transform _player;

    List<Pool> currentPoolTheme = new();
    List<Pool> otherPoolTheme = new();

    private List<GameObject> _activeChunks = new();
    private float _lastPlayerPosition;
    private Dictionary<Terrain, List<Props>> _chunkObstacles = new();
    private readonly int _voxelSize = 1;

    private LevelManager levelManager;
    private int currentTheme;

    private int chunckID;

    void Start()
    {
        levelManager = LevelManager.Instance;

        //_player.position = new(-(levelManager.ChunckWidth / 2f), _player.position.y, _player.position.z);

        GenerateInitialChunks();

        //Debug.Log(GetNormalizedSpawnPercentage(levelManager.Layout.Theme(0).TerrainList, 0, 1, GetTerrainFrequency));
        //Debug.Log(GetNormalizedSpawnPercentage(levelManager.Layout.Theme(0).Terrain(0).PropList, 0, 1, GetPropsFrequency));
    }

    void Update()
    {
        UpdateChunks();
    }

    void GenerateInitialChunks()
    {
        GenerateObjectPoolerTheme(0, 0, currentPoolTheme);
        for (int i = 0; i < levelManager.VisibleChunks; i++)
        {
            SpawnChunk(i * levelManager.ChunckLength);
        }
    }

    void UpdateChunks()
    {

        (otherPoolTheme, currentPoolTheme) = (currentPoolTheme, otherPoolTheme);

        if (_player.position.x < 0) return;
        int playerChunkIndex = Mathf.FloorToInt(_player.position.x / levelManager.ChunckLength);

        if (playerChunkIndex * levelManager.ChunckLength != Mathf.FloorToInt(_lastPlayerPosition / levelManager.ChunckLength) * levelManager.ChunckLength)
        {
            // load new chunk
            for (int i = playerChunkIndex - levelManager.VisibleChunks; i <= playerChunkIndex + levelManager.VisibleChunks; i++)
            {
                //if (!IsChunkActive(i))
                //{
                //    SpawnChunk(i * levelManager.ChunckLength);
                //}
            }

            // unload old chunk
            for (int i = _activeChunks.Count - 1; i >= 0; i--)
            {
                int chunkIndex = Mathf.FloorToInt(_activeChunks[i].transform.position.x / levelManager.ChunckLength);
                //if (Mathf.Abs(playerChunkIndex - chunkIndex) > levelManager.VisibleChunks)
                //{
                //    UnloadChunk(_activeChunks[i]);
                //}
            }

            _lastPlayerPosition = _player.position.x;
        }
    }

    void SpawnChunk(float position)
    {
        GameObject newChunk = new("Chunk");
        newChunk.transform.position = new Vector3(position, 0f, 0f);

        chunckID = Mathf.CeilToInt(position / levelManager.ChunckLength);

        _activeChunks.Add(newChunk);

        for (float i = 0; i < levelManager.ChunckLength; i += _voxelSize)
        {
            Terrain terrain = null;

            float random = Random.Range(0f, 100f);
            float tmp = 0;

            for (int j = 0; j < currentPoolTheme.Count; j++)
            {
                tmp += GetNormalizedSpawnPercentage(levelManager.Layout.Theme(currentTheme).TerrainList, j, chunckID, GetTerrainFrequency);
                if (random <= tmp)
                {
                    terrain = currentPoolTheme[j].Terrain.GetObject();
                    tmp = j;
                    break;
                }
            }

            terrain.transform.position = new(0f, 0f, position + i);
            terrain.transform.SetParent(newChunk.transform);

            terrain.transform.localScale = new(levelManager.ChunckWidth, 1f, 1f);

            GenerateObstacles(terrain, currentPoolTheme[(int)tmp], (int)tmp);
        }
    }

    void GenerateObstacles(Terrain terrain, Pool obejctPool, int terrainID)
    {

        List<Props> props = terrain.AllPropsList;

        bool isReverse = Random.Range(0, 2) == 0;

        bool isSingleType = terrain.IsSingleType;

        Props prop = null;

        if (isSingleType)
        {
            float random = Random.Range(0f, 100f);
            float tmp = 0;

            for (int j = 0; j < obejctPool.PropsList.Count; j++)
            {
                tmp += GetNormalizedSpawnPercentage(terrain.PropList, j, chunckID, GetPropsFrequency);
                if (random <= tmp)
                {
                    prop = levelManager.Layout.Theme(currentTheme).Terrain(terrainID).AllProp(j); //cambiare sistemare ogni tanto un easter egg
                    tmp = j;
                    break;
                }
            }
            if (prop.Max == 1)
            {
                prop = obejctPool.Props((int)tmp).GetObject();

                if (prop.GetComponent<DynamicProps>() != null && isReverse) prop.GetComponent<DynamicProps>().Reverse();

                prop.transform.position = new(0, 0, terrain.transform.position.z);

                prop.transform.SetParent(terrain.transform);
            }
            else
            {
                int rowWidth = (int)terrain.transform.localScale.x;

                int maxObjectInRow = Mathf.FloorToInt(rowWidth - 2 / (int)prop.Size);

                float difficulty = terrain.Frequency(chunckID);

                int objectInRow = Mathf.Clamp(Mathf.CeilToInt(difficulty * rowWidth * levelManager.Layout.ObstacleDensity), 0, maxObjectInRow);

                bool[] isOccupied = new bool[rowWidth];

                for (int i = 0; i < objectInRow; i++)
                {
                    int tp;
                    do
                    {
                        tp = Mathf.CeilToInt(Random.Range(0, rowWidth));
                    } while (isOccupied[tp] || isOccupied[tp + (prop.Size - 1) >= isOccupied.Length ? (prop.Size - 2) : tp + (prop.Size - 1)]);
                    for (int j = tp; j < tp + prop.Size - 1; j++)
                    {
                        isOccupied[j + (prop.Size - 1) >= isOccupied.Length ? (prop.Size - 2) : j + (prop.Size - 1)] = true;
                    }
                    prop = obejctPool.Props((int)tmp).GetObject();

                    if (prop.GetComponent<DynamicProps>() != null && isReverse) prop.GetComponent<DynamicProps>().Reverse();

                    prop.transform.position = new(tp, 0, terrain.transform.position.z);
                    prop.transform.SetParent(terrain.transform);
                }
            }
        }
        else
        {
            int rowWidth = (int)terrain.transform.localScale.z;

            int maxObjectInRow = Mathf.FloorToInt(rowWidth - 2);

            float difficulty = terrain.Frequency(chunckID);

            int objectInRow = Mathf.Clamp(Mathf.CeilToInt(difficulty * rowWidth * levelManager.Layout.ObstacleDensity), 0, maxObjectInRow);

            bool[] isOccupied = new bool[rowWidth];

            for (int i = 0; i < objectInRow; i++)
            {
                int tp;
                do
                {
                    tp = Mathf.CeilToInt(Random.Range(0, rowWidth));
                } while (isOccupied[tp]);
                for (int j = tp; j < tp; j++)
                {
                    isOccupied[j] = true;
                }

                float random = Random.Range(0f, 100f);
                float tmp = 0;

                for (int j = 0; j < obejctPool.PropsList.Count; j++)
                {
                    tmp += GetNormalizedSpawnPercentage(terrain.PropList, j, chunckID, GetPropsFrequency);
                    if (random <= tmp)
                    {
                        prop = levelManager.Layout.Theme(currentTheme).Terrain(terrainID).AllProp(j);
                        tmp = j;
                        break;
                    }
                }
                prop = obejctPool.Props((int)tmp).GetObject();

                if (prop.GetComponent<DynamicProps>() != null && isReverse) prop.GetComponent<DynamicProps>().Reverse();

                prop.transform.position = new(tp - levelManager.ChunckWidth / 2, 0, terrain.transform.position.z); //correggere
                prop.transform.SetParent(terrain.transform);
            }
        }
    }

    float GetTerrainFactor(Terrain terrain)
    {
        // Calcola il fattore specifico per il terreno in base alla tua logica
        // Puoi modificare questo metodo in base alle tue esigenze specifiche
        return terrain.Frequency(0);  // Stiamo usando il primo chunk come esempio
    }


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

    /// <summary>
    /// Destroys all object poolers in the other pool theme.
    /// </summary>
    void DestroyObjectPoolerTheme()
    {
        if (otherPoolTheme.Any())
        {
            foreach (var item in otherPoolTheme)
            {
                item.DestroyPoolers();
            }
        }
    }

    /// <summary>
    /// Generates and configures object pools for terrains and associated props based on the specified theme's configurations.
    /// </summary>
    /// <param name="currentThemeIndex">Index of the current theme.</param>
    /// <param name="chunkCounter">Counter for chunks in the game.</param>
    /// <param name="pool">List to store the generated object pools.</param>
    void GenerateObjectPoolerTheme(int currentThemeIndex, int chunkCounter, List<Pool> pool)
    {
        int[] terrainSpawnPercentage = new int[levelManager.Layout.Theme(currentThemeIndex).TerrainList.Count];
        int[][] propSpawnPercentage = new int[levelManager.Layout.Theme(currentThemeIndex).TerrainList.Count][];
        for (int i = 0; i < levelManager.Layout.Theme(currentThemeIndex).TerrainList.Count; i++)
        {
            terrainSpawnPercentage[i] = Mathf.CeilToInt(GetNormalizedSpawnPercentage(levelManager.Layout.Theme(currentThemeIndex).TerrainList, i, chunkCounter, GetTerrainFrequency) / levelManager.ChunckLength);
            Terrain tmp = levelManager.Layout.Theme(currentThemeIndex).Terrain(i);
            propSpawnPercentage[i] = new int[tmp.PropList.Count + tmp.RarityList.Count];
            List<Props> allProps = tmp.PropList.Concat(tmp.RarityList).ToList();
            for (int j = 0; j < allProps.Count; j++)
            {
                propSpawnPercentage[i][j] = Mathf.CeilToInt(GetNormalizedSpawnPercentage(allProps, j, chunkCounter, GetPropsFrequency) / levelManager.ChunckWidth);
            }
        }

        for (int i = 0; i < terrainSpawnPercentage.Length; i++)
        {
            ObjectPooler<Terrain> objectPooler;
            Terrain tmp = levelManager.Layout.Theme(currentThemeIndex).Terrain(i);
            objectPooler = new ObjectPooler<Terrain>(tmp, terrainSpawnPercentage[i] * levelManager.VisibleChunks, transform);
            Pool pool1 = new(objectPooler);
            for (int j = 0; j < propSpawnPercentage[i].Length; j++)
            {
                ObjectPooler<Props> objectPooler2;
                int num = propSpawnPercentage[i][j] * terrainSpawnPercentage[i] * levelManager.VisibleChunks;
                objectPooler2 = new(j < tmp.PropList.Count ? tmp.PropList[j] : tmp.RarityList[j - tmp.PropList.Count], num, transform);
                pool1.Add(objectPooler2);
            }
            pool.Add(pool1);
        }
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
}