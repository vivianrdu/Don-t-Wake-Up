using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pebbles : MonoBehaviour
{


    #region body_variables

    private Rigidbody2D rB;
    private CapsuleCollider2D cc;
    public float throw_velocity;

    #endregion


    public Enemy enemy_game;

    #region playerinteractions variables
    private bool player_touch;
    private bool player_picked_up;
    private Player player;


    //temp test variables


    #endregion
    // Start is called before the first frame update
    void Awake()
    {
        rB = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        player_picked_up = false;
    }

    // Update is called once per frame
    void Update()
    {

        
        if(player_touch)
        {
            if (Input.GetKey(KeyCode.E) && !player_picked_up)
            {
                player_picked_up = true;
                StartCoroutine(pick_uproutine());
            }
        }

        if(player_picked_up && Input.GetKey(KeyCode.F))
        {
            player_picked_up = false;
            StartCoroutine(throwing());
            
        }
        

    }



    #region routines

    IEnumerator pick_uproutine()
    {
        Debug.Log("pick up routine started");
        //I set position to freeze so gravity does not affect it.
        rB.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;

        rB.mass = 0; // prevent it from going up and down constantly
        while (player_picked_up)
        {
           
            transform.position = player.transform.position + player.transform.TransformDirection(new Vector3(0.5f*player.currDirection.x, 0, 0));
            yield return null;
        }
        yield return null;
    }


    IEnumerator throwing()
    {
        
        transform.position = player.transform.position + player.transform.TransformDirection(new Vector3(0.5f * player.currDirection.x, 0, 0));
        rB.mass = 1;
        rB.velocity = new Vector2(player.currDirection.x * throw_velocity, 3);
        yield return new WaitForSeconds(1);
    }

    #endregion
   

    private void OnCollisionEnter2D(Collision2D collision)
    {

        Debug.Log("collision happens");
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("touching Player");
            player_touch = true;
            if(player == null)
            {
                player = collision.gameObject.GetComponent<Player>();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player_touch = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            enemy_game.Move();
        }
    }
}
