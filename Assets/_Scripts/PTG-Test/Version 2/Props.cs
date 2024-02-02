using UnityEngine;

public class Props : MonoBehaviour
{
    [SerializeField, Min(1)] int size;
    [SerializeField, Range(0, 100), Tooltip("percentage value")] float spawnFrequency;
    [SerializeField, Min(0), Tooltip("minimum required presence of props in the scene")] int min = 0;

    public float Frequency => spawnFrequency;
    public int Min => min;
    public float Size => size;
}
