using System.Collections;
using UnityEngine;

public class AudioEffects : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] AudioClip audioClip;
    [SerializeField, Min(0)] float delay;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;

        if (audioClip != null)
        {
            audioSource.enabled = true;
            StartCoroutine(Playback());
        }
    }

    IEnumerator Playback()
    {
        while (true)
        {
            audioSource.Play();

            yield return new WaitForSeconds(audioSource.clip.length);

            yield return new WaitForSeconds(delay);
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
