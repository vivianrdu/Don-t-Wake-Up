using System.Collections;
using UnityEngine;

public class PlayerSoundHandler : MonoBehaviour
{
    private AudioSource[] playerSounds;

    private AudioSource walking;

    void Start()
    {
        playerSounds = GetComponents<AudioSource>();
        walking = playerSounds[0];
    }

    #region Walking_functions
    public void PlayWalking(bool isWalking)
    {
        if (!isWalking)
        {
            Debug.Log("Play walking sound");
            walking.PlayOneShot(walking.clip);
        }
    }

    public void StopWalking(bool isWalking)
    {
        if (isWalking)
        {
            Debug.Log("Stop walking sound");
            walking.Stop();
        }
    }
    #endregion


    public void FadeSound(AudioSource s1)
    {
        if (s1 == null)
        {
            Debug.Log("No input audio source.");
            return;
        }
        StartCoroutine("FadeSounds");
    }

    private IEnumerator FadeSounds(AudioSource s1)
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
