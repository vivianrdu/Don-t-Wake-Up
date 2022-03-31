using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Sleeper : Enemy
{


    #region Player_Variables


    #endregion

    #region Movement_variables
    public float patrol_radius;
    public bool ismoving;
    #endregion

    #region Stun_variables
    private bool isSleeping;
    
    #endregion

    #region Attack_variables

    #endregion

    #region Physics_components

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        startup_stuff();
        ismoving = false;
        isSleeping = true;
        anim.SetBool("isSleeping", true);
        anim.SetBool("playerDetected", false);
        DEnemyColl.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {


        if (ismoving)
        {
            attack_the_player();
            move_to_player();
           
        }
        if (player_in_Game == null || playerposition == null)
        {
            return;
        }

        else
        {
            if (playerposition != null)
            {
                if (player_in_Game.isRunning && isSleeping)
                {
                    Debug.Log("Waking up");
                    StartCoroutine(Wake_up());

                }
            }
        }
        
    }


    public new void Reset_position()
    {
        transform.position = respawn_anchor;
        //reset

        ismoving = false;
        isSleeping = true;
        anim.SetBool("isSleeping", true);
        anim.SetBool("playerDetected", false);
        DEnemyColl.enabled = false;
        isAttacking = false;
    }


    #region Move_functions
    public void hunt_player()
    {
        Debug.Log("is sleeper running" + ismoving);
        ismoving = true;
        Debug.Log("is sleeper running2" + ismoving);

    }

    #endregion

    #region Waking_up and Falling_asleep

     IEnumerator Wake_up()
    {
        //input animation code here please
        DEnemyColl.enabled = true;
        Debug.Log("woke up");
        isSleeping = false;
        anim.SetBool("isSleeping", false);
        yield return new WaitForSeconds(2); //change number here to fit with waking up
        anim.SetBool("playerDetected", true);
        hunt_player();
    }

    #endregion

}
