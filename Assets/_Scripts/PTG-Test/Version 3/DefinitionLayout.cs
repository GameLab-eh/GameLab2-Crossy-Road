using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefinitionLayout", menuName = "ScriptableObjects/DefinitionLayout", order = 1)]
public class DefinitionLayout : ScriptableObject
{
    public List<DefinitionTheme> themeList;

    [SerializeField, Min(0)] int chunkLenght;
    [SerializeField, Tooltip("number of chunck before change"), Min(0)] public int chunckDelay;
    [SerializeField, Min(0)] float obstacleDensity = 2;
    public AnimationCurve difficultyCurve;

    public List<DefinitionTheme> Theme => themeList;
    public string Name => this.name;
    public float ObstacleDensity => obstacleDensity;
    public AnimationCurve DifficultyCurve => difficultyCurve;
    public int ChunckDelay => chunckDelay;
}
