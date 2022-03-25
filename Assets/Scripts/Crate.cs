using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{

    #region Targeting_variables
    public Transform player;
    public float radius;
    #endregion

    #region Physics_variables
    Rigidbody2D CrateRB;
    public Vector2 CrateDirection;
    #endregion 

    void Awake()
    {
        CrateRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float distFromPlayer = Vector2.Distance(player.position, transform.position);
        if (Input.GetKey(KeyCode.T) && distFromPlayer <= radius)
        {
            Debug.Log("moving crate");
            player.GetComponent<Player>().movingCrate = true;
            CrateRB.velocity = player.GetComponent<Player>().returnPlayerRB().velocity;
        }
        else
        {
            player.GetComponent<Player>().movingCrate = false;
            CrateRB.velocity = Vector2.zero;
        }
    }
}
