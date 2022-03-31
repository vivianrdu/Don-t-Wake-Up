using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Large : Enemy
{


    public bool playerHunt;
    public bool should_I_hunt_the_player;


    // Start is called before the first frame update
    void Start()
    {
        Startup();
        isAttacking = false;
        anim.SetBool("playerDetected", false);

        
        playerHunt = false;
        should_I_hunt_the_player = false;

        DEnemyColl.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {



        if (playerposition != null && should_I_hunt_the_player == false)
        {
            Debug.Log("Teststtststs:" + playerposition);
            should_I_hunt_the_player = true;
        }

        if (should_I_hunt_the_player)
        {
            anim.SetBool("playerDetected", true); //maybe have to move this for animation
            StartCoroutine(Detected_routine());
        }

        

        if (playerHunt)
        {
            
            Move();
        }
    }

    public new void Reset_position()
    {
        transform.position = respawn_anchor;
        //reset

        direction = new Vector2(0, 0);
        DEnemyRB.velocity = direction * attack_speed;
        isAttacking = false;
        anim.SetBool("playerDetected", false);
        anim.SetBool("Chasing", false);

        playerHunt = false;

        should_I_hunt_the_player = false;
        DEnemyColl.enabled = false;
        transform.localScale = new Vector3(3, 3, 1);



        Debug.Log(" is playerhunt true?" +playerHunt);
        Debug.Log(" is playerhunt true?" + should_I_hunt_the_player);
        Debug.Log(" is playerposition null? " + playerposition == null);
    }


    #region Movement_functions
    public new void Move()
    {
        

        anim.SetBool("Chasing", true);
        move_to_player();
        
    }

    public new void move_to_player()
    {
        
        
        direction = new Vector2(1, 0);
        //Debug.Log("iscalled");
        DEnemyRB.velocity = direction * attack_speed;
        anim.SetFloat("dirX", direction.x);
    }

    #endregion

    #region Triggers and Collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player") && !isAttacking)
        {
            isAttacking = true;
            StartCoroutine(Attack_routine());
        }
        
    }
    #endregion

    #region Routines
    IEnumerator Detected_routine()
    {
        yield return new WaitForSeconds(1f);
        DEnemyColl.enabled = true;
        /* Increase enemy size */
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(8, 8, 1), Time.deltaTime);

        playerHunt = true;

        yield return null;
    }

    new IEnumerator Attack_routine()
    {
        anim.SetBool("Attacking", true);
        DEnemyRB.velocity = Vector2.zero;
        playerHunt = false;
        should_I_hunt_the_player = false;
        //player_in_Game.enabled = !player_in_Game.enabled;

        Debug.Log("Kill player");
        yield return StartCoroutine(player_in_Game.Die());
        //player_in_Game.enabled = !player_in_Game.enabled;

        anim.SetBool("Attacking", false);

        yield return null;
    }
    #endregion
}
