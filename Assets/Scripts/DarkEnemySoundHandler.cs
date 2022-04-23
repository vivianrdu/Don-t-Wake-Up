using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkEnemySoundHandler : MonoBehaviour
{
    private AudioSource[] darkEnemySounds;

    private AudioSource breathing;
    private AudioSource chasing;
    private AudioSource screeching;
    private AudioSource snoring;
    // Start is called before the first frame update
    void Start()
    {
        darkEnemySounds = GetComponents<AudioSource>();
        breathing = darkEnemySounds[0];
        chasing = darkEnemySounds[1];
        screeching = darkEnemySounds[2];
        snoring = darkEnemySounds[3];
    }

    public void PlayBreathing()
    {
        //Debug.Log("dark enemy breathing");
        StopAllCoroutines();
        if (!breathing.isPlaying)
        {
            StartCoroutine(FadeIn(breathing, 1f));
            //breathing.Play(0);
        }
    }
    public void StopBreathing()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut(breathing, 1f));
    }

    public void PlayChasing()
    {
        //Debug.Log("dark enemy chasing");
        StopAllCoroutines();
        if (!chasing.isPlaying)
        {
            chasing.Play(0);
        }
    }
    public void StopChasing()
    {
        //Debug.Log("dark enemy stop chasing");
        StopAllCoroutines();
        StartCoroutine(FadeOut(chasing, 1f));
        //chasing.Stop();
    }

    public void PlayScreeching()
    {
        //Debug.Log("dark enemy screeching");
        StopAllCoroutines();
        if (!screeching.isPlaying)
        {
            screeching.Play(0);
        }
    }
    public void StopScreeching()
    {
        //Debug.Log("dark enemy stop screeching");
        StopAllCoroutines();
        StartCoroutine(FadeOut(screeching, 1f));
    }

    public void PlaySnoring()
    {
        StopAllCoroutines();
        StartCoroutine(FadeIn(snoring, 1f));
    }
    public void StopSnoring()
    {
        //Debug.Log("dark enemy stop snoring");
        StopAllCoroutines();
        StartCoroutine(FadeOut(snoring, 1f));
    }

    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        audioSource.Play();
        audioSource.volume = 0f;
        while (audioSource.volume < 1)
        {
            audioSource.volume += Time.deltaTime / FadeTime;
            yield return null;
        }
    }

    public IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
