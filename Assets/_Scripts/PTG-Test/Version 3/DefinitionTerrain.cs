using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "DefinitionTerrain", menuName = "ScriptableObjects/DefinitionTerrain", order = 1)]
public class DefinitionTerrain : ScriptableObject
{
    public GameObject prefab;
    public List<Props> propList;
    public AnimationCurve frequencyRate;

    [SerializeField] List<Props> rarityPropList;

    [SerializeField, Tooltip("true = one single type on terrain")] bool isSingleType;

    private int max;

    public float Frequency(int factor) => frequencyRate.Evaluate(factor / 100f);
    public List<Props> Props => propList;
    public int Max => max;
    public List<Props> Rarity => rarityPropList;
    public List<Props> AllProps => propList.Concat(rarityPropList).ToList();
    public bool IsSingleType => isSingleType;
}
