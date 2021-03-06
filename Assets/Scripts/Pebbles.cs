using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pebbles : MonoBehaviour
{
    #region Body_variables

    private Rigidbody2D rB;
    private CapsuleCollider2D cc;
    
    // floor is the bottom of the ocean
    public bool on_floor;
    private bool touch_water;
    #endregion

    #region Physics_variables
    public float throw_velocity;
    private Vector2 respawn_anchor;
    #endregion

    #region playerinteractions variables
    private bool player_touch;
    private bool player_picked_up;
    private Player player;
    private Vector2 distance_to_player;
    #endregion

    void Awake()
    {
        player = FindObjectOfType<Player>();
        rB = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        respawn_anchor = transform.position;
        player_picked_up = false;
        touch_water = false;
        on_floor = false;
        distance_to_player = new Vector2(transform.position.x - player.transform.position.x, transform.position.y - player.transform.position.y);
    }

    void Update()
    {
        dist_play();
        //Debug.Log( "Player touch" +  player_touch);
        if (player_touch)
        {
            // Allow the player to pick up if they have not picked it up already and thrown it into the water
            if (Input.GetKey(KeyCode.E) && !player_picked_up && !touch_water)
            {
                player_picked_up = true;
                StartCoroutine(pick_uproutine());
            }
        }
        if (player_picked_up && Input.GetKey(KeyCode.L))
        {
            player_picked_up = false;
            StartCoroutine(throwing());

        }
    }

    private void dist_play()
    {
        distance_to_player.x = transform.position.x - player.transform.position.x;
        distance_to_player.y = transform.position.y - player.transform.position.y;

        

        if(distance_to_player.x < 1 && distance_to_player.y < 1 && distance_to_player.x > -1 && distance_to_player.y > -1)
        {
            player_touch = true;
        } else
        {
            player_touch = false;
        }
    }

    #region Coroutines

    IEnumerator pick_uproutine()
    {
        Debug.Log("pick up routine started");
        //I set position to freeze so gravity does not affect it.
        rB.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        cc.enabled = false;
        rB.mass = 0; // prevent it from going up and down constantly
        while (player_picked_up)
        {

            transform.position = player.transform.position + player.transform.TransformDirection(new Vector3(0.5f * player.currDirection.x, 0, 0));
            yield return null;
        }
        yield return null;
    }
    IEnumerator throwing()
    {
        cc.enabled = true;
        transform.position = player.transform.position + player.transform.TransformDirection(new Vector3(0.5f * player.currDirection.x, 0, 0));
        rB.mass = 0.1f;
        rB.velocity = new Vector2(player.currDirection.x * throw_velocity, 3);
        yield return new WaitForSeconds(1);
    }

    // The enemy will follow the pebble for 5 seconds after touching the floor
    IEnumerator Pebble_delay()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("At the bottom of the ocean");
        on_floor = true;
        yield return null;
    }

    #endregion
    #region Respawn
    public void Reset_position()
    {
        cc.enabled = true;
        transform.position = respawn_anchor;
        player_picked_up = false;
        touch_water = false;
        on_floor = false;
    }
    #endregion

    #region Collisions and Triggers
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            StartCoroutine(Pebble_delay());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            Debug.Log("Pebble in water");
            touch_water = true;
            rB.mass = 10f;
        }
    }
    #endregion
}
