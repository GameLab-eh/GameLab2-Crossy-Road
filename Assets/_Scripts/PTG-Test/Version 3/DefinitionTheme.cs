using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefinitionTheme", menuName = "ScriptableObjects/DefinitionTheme", order = 1)]
public class DefinitionTheme : ScriptableObject
{
    [SerializeField] List<DefinitionTerrain> terrainList;
    public List<DefinitionTerrain> Terrain => terrainList;
    public string Name => this.name;
}
