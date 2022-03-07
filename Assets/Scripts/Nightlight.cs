using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class Nightlight : MonoBehaviour
{
    #region Animation_variables
    private Animator anim;
    #endregion

    #region Physics_components
    BoxCollider2D nightlightTBC;
    public float radius;
    private bool timerIsRunning;
    Light2D fire;
    #endregion

    #region Targeting_variables
    public Transform player;
    public Transform checkpoint;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        nightlightTBC = GetComponent<BoxCollider2D>();
        fire = GetComponentInChildren<Light2D>();
        anim = GetComponent<Animator>();
        timerIsRunning = false;
    }

    void Update()
    {
        float distFromPlayer = Vector2.Distance(player.position, transform.position);

        if (player == null)
        {
            return;
        }
        else
        {
            if ((Input.GetKeyDown(KeyCode.E)) && (distFromPlayer <= radius) && (timerIsRunning == false))
            {
                Debug.Log("Picked up");
                PickedUp();
                StartCoroutine(Countdown());
            }
        }
    }

    private void PickedUp()
    {
        fire.intensity = 1;
        timerIsRunning = true;
    }

    private void Reset()
    {
        StopCoroutine(Countdown());
        StartCoroutine(Countdown());
    }

    private void TurnOff()
    {
        timerIsRunning = false;
    }

    public IEnumerator Countdown()
    {
        float totalTransitionTime = 20f;
        float elapsedTime = 0;

        while (fire.intensity >= 0.1)
        {
            fire.intensity = Mathf.Lerp(fire.intensity, 0, elapsedTime / totalTransitionTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        TurnOff();
        Debug.Log("Turned off");
    }
}
