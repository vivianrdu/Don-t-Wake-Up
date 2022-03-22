using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{


    public Sound[] sounds;


    public AudioMixerGroup musicGroup;
    public AudioMixerGroup sfxGroup;
    public AudioMixerGroup foleyGroup;

    private static bool keepFadingIn;
    private static bool keepFadingOut;

    // Use this for initialization
    void Awake()
    {
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.clip = sound.clip;

            if (sound.group == "Music")
            {
                sound.source.outputAudioMixerGroup = musicGroup;
                sound.source.playOnAwake = true;
                sound.source.loop = true;
            }
            else if (sound.group == "Foley")
            {
                sound.source.loop = true;
                sound.source.outputAudioMixerGroup = foleyGroup;
            }
            else if (sound.group == "SFX")
            {
                sound.source.outputAudioMixerGroup = sfxGroup;
            }
            else
            {
                sound.source.outputAudioMixerGroup = foleyGroup;
            }

        }
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "0.StartMenu")
        {
            PlayMusic("StartMenuBGMusic");
        }
        else if (SceneManager.GetActiveScene().name == "0.Tutorial")
        {

        }
        else if (SceneManager.GetActiveScene().name == "1.DarkScene")
        {

        }
        else if (SceneManager.GetActiveScene().name == "1.DarkSceneAnimated")
        {

        }
        else if (SceneManager.GetActiveScene().name == "2.WaterScene")
        {

        }
        else if (SceneManager.GetActiveScene().name == "2.WaterScene")
        {

        }
        else if (SceneManager.GetActiveScene().name == "2.WaterSceneAnimated")
        {

        }
        else if (SceneManager.GetActiveScene().name == "3.PeopleScene")
        {

        }
        else if (SceneManager.GetActiveScene().name == "3.PeopleSceneAnimated")
        {

        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("No music");
            return;
        }
        s.source.PlayOneShot(s.source.clip);
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("No music");
            return;
        }
        s.source.volume = 0;
        StartCoroutine(FadeTrackIn(s.source));
    }

    private IEnumerator FadeTrackIn(AudioSource newSource)
    {
        float timeToFade = 1.25f;
        float timeElapsed = 0;

        newSource.PlayOneShot(newSource.clip);

        while (timeElapsed < timeToFade)
        {
            newSource.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            Debug.Log(newSource.volume.ToString());
            yield return null;
        }
    }

    public bool IsPlaying(string audio)
    {
        Sound s = Array.Find(sounds, sound => sound.name == audio);
        if (s != null && s.source.isPlaying)
        {
            Debug.Log("playing clip");
            return true;
        }
        else
        {
            Debug.Log("not playing");
            return false;
        }
    }

    public void Stop(string audioClip)
    {
        Sound s = Array.Find(sounds, sound => sound.name == audioClip);
        if (s != null && s.source.isPlaying)
        {
            s.source.Stop();
        }
    }

}
