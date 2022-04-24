using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnoringRadius : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            Debug.Log("dark enemy snores");
            GetComponentInParent<DarkEnemySoundHandler>().PlaySnoring();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("dark enemy stops snoring");
            GetComponentInParent<DarkEnemySoundHandler>().StopSnoring();
        }
    }
}
