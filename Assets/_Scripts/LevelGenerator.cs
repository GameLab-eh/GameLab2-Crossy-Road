using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject voxelPrefab; // Prefab del voxel (ad esempio, un cubo)
    public float voxelSize = 1f; // Dimensione del voxel
    public int chunkWidth = 10; // Larghezza del chunk in linee di voxel
    public int chunkDepth = 20; // Profondità del chunk in linee di voxel
    public int chunkHeight = 5; // Altezza massima del terreno in voxel
    public float obstacleProbability = 0.3f; // Probabilità di posizionare un ostacolo

    void Start()
    {
        GenerateChunk();
    }

    void GenerateChunk()
    {
        for (int x = 0; x < chunkWidth; x++)
        {
            for (int z = 0; z < chunkDepth; z++)
            {
                // Genera il terreno
                int groundHeight = Random.Range(1, chunkHeight);
                for (int y = 0; y < groundHeight; y++)
                {
                    InstantiateVoxel(new Vector3(x * voxelSize, y * voxelSize, z * voxelSize));
                }

                // Posiziona ostacoli casualmente
                if (Random.value < obstacleProbability && groundHeight > 1)
                {
                    int obstacleHeight = Random.Range(1, groundHeight);
                    InstantiateVoxel(new Vector3(x * voxelSize, obstacleHeight * voxelSize, z * voxelSize), Color.red);
                }
            }
        }
    }

    void InstantiateVoxel(Vector3 position, Color? color = null)
    {
        GameObject voxel = Instantiate(voxelPrefab, position, Quaternion.identity);
        voxel.transform.localScale = new Vector3(voxelSize, voxelSize, voxelSize);
        if (color != null)
        {
            voxel.transform.GetChild(0).GetComponent<Renderer>().material.color = color.Value;
        }
    }
}
