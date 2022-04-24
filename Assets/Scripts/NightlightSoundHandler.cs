using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightlightSoundHandler : MonoBehaviour
{
    private AudioSource[] nightlightSounds;
    private AudioSource turn_on;
    private AudioSource turn_off;

    void Start()
    {
        nightlightSounds = GetComponents<AudioSource>();
        turn_on = nightlightSounds[0];
        turn_off = nightlightSounds[1];
    }

    public void PlayTurnOn()
    {
        //Debug.Log("dark enemy breathing");
        if (!turn_on.isPlaying)
        {
            turn_on.PlayOneShot(turn_on.clip);
        }
    }
    public void PlayTurnOff()
    {
        if (!turn_off.isPlaying)
        {
            turn_off.PlayOneShot(turn_off.clip);
        }
    }
}
