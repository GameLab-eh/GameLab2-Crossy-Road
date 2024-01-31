using UnityEngine;

public class Props : MonoBehaviour
{
    [SerializeField, Min(1)] int size;
    [SerializeField, Min(0)] AnimationCurve frequencyRate;
    [SerializeField, Min(0), Tooltip("minimum required presence of props in the scene")] int min = 0;

    public float GetFrequency(float factor)
    {
        factor /= 100f;
        return frequencyRate.Evaluate(factor);
    }

    public float GetSize() { return size; }
}
