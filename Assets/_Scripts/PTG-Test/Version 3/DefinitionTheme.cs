using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefinitionTheme", menuName = "ScriptableObjects/DefinitionTheme", order = 1)]
public class DefinitionTheme : ScriptableObject
{
    [SerializeField] List<DefinitionTerrain> terrainList;

    [SerializeField] DefinitionTerrain safeArea;

    [SerializeField] GameObject waterfall;

    [SerializeField] GameObject bird;

    [SerializeField] GameObject player;

    public List<DefinitionTerrain> Terrain => terrainList;
    public string Name => this.name;

    public GameObject Player => player;
    public DefinitionTerrain SafeArea => safeArea;
    public GameObject Waterfall => waterfall;
    public GameObject Bird => bird;
}
