using System;
using System.Collections.Generic;
using UnityEngine;
using static AudioPropManager;

[RequireComponent(typeof(AudioManager))]
public class AudioManager : MonoBehaviour
{
    [Header("Soundtrack")]
    [SerializeField] AudioClip _soundtrack;

    [Header("Audio Effects")]
    [SerializeField] List<SoundTrack> _tracks;

    [Header("Audio Set")]
    [SerializeField, Range(0, 1)] float _volumeMaster = 0.5f;
    [SerializeField, Range(0, 1)] float _voluemMusic = 0.5f;
    [SerializeField, Range(0, 1)] float _volumeEffects = 0.5f;

    [Header("Variables")]
    [SerializeField, Range(0, 1)] float _advance = 0.2f;

    AudioSource _music;
    AudioSource _effect;

    public delegate void AudioManagerVolumeChanged(float master, float music, float effects);
    public static event AudioManagerVolumeChanged VolumeChanged;

    public List<AudioSource> allAudioSources = new List<AudioSource>();

    private void Start()
    {
        _music = gameObject.AddComponent<AudioSource>();
        _effect = _music;

        PlaySoundtrack();
        SetVolumeForAll();
    }

    private void Update()
    {
        SetVolumeForAll();
    }

    private void OnEnable()
    {
        //OptionManager.Master += ChangeVolumeMaster;
        //OptionManager.Music += ChangeVolumeMusic;
        //OptionManager.Effects += ChangeVolumeEffects;
        AudioPropManager.Effects += PlayEffect;
    }

    private void OnDisable()
    {
        //OptionManager.Master -= ChangeVolumeMaster;
        //OptionManager.Music -= ChangeVolumeMusic;
        //OptionManager.Effects -= ChangeVolumeEffects;
        AudioPropManager.Effects -= PlayEffect;
    }

    void PlaySoundtrack()
    {
        _music.clip = _soundtrack;
        _music.loop = true;
        _music.volume = _volumeMaster * _voluemMusic;
        _music.Play();
    }

    public void PlayEffect(string index)
    {
        foreach (SoundTrack track in _tracks)
        {
            if (track.trackName == index)
            {
                _effect.PlayOneShot(track.track, _volumeMaster * (_volumeEffects * 4));
                break;
            }
        }
    }
    public void PlayEffect(AudioClip track)
    {
        _effect.PlayOneShot(track, _volumeMaster * (_volumeEffects * 4));
    }

    void ChangeVolumeMaster(float volume)
    {
        _volumeMaster = Mathf.Clamp01(volume);
        UpdateVolume();
        SetVolumeForAll();
    }
    void ChangeVolumeMusic(float volume)
    {
        _voluemMusic = Mathf.Clamp01(volume);
        UpdateVolume();
    }

    void ChangeVolumeEffects(float volume)
    {
        _volumeEffects = Mathf.Clamp01(volume);
        UpdateVolume();
        SetVolumeForAll();
    }

    public float VolumeMaster => _volumeMaster;
    public float VolumeEffects => _volumeEffects;

    void UpdateVolume()
    {
        _music.volume = _volumeMaster * _voluemMusic;
        _effect.volume = _volumeMaster * _volumeEffects;

        VolumeChanged?.Invoke(_volumeMaster, _voluemMusic, _volumeEffects);
    }
    void SetVolumeForAll()
    {
        float newVolume = GameManager.Instance.AudioManager.VolumeEffects * GameManager.Instance.AudioManager.VolumeMaster;

        // Iterate through each AudioSource object in the list and set the desired volume
        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.volume = newVolume;
        }
    }

    public void AddManualAudioSource(AudioSource audioSourceObject)
    {
        if (audioSourceObject != null)
        {
            AudioSource audioSource = audioSourceObject;
            if (audioSource != null)
            {
                allAudioSources.Add(audioSource);
            }
            else
            {
                Debug.LogWarning("The object does not contain an AudioSource component.");
            }
        }
        else
        {
            Debug.LogWarning("Object not found.");
        }
    }
}

[Serializable]
public class SoundTrack
{
    public string trackName;

    public AudioClip track;
}