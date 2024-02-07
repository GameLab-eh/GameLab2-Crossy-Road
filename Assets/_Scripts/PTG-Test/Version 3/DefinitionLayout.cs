using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefinitionLayout", menuName = "ScriptableObjects/DefinitionLayout", order = 1)]
public class DefinitionLayout : ScriptableObject
{
    [SerializeField] List<DefinitionTheme> themeList;

    [SerializeField, Min(0)] int chunkLength;
    [SerializeField, Tooltip("Number of chunks before change"), Min(0)] public int chunkDelay;
    [SerializeField, Min(0)] float obstacleDensity = 2;
    public AnimationCurve difficultyCurve;

    public List<DefinitionTheme> Theme => themeList;
    public string Name => name;
    public float ObstacleDensity => obstacleDensity;
    public AnimationCurve DifficultyCurve => difficultyCurve;
    public int ChunkDelay => chunkDelay;
    public int ChunkLength => chunkLength;
}
