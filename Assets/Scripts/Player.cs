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

    // Iris Edit 1
    private AudioManager audioManager;

    // bool to detect whether Player's feet is in contact with a surface
    public bool feetContact;
    public bool feetContact_water;
    public bool isCrouching;
    public bool isRunning;
    
    // bool to detect whether player is moving something currently
    public bool movingCrate;


    // bool to decide how high Player can jump
    //public float jumpForce;
    public float jumpHeight;
    private bool jumping_routine_ongoing;
    #endregion

    #region hide_variables
    public bool isHidden;
    private bool withinHiding;
    #endregion


    #region Animation_components
    Animator anim;
    #endregion

    #region Physics_components
    Rigidbody2D PlayerRB;
    #endregion

    #region Health_variables and respawns;
    public Vector2 respawn_anchor;
    #endregion

    #region Other_variables
    public Vector2 currDirection;
    public int keys = 0; // the number of keys the player has
    #endregion

    // Awake is called before the first frame update
    void Awake()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        respawn_anchor = this.transform.position;

        // Iris Edit 1
        audioManager = FindObjectOfType<AudioManager>();

        isHidden = false; //is done as enemy checks that automatically, otherwise get null error
        isCrouching = false;

    }

    // Update is called once per frame
    void Update()
    {
        // check if movement keys are being pressed WASD + Shift
        x_input = Input.GetAxisRaw("Horizontal");
        y_input = Input.GetAxisRaw("Vertical");


        if (movingCrate) { // if trying to move a crate, does a different set of movements
            CrateMove();
        }
        else
        {
            Move();
            // jump
            if (Input.GetKeyDown(KeyCode.Space) && canJump())
            {
                //PlayerRB.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
                jumping();
            }
            // end jump
        }
    }

    #region Movement_functions
    // jump function
    public bool canJump()
    {
        if (feetContact && !jumping_routine_ongoing && !isCrouching)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void CrateMove()
    {
        Debug.Log("moving crate");
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            anim.SetBool("walking", true);
            anim.SetBool("crouching", false);
            anim.SetBool("running", false);
            anim.SetBool("swimming", false);
            isCrouching = false;
            isHidden = false;
            PlayerRB.velocity = new Vector2(x_input * walking_speed, 0);
        }
        if (x_input > 0)
        {
            currDirection = Vector2.right;
        }
        else if (x_input < 0)
        {
            currDirection = Vector2.left;
        }
        else
        {
            PlayerRB.velocity = Vector2.zero;
            anim.SetBool("crouching", false);
            anim.SetBool("running", false);
            anim.SetBool("swimming", false);
        }
    }

    private void Move()
    {
        if(feetContact)
        {
            if (Input.GetKey(KeyCode.S))
            {
                anim.SetBool("crouching", true);
                anim.SetBool("running", false);
                anim.SetBool("walking", false);
                anim.SetBool("swimming", false);
                isCrouching = true;
                isRunning = false;

                PlayerRB.velocity = new Vector2(x_input * crouching_speed,0);

                if(withinHiding)
                {
                    Debug.Log("hiding called");
                    isHidden = true;
                    Debug.Log(isHidden);
                }else
                {
                    isHidden = false;
                }

            } else if (Input.GetKey(KeyCode.LeftShift) | Input.GetKey(KeyCode.RightShift))
            {
                anim.SetBool("running", true);
                anim.SetBool("crouching", false);
                anim.SetBool("walking", false);
                anim.SetBool("swimming", false);
                isCrouching = false;
                isHidden = false;
                isRunning = true;
                PlayerRB.velocity = new Vector2(x_input * running_speed, 0);
            } else
            {
                anim.SetBool("walking", true);
                anim.SetBool("crouching", false);
                anim.SetBool("running", false);
                anim.SetBool("swimming", false);
                isCrouching = false;
                isHidden = false;
                isRunning = false;
                PlayerRB.velocity = new Vector2(x_input * walking_speed, 0);
            }


            if (x_input > 0)
            {
                currDirection = Vector2.right;
            }
            else if (x_input < 0)
            {
                currDirection = Vector2.left;
            }
            else
            {
                PlayerRB.velocity = Vector2.zero;
                anim.SetBool("walking", false);
                anim.SetBool("crouching", false);
                anim.SetBool("running", false);
                anim.SetBool("swimming", false);
            }
        }
        if (feetContact_water)
        {
            anim.SetBool("walking", false);
            anim.SetBool("crouching", false);
            anim.SetBool("running", false);
            anim.SetBool("swimming", true);
            isCrouching = false;

            PlayerRB.velocity = new Vector2(x_input * walking_speed, 0);

            if (x_input > 0)
            {
                currDirection = Vector2.right;
            }
            else if (x_input < 0)
            {
                currDirection = Vector2.left;
            }
            else
            {
                PlayerRB.velocity = Vector2.zero;
            }
        }
        anim.SetFloat("dirX", currDirection.x);
        anim.SetFloat("dirY", currDirection.y);
    }


    private void jumping()
    {
        StartCoroutine(Jumping_Routine());
    }

    IEnumerator Jumping_Routine()
    {
        jumping_routine_ongoing = true;
        Debug.Log("jumping coroutine starts");
        anim.SetBool("walking", false);
        anim.SetBool("crouching", false);
        anim.SetBool("running", false);
        anim.SetBool("swimming", false);

        anim.SetBool("jumping", true);
        yield return new WaitForSeconds(0.1f);
        PlayerRB.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
        Debug.Log(feetContact);

        //yield return new WaitForSeconds(1); // needs to be done to ensure that feetcontact has been lifted
        feetContact = false;
        while (!feetContact)
        {
            
            yield return null;
        }
        Debug.Log(feetContact);
        anim.SetBool("jumping", false);
        jumping_routine_ongoing = false;
        
    }

    #endregion

    #region Spawn_function
    public void Set_spawn_anchor(Vector2 checkpoint)
    {
        respawn_anchor = checkpoint;
    }
    #endregion

    #region Health_functions
    public IEnumerator Die()
    {
        /** Player SpriteRenderer disabled (disappears) **/
        transform.GetComponent<SpriteRenderer>().enabled = false;

        GameObject img = GameObject.FindWithTag("Fade");
        yield return StartCoroutine(img.GetComponent<Fade>().FadeToBlack());
        yield return new WaitForSeconds(1f);
        Reload();
    }

    public void Reload()
    {
        GameObject gm = GameObject.FindWithTag("GameController");
        GameObject img = GameObject.FindWithTag("Fade");

        /** This occurs when screen is black:
         * Player position is reset
         * Player SpriteRenderer reenabled (appears)
         * Player faces forward
        **/
        gm.GetComponent<GameManager>().Reset_current_scene();
        transform.GetComponent<SpriteRenderer>().enabled = true;
        transform.position = respawn_anchor;
        currDirection = Vector2.down;

        /** Fades from black **/
        StartCoroutine(img.GetComponent<Fade>().FadeFromBlack());
    }
    #endregion

    #region Collision and triggers

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            feetContact = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Crate"))
        {
            Debug.Log("feetcontact");
            feetContact = true;
        }

        if(collision.gameObject.CompareTag("respawn_anchor"))
        {
            respawn_anchor = collision.transform.position;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Crate"))
        {
            Debug.Log("feetcontact gone");
            feetContact = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Hideable_Object"))
        {

            withinHiding = true;
            Debug.Log("within Hiding" + withinHiding);
        }
        if (collision.gameObject.CompareTag("Water"))
        {
            Debug.Log("feetcontact in water");
            feetContact_water = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Hideable_Object"))
        {
            withinHiding = false;
        }
        if (collision.gameObject.CompareTag("Water"))
        {
            Debug.Log("feetcontact out of water");
            feetContact_water = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            Debug.Log("feetcontact in water");
            feetContact_water = true;
        }
    }
    #endregion

    #region Return Functions
    public Rigidbody2D returnPlayerRB() {
        return PlayerRB;
    }
    #endregion
}