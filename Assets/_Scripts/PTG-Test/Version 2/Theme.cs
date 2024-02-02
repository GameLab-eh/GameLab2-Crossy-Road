using System.Collections.Generic;
using UnityEngine;

public class Theme : MonoBehaviour
{
    [SerializeField] List<Terrain> terrainList;

    public List<Terrain> TerrainList => terrainList;
    public string Name => this.name;
    public Terrain Terrain(int index) => terrainList[index];
}
