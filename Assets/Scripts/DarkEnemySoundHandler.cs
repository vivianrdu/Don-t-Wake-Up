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
    private AudioSource large_enemy_chasing;
    // Start is called before the first frame update
    void Start()
    {
        darkEnemySounds = GetComponents<AudioSource>();
        breathing = darkEnemySounds[0];
        chasing = darkEnemySounds[1];
        screeching = darkEnemySounds[2];
        snoring = darkEnemySounds[3];
        large_enemy_chasing = darkEnemySounds[4];
    }

    public void PlayBreathing()
    {
        Debug.Log("dark enemy breathing");
        if (!breathing.isPlaying)
        {
            StartCoroutine(FadeIn(breathing, 1f));
            //breathing.Play(0);
        }
    }
    public void StopBreathing()
    {
        breathing.Stop();
    }

    public void PlayChasing()
    {
        //Debug.Log("dark enemy chasing");
        if (!chasing.isPlaying)
        {
            chasing.Play(0);
        }
    }
    public void StopChasing()
    {
        //Debug.Log("dark enemy stop chasing");
        StartCoroutine(FadeOut(chasing, 1f));
        //chasing.Stop();
    }

    public void PlayScreeching()
    {
        //Debug.Log("dark enemy screeching");
        if (!screeching.isPlaying)
        {
            screeching.Play(0);
        }
    }
    public void StopScreeching()
    {
        //Debug.Log("dark enemy stop screeching");
        StartCoroutine(FadeOut(screeching, 1f));
    }

    public void PlaySnoring()
    {
        StartCoroutine(FadeIn(snoring, 1f));
    }
    public void StopSnoring()
    {
        //Debug.Log("dark enemy stop snoring");
        StartCoroutine(FadeOut(snoring, 1f));
    }

    public void PlayLargeEnemyChasing()
    {
        Debug.Log("large dark enemy chasing");
        if (!large_enemy_chasing.isPlaying)
        {
            large_enemy_chasing.Play(0);
        }
    }
    public void StopLargeEnemyChasing()
    {
        large_enemy_chasing.Stop();
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
