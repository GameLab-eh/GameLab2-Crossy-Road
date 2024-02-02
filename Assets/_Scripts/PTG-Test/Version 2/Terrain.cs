using System.Collections.Generic;
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
    public List<Props> PropList => propList;
    public int Max => max;
    public List<Props> RarityList => rarityPropList;
}
