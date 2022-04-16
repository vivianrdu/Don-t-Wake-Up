using System.Collections;
using UnityEngine;

public class PlayerSoundHandler : MonoBehaviour
{
    private AudioSource[] playerSounds;

    private AudioSource walking;
    private AudioSource running;
    private AudioSource swimming;
    private AudioSource dying;

    void Start()
    {
        playerSounds = GetComponents<AudioSource>();
        walking = playerSounds[0];
        running = playerSounds[1];
        swimming = playerSounds[2];
        dying = playerSounds[3];
    }

    #region Walking_functions
    public void PlayWalking()
    {
        if (!walking.isPlaying)
        {
            //Debug.Log("Play walking sound");
            walking.Play(0);
        }
    }

    public void StopWalking()
    {

        //Debug.Log("Stop walking sound");
        walking.Stop();
    }
    #endregion

    #region Running_functions
    public void PlayRunning()
    {
        if (!running.isPlaying)
        {
            Debug.Log("Play running sound");
            running.Play(0);
        }
    }

    public void StopRunning()
    {
        if (running.isPlaying)
        {
            Debug.Log("Stop running sound");
            running.Stop();
        }
    }
    #endregion

    #region Swimming_functions
    public void PlaySwimming()
    {
        if (!swimming.isPlaying)
        {
            //Debug.Log("Play swimming sound");
            swimming.Play(0);
        }
    }

    public void StopSwimming()
    {

        //Debug.Log("Stop swimming sound");
        swimming.Stop();
    }
    #endregion

    public void PlayDying()
    {
        if (!dying.isPlaying)
        {
            //Debug.Log("Play dying sound");
            dying.Play(0);
        }
    }

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
