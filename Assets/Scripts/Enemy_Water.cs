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
    public bool pebble_Detected;
    public Transform pebbleposition;
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
        anim.SetBool("Stunned", false);

        respawn_anchor = this.transform.position;

        currdirection_of_patrol = -1;//set initial start of patrol


        patrol_stopping_timer = 0;
    }

    void Update()
    {
        
        patrol_stopping_timer -= Time.deltaTime;

        if (playerposition == null || player_in_Game == null)
        {
            anim.SetBool("playerDetected", false);
            patrol();


            return;
        }
        else
        {
            if (pebble_Detected)
            {
                //Debug.Log("player is hidden is called");
                anim.SetBool("playerDetected", false);
                Follow_pebble();
            }
            
            //not currently stunned
            else
            {
                anim.SetBool("playerDetected", true); //maybe have to move this for animation
                Attack();
                Move();
            }
        }
    }

    #region Movement_functions
    public new void Move()
    {

        move_to_player();

        patrol_stopping_timer = Random.Range(0, 5); 
    }

    public new void patrol()
    {
        if (patrol_stopping_timer <= 0)
        {

            float random_number = Random.Range(0, 1000);
            if (random_number < patrol_stopping_randoness)
            {
                patrol_stopping_timer = Random.Range(0, 5);

                return;
            }

            patrol_orientation();
            direction = new Vector2(currdirection_of_patrol, 0);
            DEnemyRB.velocity = direction * walking_speed;
            anim.SetFloat("dirX", direction.x);
            anim.SetBool("playerDetected", false);
            anim.SetBool("Stunned", false);

            return;
        }
        else if (patrol_stopping_timer > 0)
        {
            direction = new Vector2(0, 0);
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

    private void Follow_pebble()
    {
        if (pebbleposition.position.x > DEnemyRB.transform.position.x)
        {
            direction = new Vector2(1, 0);
        }
        else
        {
            direction = new Vector2(-1, 0);
        }
        DEnemyRB.velocity = direction * attack_speed;
        anim.SetFloat("dirX", direction.x);
    }
    #endregion

    #region Death_and_Respawn_variables

    public new void Reset_position()
    {
        transform.position = respawn_anchor;
        //reset
        isAttacking = false;
        anim.SetBool("playerDetected", false);
    }
    #endregion
}
