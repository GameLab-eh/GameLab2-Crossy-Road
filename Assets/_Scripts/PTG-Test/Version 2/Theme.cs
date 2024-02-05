using System.Collections.Generic;
using UnityEngine;

public class Theme : MonoBehaviour
{
    [SerializeField] List<Terrain> terrainList;

    public List<Terrain> Terrain => terrainList;
    public string Name => this.name;
}
