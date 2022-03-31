using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Sleeper : Enemy
{


    #region Player_Variables


    #endregion

    #region Movement_variables
    public float patrol_radius;

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
        
        isSleeping = true;
        anim.SetBool("isSleeping", true);
        anim.SetBool("playerDetected", false);
        DEnemyColl.enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player_in_Game == null)
        {
            return;
        }
        else if (player_in_Game.isRunning && isSleeping)
        {
            Debug.Log("Waking up");
            StartCoroutine(Wake_up());
            
        }
    }


    #region Move_functions
    public void hunt_player()
    {

        move_to_player();
        
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
        hunt_player();
    }

    #endregion

}
