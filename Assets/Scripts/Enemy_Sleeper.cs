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

    #region Sleeping_variables
    protected CircleCollider2D HeadColl;
    private bool isSleeping;

    #endregion

    #region Attack_variables

    #endregion

    #region Physics_components

    #endregion

    #region Animation_components
    #endregion

    #region Audio_variables
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Startup();
        HeadColl = GetComponent<CircleCollider2D>();
        isMoving = false;
        isSleeping = true;
        anim.SetBool("isSleeping", true);
        anim.SetBool("playerDetected", false);
        HeadColl.enabled = false;
        DEnemyColl.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (player_in_Game == null || playerposition == null)
        {
            Debug.Log("stop all sounds");
            sh.StopChasing();
            return;
        }
        if (isMoving)
        {
            Debug.Log("stop snoring and start chasing");
            DEnemyColl.enabled = true;
            HeadColl.enabled = true;
            sh.StopSnoring();
            sh.PlayChasing();
            Attack();
            Move(DEnemyRB, playerposition);

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

        sh.StopChasing();
        sh.StopSnoring();

        

        transform.position = respawn_anchor;
        //reset
        direction = new Vector2(0, 0);
        DEnemyRB.velocity = direction * attack_speed;

        isMoving = false;
        isSleeping = true;
        anim.SetBool("isSleeping", true);
        anim.SetBool("playerDetected", false);
        DEnemyColl.enabled = false;
        HeadColl.enabled = false;
        isAttacking = false;
        playerposition = null;
        reset_attack();
    }


    #region Move_functions
    public void hunt_player()
    {
        isMoving = true;
    }

    #endregion

    #region Waking_up and Falling_asleep

    IEnumerator Wake_up()
    {
        //input animation code here please
        yield return StartCoroutine(Change_color(Color.black));

        //change number here to fit with waking up

        
        isSleeping = false;

        anim.SetBool("isSleeping", false);
        anim.SetBool("playerDetected", true);
        yield return StartCoroutine(Change_color(Color.white));

        hunt_player();
    }

    IEnumerator Change_color(Color col)
    {
        float totalTransitionTime = 1.5f;
        float elapsedTime = 0;

        while (elapsedTime <= totalTransitionTime)
        {
            if (col == Color.black)
            {
                spriteEnemy.color = Color.Lerp(spriteEnemy.color, Color.black, elapsedTime / totalTransitionTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            else
            {
                spriteEnemy.color = Color.Lerp(spriteEnemy.color, Color.white, elapsedTime / totalTransitionTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }


    }

    public new void SmallReset()
    {
        spriteEnemy.color = currentColor;
        Reset_position();
    }

    #endregion

}
