using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefinitionTerrain", menuName = "ScriptableObjects/DefinitionTerrain", order = 1)]
public class DefinitionTerrain : ScriptableObject
{
    [SerializeField] GameObject prefab;
    [SerializeField] List<Props> propList;
    [SerializeField] AnimationCurve frequencyRate;
    [SerializeField] AnimationCurve density;
    [SerializeField, Tooltip("true = one single type on terrain")] bool isSingleType;

    [SerializeField] bool isFull;
    [SerializeField] bool isMove;

    public GameObject Prefab => prefab;
    public bool IsFull => isFull;
    public bool IsMove => isMove;

    public float Frequency(int factor) => frequencyRate.Evaluate(factor / 100f);
    public float Density(int factor) => density.Evaluate(factor / 100f);
    public List<Props> Props => propList;
    public bool IsSingleType => isSingleType;
}
