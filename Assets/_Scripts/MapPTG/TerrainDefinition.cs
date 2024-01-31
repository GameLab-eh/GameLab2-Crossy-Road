using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDefinition : MonoBehaviour
{
    [SerializeField] List<GameObject> prefabs;
    [SerializeField] AnimationCurve frequencyRate;

    private int max;

    private void Awake()
    {
        int rowWidth = LevelManager.Instance.GetChunckWidth();
        max = (rowWidth - Mathf.CeilToInt(rowWidth / 8) * 2) * prefabs.Count;
    }

    public float GetFrequency(float factor)
    {
        factor /= 100f;
        return frequencyRate.Evaluate(factor);
    }

    public List<GameObject> GetPrefabs() { return prefabs; }

    public int GetMax() { return max; }
}
