using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefinitionLayout", menuName = "ScriptableObjects/DefinitionLayout", order = 1),]
public class DefinitionLayout : ScriptableObject
{
    public List<DefinitionTheme> themeList;
    [Tooltip("number of chunck before change"), Min(0)] public int chunckDelay;
    public int obstacleDensity = 2;
    public AnimationCurve difficultyCurve;

    public List<DefinitionTheme> Theme => themeList;
    public string Name => this.name;
    public int ObstacleDensity => obstacleDensity;
    public AnimationCurve DifficultyCurve => difficultyCurve;
    public int ChunckDelay => chunckDelay;
}
