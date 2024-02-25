using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class AudioPropManager : MonoBehaviour
{
    [SerializeField] AudioClip track;

    public delegate void AudioProp(AudioClip track);
    public static event AudioProp Effects = null;

    public void StartAudioEffect() => Effects?.Invoke(track);
}
