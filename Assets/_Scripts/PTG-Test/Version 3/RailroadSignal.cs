using UnityEngine;
using System.Collections;

public class RailroadSignal : Props
{
    [SerializeField] Transform light1;
    [SerializeField] Transform light2;

    private AudioSource audioSource;
    [SerializeField] AudioClip audioClip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
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

        if (value)
        {
            audioSource.enabled = true;
            audioSource.Play();
            StartCoroutine(DisableAfterPlayback());
        }
    }

    IEnumerator DisableAfterPlayback()
    {
        // Wait for the duration of the AudioClip
        yield return new WaitForSeconds(audioSource.clip.length);

        // After playback, disable the AudioSource
        audioSource.enabled = false;
    }
}
