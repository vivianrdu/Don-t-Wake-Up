using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamppost : MonoBehaviour
{
    #region Checkpoint_variables
    private Vector2 location;
    private Collider2D coll;
    #endregion

    #region Targeting_variables
    public Transform player;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        location = transform.position;
        coll = transform.GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            Make_checkpoint();
        }
    }

    private void Make_checkpoint()
    {
        player.GetComponent<Player>().Set_spawn_anchor(location);
    }
}
