using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWay_Door : Door //this is the scipt for the EXIT of the one way door; the entrance is the same as regular doors. 
{

    #region Opening_Door_variables
    bool canOpen = false;
    #endregion

    void Update()
    {
        return;
    }

    void OnTriggerStay2D (Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && !canOpen)
        {
            Debug.Log("Can't open this door.");
        }
    }
}
