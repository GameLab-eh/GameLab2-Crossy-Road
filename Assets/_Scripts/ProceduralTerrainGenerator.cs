using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProceduralTerrainGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] List<GameObject> _terrainPrefabs;
    [SerializeField] List<GameObject> _obstacleGrass;
    [SerializeField] List<GameObject> _obstacleRoad;
    [SerializeField] List<GameObject> _obstacleRoad2;
    [SerializeField] List<GameObject> _obstacleRailroad;
    [SerializeField] List<GameObject> _obstacleRiver;
    [SerializeField] List<GameObject> _obstacleLake;
    [SerializeField] List<GameObject> _obstacleHighway;

    [Space]
    [SerializeField] List<GameObject> _takeable;
    [SerializeField] AnimationCurve spawnCurve;

    [Header("Player")]
    [SerializeField] Transform _player;

    [Header("Start area")]
    [SerializeField] GameObject _departureArea;

    [Header("Chunck data")]
    [SerializeField] int _visibleChunks = 3;
    [SerializeField] int _obstacleDensity = 2;
    [SerializeField] AnimationCurve difficultyCurve;

    [Header("Chunk size")]
    [SerializeField, Min(1)] int length = 20;
    int width;

    private List<GameObject> _activeChunks = new();
    private float _lastPlayerPosition;
    private Dictionary<GameObject, List<GameObject>> _chunkObstacles = new();
    private readonly int _voxelSize = 1;

    private int chunckID;

    void Start()
    {
        width = LevelManager.Instance.ChunckWidth;

        _player.position = new(_player.position.x, _player.position.y, width / 2f);

        _departureArea.transform.localScale = new(_departureArea.transform.localScale.x, 1, width);

        GenerateInitialChunks();
    }

    void Update()
    {
        UpdateChunks();
    }

    void GenerateInitialChunks()
    {
        for (int i = 0; i < _visibleChunks; i++)
        {
            SpawnChunk(i * length);
        }
    }

    void UpdateChunks()
    {
        if (_player.position.x < 0) return;
        int playerChunkIndex = Mathf.FloorToInt(_player.position.x / length);

        // Controlla se il giocatore si è spostato a un nuovo chunk
        if (playerChunkIndex * length != Mathf.FloorToInt(_lastPlayerPosition / length) * length)
        {
            // Carica nuovi chunk
            for (int i = playerChunkIndex - _visibleChunks; i <= playerChunkIndex + _visibleChunks; i++)
            {
                if (!IsChunkActive(i))
                {
                    SpawnChunk(i * length);
                }
            }

            // Scarica chunk non più visibili
            for (int i = _activeChunks.Count - 1; i >= 0; i--)
            {
                int chunkIndex = Mathf.FloorToInt(_activeChunks[i].transform.position.x / length);
                if (Mathf.Abs(playerChunkIndex - chunkIndex) > _visibleChunks)
                {
                    UnloadChunk(_activeChunks[i]);
                }
            }

            _lastPlayerPosition = _player.position.x;
        }
    }

    void SpawnChunk(float positionX)
    {
        // Creazione di un nuovo oggetto per il chunk
        GameObject newChunk = new("Chunk");
        newChunk.transform.position = new Vector3(positionX, 0f, 0f);

        chunckID = Mathf.CeilToInt(positionX / length);

        _activeChunks.Add(newChunk);

        // Generazione di righe fino a riempire il chunk
        for (float z = 0; z < length; z += _voxelSize)
        {
            // Seleziona randomicamente un prefab da _terrainPrefabs
            GameObject terrainPrefab = _terrainPrefabs[Random.Range(0, _terrainPrefabs.Count)];
            GameObject terrainObject = Instantiate(terrainPrefab, new Vector3(positionX + z, 0f, 0f), Quaternion.identity, newChunk.transform);

            // Modifica la scala del terreno
            float scaleMultiplier = difficultyCurve.Evaluate(z / width);
            terrainObject.transform.localScale = new Vector3(1f, 1f, width);

            // Se il terreno è un'aiuola, aggiungi ostacoli
            switch (terrainPrefab.tag)
            {
                case "Grass":
                    GenerateObstacles(terrainObject, _obstacleGrass, false, chunckID);
                    break;
                case "Road":
                    bool isFast = Random.Range(0, 2) == 0;
                    GenerateObstacles(terrainObject, isFast ? _obstacleRoad : _obstacleRoad2, false, chunckID);
                    break;
                case "Railroad":
                    GenerateObstacles(terrainObject, _obstacleRailroad, true, chunckID);
                    break;
                case "River":
                    GenerateObstacles(terrainObject, _obstacleRiver, false, (100f - chunckID));
                    break;
                case "Lake":
                    GenerateObstacles(terrainObject, _obstacleLake, false, (100f - chunckID));
                    break;
                case "Highway":
                    GenerateObstacles(terrainObject, _obstacleHighway, false, (100f - chunckID));
                    break;
            }
        }
    }

    void GenerateObstacles(GameObject terrainObject, List<GameObject> obstacleList, bool single, float factor)
    {
        bool isReverse = Random.Range(0, 2) == 0;

        bool isDynamic = obstacleList[0].GetComponent<ObstacleMotion>() != null;

        List<GameObject> obstacles = new();
        _chunkObstacles.Add(terrainObject, obstacles);

        float rowWidth = terrainObject.transform.localScale.z * _voxelSize;

        float _factor = factor / 100f;
        float difficultyPercentage = difficultyCurve.Evaluate(_factor);
        float spawnPercentage = spawnCurve.Evaluate(_factor);

        int size = Mathf.Clamp(Mathf.CeilToInt(obstacleList[0].transform.GetChild(0).transform.localScale.z), 1, 2);

        int obstacleCount = (int)Mathf.Clamp(Mathf.CeilToInt(difficultyPercentage * rowWidth * _obstacleDensity), 0, rowWidth - Mathf.CeilToInt(rowWidth / 8) * 2);

        obstacleCount /= size;

        if (single) obstacleCount = 1;

        bool[] @bool = new bool[(int)rowWidth];

        for (int i = 0; i < obstacleCount; i++)
        {
            int tmp;
            do
            {
                tmp = Mathf.CeilToInt(Random.Range(terrainObject.transform.position.z - 1, terrainObject.transform.position.z - 1 + rowWidth));
            } while (@bool[tmp] || @bool[(int)(tmp + (size / 2)) > (terrainObject.transform.position.z - 1 + rowWidth) ? 0 : (int)(tmp + (size / 2))]);

            @bool[tmp] = true;
            @bool[(int)(tmp + (size / 2)) > (terrainObject.transform.position.z - 1 + rowWidth) ? 0 : (int)(tmp + (size / 2))] = true;

            GameObject obstaclePrefab = obstacleList[Random.Range(0, obstacleList.Count)];

            if (terrainObject.CompareTag("Grass") && Random.value < spawnPercentage) obstaclePrefab = _takeable[Random.Range(0, _takeable.Count)];

            GameObject obstacle = Instantiate(obstaclePrefab, new Vector3(terrainObject.transform.position.x, 0f, tmp), Quaternion.identity, terrainObject.transform);
            obstacle.transform.localScale = new Vector3(1f, 1f, 1f / width);

            if (isDynamic) if (isReverse) obstacle.GetComponent<ObstacleMotion>().Reverse();

            obstacles.Add(obstacle);
        }
    }

    void UnloadChunk(GameObject chunk)
    {
        _activeChunks.Remove(chunk);
        Destroy(chunk);

        // Rimuovi ostacoli dal dizionario
        if (_chunkObstacles.ContainsKey(chunk))
        {
            _chunkObstacles.Remove(chunk);
        }
    }

    bool IsChunkActive(int index)
    {
        foreach (GameObject chunk in _activeChunks)
        {
            int chunkIndex = Mathf.FloorToInt(chunk.transform.position.x / length);
            if (chunkIndex == index)
            {
                return true;
            }
        }
        return false;
    }
}