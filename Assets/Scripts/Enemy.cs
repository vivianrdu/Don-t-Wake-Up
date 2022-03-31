using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{


    #region Player_Variables
    public Transform playerposition;
    public Player player_in_Game;
    protected Vector2 direction;
    #endregion

    #region Movement_variables
    public float walking_speed;
    public float attack_speed;
    [SerializeField]
    protected Animator anim;
    #endregion

    #region Patrol_variables
    protected float patrol_stopping_timer;
    #endregion

    #region Attack_variables
    protected bool isAttacking;
    #endregion

    #region Physics_components
    protected Rigidbody2D DEnemyRB;
    protected BoxCollider2D DEnemyColl;
    #endregion

    #region Sound_variables
    public AudioManager audioManager;
    #endregion

    #region respawn_and_health_variables
    public Vector2 respawn_anchor;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        startup_stuff();

    }

    // Update is called once per frame
    void Update()
    {
        
        //detected player in line of sight
        

    }


    public void startup_stuff()
    {
        DEnemyRB = GetComponent<Rigidbody2D>();
        DEnemyColl = GetComponent<BoxCollider2D>();

        respawn_anchor = this.transform.position;
    }

    #region Movement_functions
    public void Move()
    {
        
    }
    
    public void move_to_player()
    {
        if (playerposition.position.x > DEnemyRB.transform.position.x)
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

    public void patrol()
    {
    
    }

    #endregion

    #region Death_and_Respawn_variables

    public void Reset_position()
    {
        //transform.position = respawn_anchor;
        //reset

    }

    #endregion

    #region Triggers and Collisions

    #endregion

    #region Routines
    protected IEnumerator Attack_routine()
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

    public void attack_the_player()
    {
        if (isAttacking == false && (
                    (direction.x == 1 && Vector2.Distance(playerposition.position, transform.position) <= 2) |
                    (direction.x == -1 && Vector2.Distance(playerposition.position, transform.position) <= 1.5)))
        {

            Debug.Log("Attack");
            StartCoroutine(Attack_routine());
        }
    }
    #endregion  

}
