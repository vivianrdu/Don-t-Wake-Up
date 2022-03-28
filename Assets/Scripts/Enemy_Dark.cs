using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class Enemy_Dark : Enemy
{

    #region Player_Variables
    
    private Vector2 direction;
    
    #endregion

    #region Movement_variables
    
    #endregion

    #region Stun_variables
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
        DEnemyRB = GetComponent<Rigidbody2D>();
        DEnemyColl = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        stun = 0;
        isAttacking = false;
        anim.SetBool("playerDetected", false);
        anim.SetBool("Stunned", false);

        respawn_anchor = this.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (playerposition == null || player_in_Game == null)
        {

            patrol();

            DEnemyRB.velocity = new Vector2(0, 0);
            anim.SetBool("playerDetected", false);
            anim.SetBool("Stunned", false);
            return;
        }
        //detected player in line of sight
        else
        {
            if (player_in_Game.isHidden)
            {
                Debug.Log("player is hidden is called");
                patrol();
            }
            
            //not currently stunned
            else if (anim.GetBool("Stunned") == false)
            {
                anim.SetBool("playerDetected", true); //maybe have to move this for animation
                Debug.Log(Vector2.Distance(playerposition.position, transform.position));
                if (isAttacking == false && (
                    (direction.x == 1 && Vector2.Distance(playerposition.position, transform.position) <= 2)|
                    (direction.x == -1 && Vector2.Distance(playerposition.position, transform.position) <= 1.5)))
                {

                        Debug.Log("Attack");
                        StartCoroutine(Attack_routine());
                }
                Move();
            }
            
        }
        
    }

    #region Movement_functions
    public new void Move()
    {

        if (playerposition.position.x > DEnemyRB.transform.position.x)
        {
            direction = new Vector2(1, 0);
        } else
        {
            direction = new Vector2(-1, 0);
        }
        
        DEnemyRB.velocity = direction * walking_speed;
        anim.SetFloat("dirX", direction.x);
    }

    public new void patrol()
    {
        DEnemyRB.velocity = new Vector2(0, 0);
        anim.SetBool("playerDetected", false);
        anim.SetBool("Stunned", false);
        return;
    }

    #endregion

    #region Death_and_Respawn_variables

    public new void Reset_position()
    {
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
            Debug.Log("Stunned");
            // check to make sure not already stunned
            if (DEnemyColl.enabled)
            {
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
    IEnumerator Attack_routine()
    {
        isAttacking = true;
        float attackLength = 1f;
        DEnemyRB.velocity = Vector2.zero;

        anim.SetTrigger("Attacking");

        while (attackLength >= 0)
        {
            attackLength -= Time.deltaTime;
            yield return null;
        }
        //if (hitPlayer)
        //{
        //Debug.Log("before error");
        //Player player_test = FindObjectOfType<Player>();

        //player_test.Die();
        //Debug.Log("after error");
        //}
        RaycastHit2D[] hits = Physics2D.BoxCastAll(DEnemyRB.position + direction, Vector2.one, 0f, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.CompareTag("Player"))
            {
                yield return StartCoroutine(playerposition.GetComponent<Player>().Die());
            }
        }

        isAttacking = false;
        anim.SetBool("Attacking", false);
    }

    IEnumerator Stun_routine()
    {
        Debug.Log("Stun routine started");
        //turn off collider
        DEnemyColl.enabled = !DEnemyColl.enabled;

        while (stun >= 0)
        {
            //Debug.Log("coroutine is happening" + stun);
            stun -= Time.deltaTime;
            yield return null;
        }
        DEnemyColl.enabled = !DEnemyColl.enabled;
        anim.SetBool("Stunned", false);
    }
    #endregion
}
