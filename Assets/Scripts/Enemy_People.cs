using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_People : Enemy
{
    [Header("Variables From Child Class")]
    #region Player_Variables

    #endregion

    #region Movement_variables
    public float patrol_radius;
    public float patrol_stopping_randoness;

    private float currdirection_of_patrol;
    #endregion

    #region Attack_variables

    #endregion

    #region Physics_components

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Startup();
        isAttacking = false;
        anim.SetBool("playerDetected", false);

        respawn_anchor = this.transform.position;

        currdirection_of_patrol = -1;//set initial start of patrol


        patrol_stopping_timer = 0;


    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Vector2.Distance(playerposition.position, transform.position));


        patrol_stopping_timer -= Time.deltaTime;

        if (playerposition == null || player_in_Game == null)
        {
            anim.SetBool("playerDetected", false);
            anim.SetBool("Patrolling", true);
            patrol();


            return;
        }
        //detected player in line of sight
        else
        {
            if (player_in_Game.isHidden)
            {
                //Debug.Log("player is hidden is called");
                anim.SetBool("playerDetected", false);
                anim.SetBool("Patrolling", true);
                patrol();
            }

            else
            {
                anim.SetBool("Patrolling", false);
                anim.SetBool("playerDetected", true); //maybe have to move this for animation
                Attack();
                Move(DEnemyRB, playerposition);
                patrol_stopping_timer = Random.Range(0, 5);
            }

        }

    }

    #region Movement_functions
    public new void patrol()
    {
        //Debug.Log("patrol timer" + patrol_stopping_timer);

        if (patrol_stopping_timer <= 0)
        {

            //Debug.Log("Patrol patrol stopping timer below 0");
            float random_number = Random.Range(0, 1000);
            //Debug.Log("random number" + random_number);
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

            return;
        }
        else if (patrol_stopping_timer > 0)
        {
            direction = new Vector2(0, 0);
            DEnemyRB.velocity = direction * walking_speed;
            anim.SetBool("Patrolling", false);
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
        anim.SetBool("playerDetected", false);
    }



    #endregion

    #region Triggers and Collisions
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && player_in_Game.isHidden)
        {
            Physics2D.IgnoreCollision(collision.transform.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }
    #endregion
}
