using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class Enemy_Dark : Enemy
{
    [Header("Variables From Child Class")]
    #region Player_Variables

    #endregion

    #region Movement_variables
    public float patrol_radius;
    public float patrol_stopping_randoness;

    private float currdirection_of_patrol;
    #endregion

    #region Stun_variables
    protected CircleCollider2D HeadColl;
    private float stun;
    public float stun_length;
    #endregion

    #region Attack_variables

    #endregion

    #region Physics_components

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Startup();

        HeadColl = GetComponent<CircleCollider2D>();
        stun = 0;
        isAttacking = false;
        anim.SetBool("playerDetected", false);
        anim.SetBool("Stunned", false);

        respawn_anchor = this.transform.position;

        currdirection_of_patrol = -1;//set initial start of patrol
        patrol_stopping_timer = 0;
    }

    // Update is called once per frame
    void Update()
    {

        patrol_stopping_timer -= Time.deltaTime;

        if (playerposition == null || player_in_Game == null)
        {
            anim.SetBool("playerDetected", false);
            patrol();

            return;
        }
        //detected player in line of sight
        else
        {
            if (player_in_Game.isHidden)
            {
                sh.StopChasing();
                //Debug.Log("player is hidden is called");
                anim.SetBool("playerDetected", false);
                patrol();
            }
            //not currently stunned
            else if (anim.GetBool("Stunned") == false)
            {
                Debug.Log("this line is called");
                sh.StopBreathing();
                sh.StopScreeching();
                sh.PlayChasing();
                
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

    #endregion

    #region Death_and_Respawn_variables

    public new void Reset_position()
    {
        sh.StopBreathing();
        sh.StopChasing();
        sh.StopScreeching();

        transform.position = respawn_anchor;
        //reset
        stun = 0;
        isAttacking = false;
        anim.SetBool("playerDetected", false);
        anim.SetBool("Stunned", false);
    }



    #endregion

    #region Triggers and Collisions
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Glowing") && coll.GetComponent<Light2D>().pointLightOuterRadius >= 0.2)
        {
            Debug.Log("Stunned " + stun_length);
            // check to make sure not already stunned
            if (DEnemyColl.enabled)
            {
                //sh.StopBreathing();
                //sh.StopChasing();
                //sh.PlayScreeching();
                stun = stun_length;
                anim.SetBool("Stunned", true);
                DEnemyRB.velocity = new Vector2(0, 0);
                //the kinematic check is to prevent multiple coroutines
                StartCoroutine(Stun_routine());
            }


        }
    }

    //private void OnTriggerExit2D(Collider2D coll)
    //{
    //    if (coll.CompareTag("Glowing") && coll.GetComponent<Light2D>().pointLightOuterRadius <= 0.2)
    //    {
    //        sh.StopScreeching();
    //        //if (DEnemyColl.enabled)
    //        //{
    //        //    sh.StopScreeching();
    //        //}
    //    }
    //}
    #endregion

    #region Routines


    IEnumerator Stun_routine()
    {
        Debug.Log("Stun routine");

        if (isAttacking)
        {
            StopCoroutine(Attack_routine());
        }
        //turn off collider
        DEnemyColl.enabled = !DEnemyColl.enabled;
        HeadColl.enabled = !HeadColl.enabled;

        while (stun >= 0)
        {
            sh.StopBreathing();
            sh.StopChasing();
            sh.PlayScreeching();
            stun -= Time.deltaTime;
            yield return null;
        }
        DEnemyColl.enabled = !DEnemyColl.enabled;
        HeadColl.enabled = !HeadColl.enabled;

        sh.StopScreeching();
        anim.SetBool("Stunned", false);
    }
    #endregion
}

