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
        player = FindObjectsOfType<Player>()[0].transform;
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
                if (transform.Find("Lock") != null)
                {
                    Transform doorLock = transform.Find("Lock");
                    doorLock.GetComponent<SpriteRenderer>().enabled = false;
                }
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
        GameObject img = GameObject.FindWithTag("Fade");

        /** Player SpriteRenderer disabled (disappears) **/
        player.GetComponent<SpriteRenderer>().enabled = false;

        /** This occurs when screen is black:
         * Player position moves to other door
         * Player SpriteRenderer reenabled (appears)
         * Player faces forward
        **/
        player.position = otherSide.doorCoordinates;
        player.GetComponent<SpriteRenderer>().enabled = true;
        player.GetComponent<Player>().currDirection = Vector2.down;

        /** Fades from black **/
        StartCoroutine(img.GetComponent<Fade>().FadeFromBlack());

        canOpen = true;
        otherSide.canOpen = false;
    }
}
