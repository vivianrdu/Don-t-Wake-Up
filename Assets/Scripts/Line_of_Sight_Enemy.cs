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
            GetComponentInParent<DarkEnemySoundHandler>().PlayChasing();
            Debug.Log("Detected player");
            
        }

        if (coll.CompareTag("Pebble"))
        {
            Debug.Log("Pebble in line of sight");
            GetComponentInParent<Enemy_Water>().pebble_detected = true;
            GetComponentInParent<Enemy_Water>().pebble_position = coll.transform;
            //GetComponentInParent<DarkEnemySoundHandler>().StopChasing();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GetComponentInParent<Enemy>().playerposition = null;
            GetComponentInParent<DarkEnemySoundHandler>().StopChasing();

        }
    }

}
