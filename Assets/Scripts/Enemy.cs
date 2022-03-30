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
    }

    public void patrol()
    {
    
    }

    #endregion

    #region Death_and_Respawn_variables

    public void Reset_position()
    {
        transform.position = respawn_anchor;
        //reset

    }

    #endregion

    #region Triggers and Collisions

    #endregion



}
