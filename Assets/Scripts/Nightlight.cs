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
    public float radius;
    public bool timerIsRunning;
    private bool alreadyOn;
    Light2D fire;
    #endregion

    #region Targeting_variables
    public Transform player;
    #endregion

    #region respawn_variables
    public Vector2 respawn_anchor;
    #endregion

    #region Audio_variables
    public NightlightSoundHandler sh;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>().transform;
        /** Nightlight itself has a Light2D, this ensures the glow Light2D is the fire **/
        Light2D[] lights = GetComponentsInChildren<Light2D>();
        foreach (Light2D li in lights)
        {
            if (li.gameObject.transform.parent != null)
            {
                fire = li;
            }
        }
        anim = GetComponent<Animator>();
        timerIsRunning = false;
        alreadyOn = false;
        respawn_anchor = transform.position;
        sh = GameObject.Find("/NightlightSoundHandler").GetComponent<NightlightSoundHandler>();
    }

    void Update()
    {
        if (player == null)
        {
            return;
        }
        float distFromPlayer = Vector2.Distance(player.position, transform.position);
        if (timerIsRunning)
        {
            if (player.GetComponent<Player>().currDirection == Vector2.right)
            {
                transform.position = player.position + player.TransformDirection(new Vector3(0.5f, 0, 0));
            }
            else
            {
                transform.position = player.position + player.TransformDirection(new Vector3(-0.5f, 0, 0));
            }
        }
        else
        {
            if ((Input.GetKeyDown(KeyCode.E)) && (distFromPlayer <= radius) && (timerIsRunning == false) && (alreadyOn == false))
            {
                Debug.Log("Picked up");
                PickedUp();
                StartCoroutine(Countdown());
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        if (collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(collision.transform.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        */
    }

    private void PickedUp()
    {
        fire.intensity = 1;
        fire.pointLightOuterRadius = 2;
        timerIsRunning = true;
        alreadyOn = true; //player can't pick it up again
        GetComponent<BoxCollider2D>().enabled = false;
        sh.PlayTurnOn();
    }

    private void TurnOff()
    {

        anim.SetBool("on", false);
        timerIsRunning = false; //stops timer
        GetComponent<BoxCollider2D>().enabled = true; //falls onto the ground
        GetComponent<Rigidbody2D>().isKinematic = false;
        sh.PlayTurnOff();
    }

    public IEnumerator Countdown()
    {
        float totalTransitionTime = 2000f;
        float elapsedTime = 0;
        anim.SetBool("reset", false);// need this to enable animation upon reset again
        anim.SetBool("on", true);

        while (fire.intensity >= 0.1)
        {
            fire.intensity = Mathf.Lerp(fire.intensity, 0, elapsedTime / totalTransitionTime);
            fire.pointLightOuterRadius = Mathf.Lerp(fire.pointLightOuterRadius, 0, elapsedTime / totalTransitionTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        TurnOff();
        Debug.Log("Turned off");
    }



    public void Reset_position()

    {
        StopAllCoroutines();
        GetComponent<Rigidbody2D>().isKinematic = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        transform.position = respawn_anchor;
        //reset
        anim.SetBool("on", false);
        GetComponent<BoxCollider2D>().enabled = true;
        timerIsRunning = false;
        alreadyOn = false;
        anim.SetBool("reset", true);
    }

}
