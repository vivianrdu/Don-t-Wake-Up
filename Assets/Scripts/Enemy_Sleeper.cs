using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Sleeper : Enemy
{


    #region Player_Variables


    #endregion

    #region Movement_variables
    public float patrol_radius;
    public bool isMoving;
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
        Startup();
        isMoving = false;
        isSleeping = true;
        anim.SetBool("isSleeping", true);
        anim.SetBool("playerDetected", false);
        DEnemyColl.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (player_in_Game == null || playerposition == null)
        {
            return;
        }
        if (isMoving)
        {
            Attack();
            move_to_player();
           
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
        direction = new Vector2(0, 0);
        DEnemyRB.velocity = direction * attack_speed;

        isMoving = false;
        isSleeping = true;
        anim.SetBool("isSleeping", true);
        anim.SetBool("playerDetected", false);
        DEnemyColl.enabled = false;
        isAttacking = false;
        playerposition = null;
    }


    #region Move_functions
    public void hunt_player()
    {
        Debug.Log("is sleeper running" + isMoving);
        isMoving = true;
        Debug.Log("is sleeper running2" + isMoving);

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
        yield return new WaitForSeconds(1); //change number here to fit with waking up
        anim.SetBool("playerDetected", true);
        hunt_player();
    }

    #endregion

}
