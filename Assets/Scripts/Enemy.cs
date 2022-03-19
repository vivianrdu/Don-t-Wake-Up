using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{


    #region Player_Variables
    public Player player_in_Game;
    public Transform playerposition;
    private Vector2 direction;
    #endregion

   

    #region Physics_components
    Rigidbody2D DEnemyRB;
    BoxCollider2D DEnemyColl;
    #endregion



    #region respawn_and_health_variables
    public Vector2 respawn_anchor;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        DEnemyRB = GetComponent<Rigidbody2D>();
        DEnemyColl = GetComponent<BoxCollider2D>();

        respawn_anchor = this.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        
        //detected player in line of sight
        

    }

    #region Movement_functions
    public void Move()
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
