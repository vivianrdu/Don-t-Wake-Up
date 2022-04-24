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
        if (!breathing.isPlaying)
        {
            breathing.Play(0);
        }
    }
    public void StopBreathing()
    {
        //Debug.Log("dark enemy stop breathing");
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
        //Debug.Log("dark enemy screeching");
        if (!screeching.isPlaying)
        {
            screeching.Play(0);
        }
    }
    public void StopScreeching()
    {
        //Debug.Log("dark enemy stop screeching");
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
        snoring.Stop();
    }
}
