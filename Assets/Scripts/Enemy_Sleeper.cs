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
        isSleeping = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(player_in_Game.isRunning)
        {
            hunt_player();
        }
    }


    #region Move_functions
    public void hunt_player()
    {
        wake_up();

        move_to_player();
    }

    #endregion

    #region waking_up and falling asleep

    private void wake_up()
    {
        //input animation code here please


    }

    #endregion

}
