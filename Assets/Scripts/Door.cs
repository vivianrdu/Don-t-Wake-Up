using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    #region Targeting_variables
    public Transform player;
    public Door otherSide;
    private Vector2 doorCoordinates;
    Rigidbody2D DoorRB;
    bool canOpen = true;
    #endregion

    #region DoorType_variables
    bool keyDoor = false;
    #endregion
    
    void Awake()
    {
        DoorRB = GetComponent<Rigidbody2D>();
        doorCoordinates = transform.position;
    }
    
    void OnTriggerExit2D (Collider2D collision)
    {
        canOpen = true;
    }

    void OnTriggerStay2D (Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && canOpen)
        {
            player.position = otherSide.doorCoordinates;
            canOpen = true;
            otherSide.canOpen = false;
        }
    }
}
