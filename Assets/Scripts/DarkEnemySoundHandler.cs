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
        Debug.Log("dark enemy breathing");
        if (!breathing.isPlaying)
        {
            breathing.Play(0);
        }
    }
    public void StopBreathing()
    {
        Debug.Log("dark enemy stop breathing");
        //FadeSoundOut(breathing);
        breathing.Stop();
    }

    public void PlayChasing()
    {
        Debug.Log("dark enemy chasing");
        if (!chasing.isPlaying)
        {
            chasing.Play(0);
        }
    }
    public void StopChasing()
    {
        Debug.Log("dark enemy stop chasing");
        chasing.Stop();
    }

    public void PlayScreeching()
    {
        Debug.Log("dark enemy screeching");
        if (!screeching.isPlaying)
        {
            screeching.Play(0);
        }
    }
    public void StopScreeching()
    {
        Debug.Log("dark enemy stop screeching");
        screeching.Stop();
    }

    public void PlaySnoring()
    {
        Debug.Log("dark enemy snoring");
        if (!snoring.isPlaying)
        {
            snoring.Play(0);
        }
    }
    public void StopSnoring()
    {
        Debug.Log("dark enemy stop snoring");
        FadeSoundOut(snoring);
    }

    public void FadeSoundOut(AudioSource s1)
    {
        if (s1 == null)
        {
            Debug.Log("No input audio source.");
            return;
        }
        IEnumerator coroutine = FadeSoundsOut(s1);
        StartCoroutine(coroutine);
    }

    private IEnumerator FadeSoundsOut(AudioSource s1)
    {
        float timeToFade = 0.75f;
        float timeElapsed = 0;

        while (timeElapsed < timeToFade)
        {
            s1.volume = Mathf.Lerp(0.5f, 0, timeElapsed / timeToFade);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}
