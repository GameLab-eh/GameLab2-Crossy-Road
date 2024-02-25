using UnityEngine;

public class RailroadSignal : Props
{
    [SerializeField] Transform light1;
    [SerializeField] Transform light2;

    private AudioPropManager audioPropManager;

    private void Start()
    {
        audioPropManager = GetComponent<AudioPropManager>();
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
        light1.gameObject.SetActive(value);
        if (light2 != null) light2.gameObject.SetActive(value);

        if (value) audioPropManager.StartAudioEffect();
    }
}
