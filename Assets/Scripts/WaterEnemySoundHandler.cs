using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEnemySoundHandler : MonoBehaviour
{
    private AudioSource[] waterEnemySounds;

    private AudioSource chasing;
    
    // Start is called before the first frame update
    void Start()
    {
        waterEnemySounds = GetComponents<AudioSource>();
        chasing = waterEnemySounds[0];
    }

    public void PlayChasing()
    {
        Debug.Log("water enemy chasing");
        if (!chasing.isPlaying)
        {
            StartCoroutine(FadeIn(chasing, 1f));
            //breathing.Play(0);
        }
    }
    public void StopChasing()
    {
        Debug.Log("stop water enemy chasing");
        chasing.Stop();
    }

    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        audioSource.Play();
        audioSource.volume = 0f;
        while (audioSource.volume <= 0.9)
        {
            audioSource.volume += Time.deltaTime / FadeTime;
            yield return null;
        }
    }

    public IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume >= 0.1)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
