using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line_of_Sight_Enemy : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            GetComponentInParent<Enemy_Dark>().playerposition = coll.transform;
            Debug.Log("Detected player");
          
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GetComponentInParent<Enemy_Dark>().playerposition = null;
        }
    }

}
