using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line_of_Sight_Enemy : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D coll)
    {

        
        if (coll.CompareTag("Player"))
        {
            //right now if enemy sees player once then player cannot hide anymore.
            
            
            GetComponentInParent<Enemy>().playerposition = coll.transform;
            GetComponentInParent<Enemy>().player_in_Game = coll.GetComponent<Player>();
            Debug.Log("Detected player");
            
        }

        if (coll.CompareTag("Pebble"))
        {
            GetComponentInParent<Enemy_Water>().pebble_Detected = true;
            GetComponentInParent<Enemy_Water>().pebbleposition = coll.transform;
            Debug.Log("Detected pebble");

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GetComponentInParent<Enemy>().playerposition = null;
            
        }

        if (collision.CompareTag("Pebble"))
        {
            GetComponentInParent<Enemy_Water>().pebble_Detected = false;
            GetComponentInParent<Enemy_Water>().pebbleposition = null;
        }
    }

}
