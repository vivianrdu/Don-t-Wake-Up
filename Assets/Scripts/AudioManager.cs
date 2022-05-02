using System.Collections;
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

    private void Update()
    {
        foreach (Sound sound in sounds)
        {
            //sound.source = gameObject.GetComponent<AudioSource>();

            //sound.source.clip = sound.clip;

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

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        String sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "0.StartMenu")
        {
            if (IsPlaying("PeopleSceneBGMusic"))
            {
                SwapMusic("PeopleSceneBGMusic", "StartMenuBGMusic");
            }
            else
            {
                PlayMusic("StartMenuBGMusic");
            }
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
        else if (sceneName == "1.DarkSceneAnimated")
        {
            if (IsPlaying("DarkSceneBGMusic"))
            {
                StopMusic("DarkSceneBGMusic");
            }
        }
        else if (sceneName == "2.WaterScene")
        {
            if (!IsPlaying("WaterSceneBGMusic"))
            {
                PlayMusic("WaterSceneBGMusic");
            }
        }
        else if (sceneName == "2.WaterSceneAnimated")
        {
            if (IsPlaying("WaterSceneBGMusic"))
            {
                StopMusic("WaterSceneBGMusic");
            }
        }
        else if (sceneName == "3.PeopleScene")
        {
            if (!IsPlaying("PeopleSceneBGMusic"))
            {
                PlayMusic("PeopleSceneBGMusic");
            }
        }
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        s.source.Play();
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

    public void StopMusic(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        StartCoroutine(FadeTrackOut(s.source));
    }

    private IEnumerator FadeTrackOut(AudioSource newSource)
    {
        float timeToFade = 1.25f;
        float timeElapsed = 0;

        newSource.Play();

        while (timeElapsed < timeToFade)
        {
            newSource.volume = Mathf.Lerp(0.3f, 0, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator FadeTrackIn(AudioSource newSource)
    {
        float timeToFade = 1.25f;
        float timeElapsed = 0;

        newSource.Play();

        while (timeElapsed < timeToFade)
        {
            newSource.volume = Mathf.Lerp(0, 0.3f, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
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

        source2.Play();

        while (timeElapsed < timeToFade)
        {
            source1.volume = Mathf.Lerp(0.3f, 0, timeElapsed / timeToFade);
            source2.volume = Mathf.Lerp(0, 0.3f, timeElapsed / timeToFade);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    public bool IsPlaying(string audio)
    {
        Sound s = Array.Find(sounds, sound => sound.name == audio);
        if (s != null && s.source.isPlaying)
        {
            return true;
        }
        else
        {
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
