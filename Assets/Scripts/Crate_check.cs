using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate_check : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D coll)
    {


        if (coll.CompareTag("Player"))
        {
            //right now if enemy sees player once then player cannot hide anymore.


            coll.GetComponent<Player>().topofcrate();

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().exittopofcrate();

        }
    }




}
