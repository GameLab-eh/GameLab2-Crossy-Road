using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DefinitionProps", menuName = "ScriptableObjects/DefinitionProps", order = 1)]
public class DefinitionProps : ScriptableObject
{
    [Min(1)] public int size;
    [Range(0, 100), Tooltip("percentage value")] public float spawnFrequency;
    [Min(0), Tooltip("Maximum required presence of props in the scene")] public int max = 0;

    public float Frequency => spawnFrequency;
    public int Max => max;
    public int Size => size;
}
