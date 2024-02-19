using UnityEngine;

public class RailroadSignal : Props
{
    new Transform light;

    private void Start()
    {
        light = gameObject.transform.GetChild(0);
    }
    private void OnEnable()
    {
        DynamicProps.run += active;
    }
    private void OnDisable()
    {
        DynamicProps.run -= active;
    }

    void active(bool value, float z)
    {
        if (z != transform.position.z) return;
        light.gameObject.SetActive(value);
    }
}
