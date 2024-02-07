using UnityEngine;

public class Props : MonoBehaviour
{
    [SerializeField, Min(1)] int size;
    [SerializeField] AnimationCurve frequencyRate;
    [SerializeField] bool isAlone;

    public float Frequency(int factor) => frequencyRate.Evaluate(factor / 100f);
    public int Size => size;
    public bool IsAlone => isAlone; 
}
