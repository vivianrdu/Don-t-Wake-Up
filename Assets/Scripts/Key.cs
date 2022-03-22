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

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

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
        Destroy(this.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }
}
