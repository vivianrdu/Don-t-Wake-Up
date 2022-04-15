using System.Collections;
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
    private bool feetContact_crate;
    public bool feetContact_ground;
    public bool feetContact_water;
    public bool isCrouching;
    public bool isRunning;

    private Vector3 ground_normalVactor;
    private Vector3 ground_contact_point;
    
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
    SpriteRenderer spritePlayer;
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

    #region Audio_variables
    public PlayerSoundHandler sh;

    #endregion


    // Awake is called before the first frame update
    void Awake()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        respawn_anchor = this.transform.position;
        spritePlayer = GetComponent<SpriteRenderer>();
        feetContact_crate = false;

        isHidden = false; //is done as enemy checks that automatically, otherwise get null error
        isCrouching = false;

        sh = GameObject.Find("/PlayerSoundHandler").GetComponent<PlayerSoundHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        // check if movement keys are being pressed WASD + Shift
        /*
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            x_input = 1;
        }else if(Input.GetAxisRaw("Horizontal") < 0)
        {
            x_input = -1;
        }
        else
        {
            x_input = Input.GetAxisRaw("Horizontal");
        }
        */
        Debug.Log("raycast check ground: " + Physics.Raycast(transform.position, Vector3.down, 1f, LayerMask.NameToLayer("Ground")));
        Debug.Log("raycast check crate: " + Physics.Raycast(transform.position, Vector3.down, 1f, LayerMask.NameToLayer("crate")));
        Debug.Log("raycast check water: " + Physics.Raycast(transform.position, Vector3.down, 1f, LayerMask.NameToLayer("water")));


        //feetContact_ground = (Physics.Raycast(transform.position, Vector3.down, 1f, LayerMask.NameToLayer("ground")));

        x_input = Input.GetAxisRaw("Horizontal");
        y_input = Input.GetAxisRaw("Vertical");

        if (feetContact || feetContact_water)
        {
            //Debug.Log("moving crate?" + movingCrate + " feet ground " + feetContact_ground);
            if (movingCrate && feetContact_ground)
            { // if trying to move a crate, does a different set of movements
                CrateMove();
                //Debug.Log("Velocity: " + PlayerRB.velocity);
            }
            else
            {
                Move();
                
                // jump
                if (Input.GetKeyDown(KeyCode.Space) && canJump())
                {
                    sh.StopWalking();
                    sh.StopRunning();
                    sh.StopSwimming();
                    sh.StopDragging();

                    //PlayerRB.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
                    jumping();
                }
                // end jump
            }
        }
    }

    #region Movement_functions
    // jump function
    public bool canJump()
    {
        if (feetContact && (feetContact_crate || feetContact_ground) && !jumping_routine_ongoing && !isCrouching)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void topofcrate()
    {
        feetContact_crate = true;
    }

    public void exittopofcrate()
    {
        feetContact_crate = false;
    }

    private void CrateMove()
    {
        Debug.Log("Call CrateMove");

        sh.StopWalking();
        sh.StopRunning();
        sh.StopSwimming();

        Debug.Log("moving crate");
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            sh.PlayDragging();

            anim.SetBool("walking", true);
            anim.SetBool("crouching", false);
            anim.SetBool("running", false);
            anim.SetBool("swimming", false);
            isCrouching = false;
            isHidden = false;
            //PlayerRB.velocity = new Vector2(x_input * walking_speed, 0);
            //Vector3.ProjectOnPlane(PlayerRB.velocity, ground_contact_point);
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
        PlayerRB.velocity = new Vector2(x_input * walking_speed, 0);
        //Vector3.ProjectOnPlane(PlayerRB.velocity, ground_contact_point);
        //PlayerRB.velocity = new Vector2(x_input * walking_speed, 0); This one works better for some reason
    }

    private void Move()
    {

        if (Input.GetKey(KeyCode.S))
        {
            sh.StopWalking();
            sh.StopRunning();
            sh.StopSwimming();
            sh.StopDragging();
            
            anim.SetBool("crouching", true);
            anim.SetBool("running", false);
            anim.SetBool("walking", false);
            anim.SetBool("swimming", false);
            isCrouching = true;
            isRunning = false;

            PlayerRB.velocity = new Vector2(x_input * crouching_speed, 0);

            if(withinHiding)
            {
                Debug.Log("hiding called");
                isHidden = true;
                Debug.Log(isHidden);
                spritePlayer.sortingLayerName = "Player_Hidden";
            }
            else
            {
                isHidden = false;
                spritePlayer.sortingLayerName = "Player";
            }

            }
        else if (Input.GetKey(KeyCode.LeftShift) | Input.GetKey(KeyCode.RightShift))
        {
            spritePlayer.sortingLayerName = "Player";
            anim.SetBool("running", true);
            anim.SetBool("crouching", false);
            anim.SetBool("walking", false);
            anim.SetBool("swimming", false);
            isCrouching = false;
            isHidden = false;
            isRunning = true;

            //Debug.Log("Stop walking sound");
            sh.StopWalking();
            sh.StopSwimming();
            sh.StopDragging();

            //bool running_cond1 = Mathf.Abs(PlayerRB.velocity.x) > Mathf.Abs(x_input * walking_speed - x_input);
            //bool running_cond2 = Mathf.Abs(PlayerRB.velocity.x) < Mathf.Abs(-x_input * walking_speed + x_input);
            bool running_cond3 = Mathf.Abs(PlayerRB.velocity.x) < Mathf.Abs(x_input * running_speed - x_input);
            bool running_cond4 = Mathf.Abs(PlayerRB.velocity.x) > Mathf.Abs(-x_input * running_speed + x_input);

            //Debug.Log("Get to running");
            //Debug.Log(PlayerRB.velocity.x);
            //Debug.Log(x_input * walking_speed - x_input);
            //Debug.Log(x_input * running_speed - x_input);
            //Debug.Log((running_cond3) || (running_cond4));

            if ((running_cond3) || (running_cond4))
            {
                //Debug.Log("Play running sound");
                sh.PlayRunning();
            }

            PlayerRB.velocity = new Vector2(x_input * running_speed, 0);
        }
        else if (Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.D))
        {
            spritePlayer.sortingLayerName = "Player";
            anim.SetBool("walking", true);
            anim.SetBool("crouching", false);
            anim.SetBool("running", false);
            anim.SetBool("swimming", false);
            isCrouching = false;
            isHidden = false;
            isRunning = false;

            sh.StopRunning();
            sh.StopSwimming();
            sh.StopDragging();

            //Debug.Log("Get to walking");
            //Debug.Log(PlayerRB.velocity.x < x_input * walking_speed - (x_input));

            bool walking_cond1 = Mathf.Abs(PlayerRB.velocity.x) < Mathf.Abs(x_input * walking_speed - x_input);
            bool walking_cond2 = Mathf.Abs(PlayerRB.velocity.x) > Mathf.Abs(-x_input * walking_speed + x_input);

            if (walking_cond1 || walking_cond2)
            {
                //Debug.Log("Playing walking sound");
                sh.PlayWalking();
            }

            PlayerRB.velocity = new Vector2(x_input * walking_speed, 0);
        }
        else
        {
            PlayerRB.velocity = Vector2.zero;
            anim.SetBool("walking", false);
            anim.SetBool("crouching", false);
            anim.SetBool("running", false);
            anim.SetBool("swimming", false);

            sh.StopWalking();
            sh.StopRunning();
            sh.StopSwimming();
            sh.StopDragging();
        }
        
        if (feetContact_water)
        {
            sh.StopWalking();
            sh.StopRunning();
            sh.StopDragging();

            bool swimming_cond1 = Mathf.Abs(PlayerRB.velocity.x) < Mathf.Abs(x_input * running_speed - x_input);
            bool swimming_cond2 = Mathf.Abs(PlayerRB.velocity.x) > Mathf.Abs(-x_input * running_speed + x_input);

            Debug.Log("Get swimming sound");

            if (swimming_cond1 || swimming_cond2)
            {
                Debug.Log("Playing swimming sound");
                sh.PlaySwimming();
            }

            anim.SetBool("walking", false);
            anim.SetBool("crouching", false);
            anim.SetBool("running", false);
            anim.SetBool("swimming", true);
            isCrouching = false;

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                PlayerRB.velocity = new Vector2(x_input * running_speed, 0);
            } 
            else 
            {
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
            }
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
        }

        anim.SetFloat("dirX", currDirection.x);
        anim.SetFloat("dirY", currDirection.y);

        if (Mathf.Abs(x_input) == 0)
        {
            sh.StopWalking();
            sh.StopRunning();
            sh.StopSwimming();
            sh.StopDragging();
        }
    }


    private void jumping()
    {
        StartCoroutine(Jumping_Routine());
    }

    IEnumerator Jumping_Routine()
    {
        jumping_routine_ongoing = true;
        //Debug.Log("jumping coroutine starts");
        anim.SetBool("walking", false);
        anim.SetBool("crouching", false);
        anim.SetBool("running", false);
        anim.SetBool("swimming", false);

        anim.SetBool("jumping", true);
        yield return new WaitForSeconds(0.1f);
        PlayerRB.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
        //Debug.Log(feetContact);


        //yield return new WaitForSeconds(1); // needs to be done to ensure that feetcontact has been lifted
        feetContact = false;
        while (!feetContact)
        {
            if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            {
                Vector2 tempPlayerVel = PlayerRB.velocity;
                PlayerRB.velocity = new Vector2(x_input * running_speed, tempPlayerVel.y);
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                Vector2 tempPlayerVel = PlayerRB.velocity;
                PlayerRB.velocity = new Vector2(x_input * walking_speed, tempPlayerVel.y);
            }

            if (x_input > 0)
            {
                currDirection = Vector2.right;
            }
            else if (x_input < 0)
            {
                currDirection = Vector2.left;
            }
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

        sh.PlayDying();

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
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Crate") || collision.gameObject.CompareTag("Enemy"))
        {
            //Debug.Log("feetcontact");
            feetContact = true;
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            feetContact = true;
            feetContact_ground = true;
        }

        if (collision.gameObject.CompareTag("respawn_anchor"))
        {
            respawn_anchor = collision.transform.position;
        }
    }

    /*
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ground_contact_point = collision.contacts[0].point;
        }
    }
    */
    private void OnCollisionExit2D(Collision2D collision)
    {
        

        if (collision.gameObject.CompareTag("Ground"))
        {
            feetContact_ground = false;
            feetContact = false;
        }
        if (collision.gameObject.CompareTag("Crate"))
        {
            //Debug.Log("feetcontact gone");

            if (!feetContact_ground)
            {
                feetContact = false;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Hideable_Object"))
        {

            withinHiding = true;
            //Debug.Log("within Hiding" + withinHiding);
        }
        if (collision.gameObject.CompareTag("Water"))
        {
            //Debug.Log("feetcontact in water");
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
            //Debug.Log("feetcontact out of water");
            feetContact_water = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            //Debug.Log("feetcontact in water");
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