using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeEnemySoundHandler : MonoBehaviour
{
    private AudioSource[] LargeEnemySounds;

    private AudioSource chasing;

    // Start is called before the first frame update
    void Start()
    {
        LargeEnemySounds = GetComponents<AudioSource>();
        chasing = LargeEnemySounds[0];
    }

    public void PlayChasing()
    {
        Debug.Log("Large enemy chasing");
        if (!chasing.isPlaying)
        {
            StartCoroutine(FadeIn(chasing, 1f));
        }
    }
    public void StopChasing()
    {
        Debug.Log("Large enemy chasing");
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
}
