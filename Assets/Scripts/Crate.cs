using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{

    #region Targeting_variables
    public Transform player;
    public float radius;
    public Player playerCharacter;
    #endregion

    #region Physics_variables
    public Rigidbody2D CrateRB;
    private BoxCollider2D box;
    public Vector2 CrateDirection;
    private Vector2 respawn_anchor;
    #endregion 

    void Awake()
    {
        CrateRB = GetComponent<Rigidbody2D>();
        playerCharacter = player.GetComponent<Player>();
        respawn_anchor = transform.position;
        box = GetComponent<BoxCollider2D>();
        radius = 1.1f;
    }

    // Update is called once per frame
    void Update()
    {
        

        float distFromPlayer = Vector2.Distance(player.position, transform.position);
        
        if (Input.GetKey(KeyCode.L) && distFromPlayer <= radius)
        {
            //Debug.Log("moving crate");
            playerCharacter.movingCrate = true;

            if (playerCharacter.feetContact_ground)
            {
                //Debug.Log("feetground and crate move");
                //Debug.Log("playerRBvelocity: " + playerCharacter.returnPlayerRB().velocity);
                CrateRB.velocity = new Vector2(playerCharacter.returnPlayerRB().velocity.x, CrateRB.velocity.y);
            }
        }
        else
        {
            playerCharacter.movingCrate = false;
            CrateRB.velocity = new Vector2(0, CrateRB.velocity.y); ;
        }
    }

    public void Reset_position()
    {
        transform.position = respawn_anchor;
        //reset
      
    }

}
