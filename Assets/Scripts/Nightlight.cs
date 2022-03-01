using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nightlight : MonoBehaviour
{
    #region Animation_variables
    private Animator anim;
    #endregion

    #region Physics_components
    BoxCollider2D nightlightBC;
    public float radius;
    #endregion

    #region Targeting_variables
    public Transform player;
    #endregion


    // variables needed:
    // player
    // Nightlight Animation object

    // if detects player in box collider 2D using Raycast 2D AND player turns on nightlight by pressing "E" AND timer == 60
    // nightlight Animation bool 'on' set to TRUE
    // nightlight Animation nightlight_flickering starts playing
    // nightlight Animation timer starts counting down

    // if nightlight Animation timer <= 0
    // nightlight Animation bool 'on' set to FALSE
    // nightlight Animation nightlight_extinguished starts playing
    // stop timer (set = -1 ?)



    // Start is called before the first frame update
    void Start()
    {
        nightlightBC = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            return;
        }
        else
        {
            float timerDuration = anim.GetFloat("timer");

            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, radius, Vector2.zero);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.CompareTag("Player") & Input.GetKeyDown("E") & timerDuration > 0)
                {
                    GetComponent<Animator>().SetBool("on", true);
                    anim.SetFloat("timer", timerDuration - Time.deltaTime);
                }
                else
                {
                    GetComponent<Animator>().SetBool("on", false);
                    anim.SetFloat("timer", -1);
                }
            }
        }
    }
}
