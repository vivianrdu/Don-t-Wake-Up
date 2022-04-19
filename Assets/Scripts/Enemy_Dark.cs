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
    private float stun;
    public float stun_length;
    #endregion

    #region Attack_variables

    #endregion

    #region Physics_components

    #endregion

    #region Audio_variables
    public DarkEnemySoundHandler sh;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Startup();

        stun = 0;
        isAttacking = false;
        anim.SetBool("playerDetected", false);
        anim.SetBool("Stunned", false);

        respawn_anchor = this.transform.position;

        currdirection_of_patrol = -1;//set initial start of patrol
        
        
        patrol_stopping_timer = 0;

        sh = GameObject.Find("/DarkEnemySoundHandler").GetComponent<DarkEnemySoundHandler>();

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Vector2.Distance(playerposition.position, transform.position));


        patrol_stopping_timer -= Time.deltaTime;

        if (playerposition == null || player_in_Game == null)
        {
            anim.SetBool("playerDetected", false);
            patrol();

            sh.StopBreathing();
            sh.StopChasing();
            
            return;
        }
        //detected player in line of sight
        else
        {
            if (player_in_Game.isHidden)
            {
                sh.StopChasing();
                sh.PlayBreathing();
                //Debug.Log("player is hidden is called");
                anim.SetBool("playerDetected", false);
                patrol();
            }
            
            //not currently stunned
            else if (anim.GetBool("Stunned") == false)
            {
                sh.StopBreathing();
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
        } else if(patrol_stopping_timer > 0)
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
        sh.StopSnoring();
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
            sh.StopBreathing();
            sh.StopChasing();
            sh.PlayScreeching();
            Debug.Log("Stunned " + stun_length);
            // check to make sure not already stunned
            if (DEnemyColl.enabled)
            {
                Debug.Log("Stunned 2" + stun_length);
                stun = stun_length;
                anim.SetBool("Stunned", true);
                DEnemyRB.velocity = new Vector2(0, 0);
                //the kinematic check is to prevent multiple coroutines
                StartCoroutine(Stun_routine());
            }

        
        }
    }
    #endregion

    #region Routines
    

    IEnumerator Stun_routine()
    {
        Debug.Log("Stun routine started test");
        //turn off collider
        DEnemyColl.enabled = !DEnemyColl.enabled;

        while (stun >= 0)
        {
            Debug.Log("coroutine is happening" + stun);
            stun -= Time.deltaTime;
            yield return null;
        }
        DEnemyColl.enabled = !DEnemyColl.enabled;
        anim.SetBool("Stunned", false);
        sh.StopScreeching();
    }
    #endregion
}
