using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    #region Targeting_variables
    public Transform player;
    public Door otherSide;
    Vector2 doorCoordinates;
    Rigidbody2D DoorRB;
    #endregion

    #region Opening_Door_variables
    bool canOpen = true;
    public float openTimer = 1f;
    #endregion
    
    #region DoorType_variables
    public bool isKeyDoor;
    #endregion

    void Awake()
    {
        DoorRB = GetComponent<Rigidbody2D>();
        doorCoordinates = transform.position;
    }
    
    void Update()
    {
        if (!canOpen)
        {
            openTimer -= Time.deltaTime;
            if (openTimer <= 0f)
            {
                canOpen = true;
                openTimer = 1f;
            }
        }
    }

    void OnTriggerStay2D (Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && canOpen)
        {
            if (isKeyDoor && player.GetComponent<Player>().keys > 0)
            {
                EnterDoor();
            }
            else if (!isKeyDoor)
            {
                EnterDoor();
            }
        }
    }

    public void EnterDoor()
    {
        player.position = otherSide.doorCoordinates;
        canOpen = true;
        otherSide.canOpen = false;
    }
}
