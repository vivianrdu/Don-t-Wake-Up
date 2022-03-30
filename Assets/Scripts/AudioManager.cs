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

    private static AudioManager instance;

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }

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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        String sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "0.StartMenu")
        {
            PlayMusic("StartMenuBGMusic");
        }
        else if (sceneName == "0.Tutorial")
        {
            if (!IsPlaying("StartMenuBGMusic"))
            {
                PlayMusic("StartMenuBGMusic");
            }
        }
        else if (sceneName == "1.DarkScene")
        {
            if (IsPlaying("StartMenuBGMusic"))
            {
                SwapMusic("StartMenuBGMusic", "DarkSceneBGMusic");
            }
            else
            {
                PlayMusic("DarkSceneBGMusic");
            }
        }
    }

    void OnDisable()
    {
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        s.source.PlayOneShot(s.source.clip);
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
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

    public void SwapMusic(string name1, string name2)
    {
        Sound s1 = Array.Find(sounds, sound => sound.name == name1);
        Sound s2 = Array.Find(sounds, sound => sound.name == name2);
        if (s1 == null || s2 == null)
        {
            Debug.Log("No music");
            return;
        }
        s2.source.volume = 0;
        StartCoroutine(SwapTrackFade(s1.source, s2.source));
    }

    private IEnumerator SwapTrackFade(AudioSource source1, AudioSource source2)
    {
        float timeToFade = 1.25f;
        float timeElapsed = 0;

        source2.PlayOneShot(source2.clip);

        while (timeElapsed < timeToFade)
        {
            source1.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
            source2.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);

            timeElapsed += Time.deltaTime;
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
