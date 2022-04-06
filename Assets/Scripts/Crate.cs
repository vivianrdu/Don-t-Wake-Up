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
    Rigidbody2D CrateRB;
    public Vector2 CrateDirection;
    #endregion 

    void Awake()
    {
        CrateRB = GetComponent<Rigidbody2D>();
        playerCharacter = player.GetComponent<Player>();
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
                CrateRB.velocity = playerCharacter.returnPlayerRB().velocity;
            }
        }
        else
        {
            playerCharacter.movingCrate = false;
            CrateRB.velocity = Vector2.zero;
        }
    }
}
