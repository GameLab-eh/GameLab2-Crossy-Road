using System.Collections.Generic;
using UnityEngine;

public class VoxelTerrainGenerator : MonoBehaviour
{
    public GameObject chunkPrefab;
    public GameObject obstaclePrefab;
    public Transform player;
    public int chunkSize = 10;
    public int voxelSize = 1;
    public int visibleChunks = 3;
    public int obstacleDensity = 2; // Numero medio di ostacoli per chunk

    private List<GameObject> activeChunks = new List<GameObject>();
    private float lastPlayerPosition;

    void Start()
    {
        GenerateInitialChunks();
    }

    void Update()
    {
        UpdateChunks();
    }

    void GenerateInitialChunks()
    {
        for (int i = 0; i < visibleChunks; i++)
        {
            SpawnChunk(i * chunkSize);
        }
    }

    void UpdateChunks()
    {
        int playerChunkIndex = Mathf.FloorToInt(player.position.x / chunkSize);

        // Controlla se il giocatore si è spostato a un nuovo chunk
        if (playerChunkIndex * chunkSize != Mathf.FloorToInt(lastPlayerPosition / chunkSize) * chunkSize)
        {
            // Carica nuovi chunk
            for (int i = playerChunkIndex - visibleChunks; i <= playerChunkIndex + visibleChunks; i++)
            {
                if (!IsChunkActive(i))
                {
                    SpawnChunk(i * chunkSize);
                }
            }

            // Scarica chunk non più visibili
            for (int i = activeChunks.Count - 1; i >= 0; i--)
            {
                int chunkIndex = Mathf.FloorToInt(activeChunks[i].transform.position.x / chunkSize);
                if (Mathf.Abs(playerChunkIndex - chunkIndex) > visibleChunks)
                {
                    UnloadChunk(activeChunks[i]);
                }
            }

            lastPlayerPosition = player.position.x;
        }
    }

    void SpawnChunk(float positionX)
    {
        GameObject newChunk = Instantiate(chunkPrefab, new Vector3(positionX, 0f, 0f), Quaternion.identity);
        activeChunks.Add(newChunk);

        // Posiziona ostacoli casualmente nel chunk
        for (int i = 0; i < chunkSize * obstacleDensity; i++)
        {
            float obstacleX = Random.Range(positionX, positionX + chunkSize);
            float obstacleY = Random.Range(0.5f, 3f); // Altezza casuale
            Instantiate(obstaclePrefab, new Vector3(obstacleX, obstacleY, 0f), Quaternion.identity, newChunk.transform);
        }
    }

    void UnloadChunk(GameObject chunk)
    {
        activeChunks.Remove(chunk);
        Destroy(chunk);
    }

    bool IsChunkActive(int index)
    {
        foreach (GameObject chunk in activeChunks)
        {
            int chunkIndex = Mathf.FloorToInt(chunk.transform.position.x / chunkSize);
            if (chunkIndex == index)
            {
                return true;
            }
        }
        return false;
    }
}
