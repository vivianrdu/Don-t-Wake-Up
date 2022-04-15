using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Water : Enemy
{
    [Header("Variables From Child Class")]
    #region Player_Variables

    #endregion

    #region Movement_variables
    public float patrol_radius;
    public float patrol_stopping_randoness;
    public bool pebble_detected;
    public Transform pebble_position;
    private float currdirection_of_patrol;
    #endregion

    #region Attack_variables

    #endregion

    #region Physics_components

    #endregion

    void Start()
    {
        Startup();

        isAttacking = false;
        anim.SetBool("playerDetected", false);
        respawn_anchor = this.transform.position;

        currdirection_of_patrol = -1;//set initial start of patrol


        patrol_stopping_timer = 0;
    }

    void Update()
    {
        
        patrol_stopping_timer -= Time.deltaTime;
        //Pebble detection takes priority, stops detecting once the pebble hits the bottom of the ocean
        if (pebble_detected && pebble_position.GetComponent<Pebbles>().on_floor == false)
        {
            //Pebble detected, stops detecting player
            anim.SetBool("playerDetected", false);
            Move(DEnemyRB, pebble_position);
        }
        else if (playerposition == null || player_in_Game == null)
        {
            anim.SetBool("playerDetected", false);
            patrol();
        }
        else
        {
            //Player detected and currently not following pebble, starts following player
             anim.SetBool("playerDetected", true);
             Attack();
             Move(DEnemyRB, playerposition);
             Set_patrol_timer();
            
        }
    }

    #region Movement_functions
    public void Set_patrol_timer()
    {
        patrol_stopping_timer = Random.Range(0, 5); 
    }

    public new void patrol()
    {
        int y_dir = 0;
        if (transform.position.y < respawn_anchor.y)
        {
            y_dir = 1;
        }

        if (patrol_stopping_timer <= 0)
        {

            float random_number = Random.Range(0, 1000);
            if (random_number < patrol_stopping_randoness)
            {
                patrol_stopping_timer = Random.Range(0, 5);

                return;
            }

            patrol_orientation();
            direction = new Vector2(currdirection_of_patrol, y_dir);
            DEnemyRB.velocity = direction * walking_speed;
            anim.SetFloat("dirX", direction.x);
            anim.SetBool("playerDetected", false);

            return;
        }
        else if (patrol_stopping_timer > 0)
        {
            direction = new Vector2(0, y_dir);
            DEnemyRB.velocity = direction * walking_speed;

            return;
        }


    }

    private void patrol_orientation()
    {
        float orientation = (transform.position.x - respawn_anchor.x);

        if ((orientation >= patrol_radius))
        {
            currdirection_of_patrol = -1;

        }
        else if (orientation <= (-patrol_radius))
        {

            currdirection_of_patrol = 1;

        }
    }
    #endregion

    #region Death_and_Respawn_variables

    public new void Reset_position()
    {
        transform.position = respawn_anchor;
        //reset
        isAttacking = false;
        pebble_detected = false;
        anim.SetBool("playerDetected", false);
    }
    #endregion
}
