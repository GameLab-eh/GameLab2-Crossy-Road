using UnityEngine;

public class Props : MonoBehaviour
{
    [SerializeField, Min(1)] int size;
    [SerializeField] AnimationCurve frequencyRate;
    [SerializeField, Range(0, 100), Tooltip("percentage value")] float spawnFrequency;

    public float Frequency(int factor) => frequencyRate.Evaluate(factor / 100f);
    public int Size => size;
}
