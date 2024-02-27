using UnityEngine;

public class AudioPropManager : MonoBehaviour
{
    [SerializeField] AudioClip track;

    public delegate void AudioProp(AudioClip track);
    public static event AudioProp Effects = null;

    public void StartAudioEffect() => Effects?.Invoke(track);
}
