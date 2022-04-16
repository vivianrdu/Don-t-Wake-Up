using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    #region Animation_variables
    private Animator anim;
    #endregion

    #region Targeting_variables
    public Transform player;
    public float radius;
    #endregion

    #region Respawn_variables
    private Vector2 respawn_anchor;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        respawn_anchor = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float distFromPlayer = Vector2.Distance(player.position, transform.position);

        if (player == null)
        {
            return;
        }
        if (distFromPlayer <= radius && Input.GetKeyDown(KeyCode.E)) { //press E to grab the key
            Debug.Log("Picked up key");
            Grab();
        }
    }

    // adds a key to the player and destroys the key object.
    void Grab()
    {
        player.GetComponent<Player>().keys += 1;
        //Destroy(this.gameObject);
        transform.GetComponent<SpriteRenderer>().enabled = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    public void Reset_position()
    {
        transform.position = respawn_anchor;
        transform.GetComponent<SpriteRenderer>().enabled = true;
        //reset

    }
}
