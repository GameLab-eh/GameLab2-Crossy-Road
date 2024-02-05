using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    [SerializeField] List<Props> propList;
    [SerializeField] AnimationCurve frequencyRate;

    [SerializeField] List<Props> rarityPropList;

    [SerializeField, Tooltip("true = one single type on terrain")] bool isSingleType;

    private int max;

    private void Awake()
    {
        int rowWidth = LevelManager.Instance.ChunckWidth;
        max = (rowWidth - Mathf.CeilToInt(rowWidth / 8) * 2) * propList.Count;
    }

    public float Frequency(int factor) => frequencyRate.Evaluate(factor / 100f);
    public Props Prop(int index) => propList[index];
    public List<Props> PropList => propList;
    public int Max => max;
    public List<Props> RarityList => rarityPropList;
    public List<Props> AllPropsList => propList.Concat(rarityPropList).ToList();
    public Props AllProp(int index) => propList.Concat(rarityPropList).ToList()[index];
    public bool IsSingleType => isSingleType;
}
