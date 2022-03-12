using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Movement_variables
    public float walking_speed;
    public float running_speed;
    public float crouching_speed;
    float x_input;
    float y_input;

    // bool to detect whether Player's feet is in contact with a surface
    public bool feetContact;
    // bool to decide how high Player can jump
    public float jumpForce;
    public float jumpHeight;
    #endregion

    #region Animation_components
    Animator anim;
    #endregion

    #region Physics_components
    Rigidbody2D PlayerRB;
    #endregion

    #region HEalth_variables and respawns;
    private float health;
    public Vector2 respawn_anchor;

    #endregion

    #region Other_variables
    public Vector2 currDirection;
    #endregion

    // Awake is called before the first frame update
    void Awake()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        health = 1;
        respawn_anchor = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // check if movement keys are being pressed WASD + Shift
        x_input = Input.GetAxisRaw("Horizontal");
        y_input = Input.GetAxisRaw("Vertical");

        Move();

        // jump
        if (Input.GetKeyDown(KeyCode.Space) && canJump())
        {
            Debug.Log("jumping works");
            PlayerRB.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
        }
        // end jump

        if (health <= 0)
        {
            Die();
        }
    }

    #region Movement_functions
    // jump function
    public bool canJump()
    {
        return feetContact;
    }

    private void Move()
    {
        // if J pressed and WASD pressed, set anim.crouching = true
        if (Input.GetKey(KeyCode.J))
        {
            anim.SetBool("crouching", true);
            anim.SetBool("running", false);
            anim.SetBool("walking", false);

            if (x_input > 0)
            {
                PlayerRB.velocity = Vector2.right * crouching_speed;
                currDirection = Vector2.right;
            }
            else if (x_input < 0)
            {
                PlayerRB.velocity = Vector2.left * crouching_speed;
                currDirection = Vector2.left;
            }
            else
            {
                PlayerRB.velocity = Vector2.zero;
                anim.SetBool("walking", false);
                anim.SetBool("running", false);
                anim.SetBool("jumping", false);
                anim.SetBool("crouching", false);
            }
        }

        // else if shift pressed and WASD pressed, set anim.running = true
        else if (Input.GetKey(KeyCode.LeftShift) | Input.GetKey(KeyCode.RightShift))
        {
            anim.SetBool("running", true);
            anim.SetBool("crouching", false);
            anim.SetBool("walking", false);

            if (x_input > 0)
            {
                anim.SetBool("walking", true);
                PlayerRB.velocity = Vector2.right * running_speed;
                currDirection = Vector2.right;
            }
            else if (x_input < 0)
            {
                anim.SetBool("walking", true);
                PlayerRB.velocity = Vector2.left * running_speed;
                currDirection = Vector2.left;
            }
            else if (y_input > 0)
            {
                anim.SetBool("walking", true);
                PlayerRB.velocity = Vector2.up * running_speed;
                currDirection = Vector2.up;
            }
            else if (y_input < 0)
            {
                anim.SetBool("walking", true);
                PlayerRB.velocity = Vector2.down * running_speed;
                currDirection = Vector2.down;
            }
            else
            {
                PlayerRB.velocity = Vector2.zero;
                anim.SetBool("walking", false);
                anim.SetBool("running", false);
                anim.SetBool("jumping", false);
                anim.SetBool("crouching", false);
            }
        }

        // else if WASD pressed, set anim.walking = true
        else if ((Input.GetKey(KeyCode.LeftShift) == false) | (Input.GetKey(KeyCode.RightShift)) == false)
        {
            anim.SetBool("walking", true);
            anim.SetBool("crouching", false);
            anim.SetBool("running", false);

            if (x_input > 0)
            {
                PlayerRB.velocity = Vector2.right * walking_speed;
                currDirection = Vector2.right;
            }
            else if (x_input < 0)
            {
                PlayerRB.velocity = Vector2.left * walking_speed;
                currDirection = Vector2.left;
            }
            else if (y_input > 0)
            {
                PlayerRB.velocity = Vector2.up * walking_speed;
                currDirection = Vector2.up;
            }
            else if (y_input < 0)
            {
                PlayerRB.velocity = Vector2.down * walking_speed;
                currDirection = Vector2.down;
            }
            else
            {
                PlayerRB.velocity = Vector2.zero;
                anim.SetBool("walking", false);
                anim.SetBool("running", false);
                anim.SetBool("jumping", false);
                anim.SetBool("crouching", false);
            }
        }
        // jumping
        //else if (Input.GetKeyDown(KeyCode.Space) == true)
           
        //{
        //    Debug.Log("isJunping");
        //    jumping();
        //}
        //{
        //    anim.SetBool("jumping", true);
        //    if (x_input > 0)
        //    {
        //        PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, 0);
        //        PlayerRB.AddForce(new Vector2(0, jumpHeight));
        //        currDirection = Vector2.right;
        //    }
        //    else if (x_input < 0)
        //    {
        //        PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, 0);
        //        PlayerRB.AddForce(new Vector2(0, jumpHeight));
        //        currDirection = Vector2.left;
        //    }
        //    else
        //    {
        //        PlayerRB.velocity = Vector2.zero;
        //        anim.SetBool("walking", false);
        //        anim.SetBool("running", false);
        //        anim.SetBool("jumping", false);
        //        anim.SetBool("crouching", false);
        //    }
        //}
        // else anim.walking and anim.running = false
        else
        {
            PlayerRB.velocity = Vector2.zero;
            anim.SetBool("walking", false);
            anim.SetBool("running", false);
            anim.SetBool("jumping", false);
            anim.SetBool("crouching", false);
        }
        anim.SetFloat("dirX", currDirection.x);
        anim.SetFloat("dirY", currDirection.y);
    }


    private void jumping()
    {
        Debug.Log("isJunping function called");
        anim.SetBool("jumping", true);
        if (x_input > 0)
        {
            PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, 0);
            PlayerRB.AddForce(new Vector2(0, jumpHeight));
            currDirection = Vector2.right;
        }
        else if (x_input < 0)
        {
            PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, 0);
            PlayerRB.AddForce(new Vector2(0, jumpHeight));
            currDirection = Vector2.left;
        }
        else
        {
            PlayerRB.velocity = Vector2.zero;
            anim.SetBool("walking", false);
            anim.SetBool("running", false);
            anim.SetBool("jumping", false);
            anim.SetBool("crouching", false);
        }
    }
    #endregion

    #region Health_functions
    public void Die()
    {
        GameObject img = GameObject.FindWithTag("Fade");
        StartCoroutine(img.GetComponent<Fade>().FadeToBlack());
        

        Debug.Log("before reload");
        Reload();
    }

    public void Reload()
    {
        GameObject gm = GameObject.FindWithTag("GameController");
        Debug.Log("Reloading");
        gm.GetComponent<GameManager>().DarkScene();
        //gm.GetComponent<GameManager>().reset_current_scene();
        this.transform.position = respawn_anchor;
    }



    #endregion

    #region Collision and triggers

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("feetcontact");
            feetContact = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            feetContact = false;
        }
    }

    #endregion






}