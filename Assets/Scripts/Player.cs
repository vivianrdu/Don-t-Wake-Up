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
    BoxCollider2D playercollider;
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


    public float ground_distance;

    // Awake is called before the first frame update
    void Awake()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
        playercollider = GetComponent<BoxCollider2D>();
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





        //feetContact_ground = (Physics.Raycast(transform.position, Vector3.down, 1f, LayerMask.NameToLayer("ground")));
        contact_check();
        x_input = Input.GetAxisRaw("Horizontal");
        y_input = Input.GetAxisRaw("Vertical");

        if (feetContact || feetContact_water)
        {

            if (movingCrate && feetContact_ground)
            { // if trying to move a crate, does a different set of movements
                CrateMove();

            }
            else
            {

                Debug.Log("Calls move");
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

            move_setup("walking");
            //PlayerRB.velocity = new Vector2(x_input * walking_speed, 0);
            //Vector3.ProjectOnPlane(PlayerRB.velocity, ground_contact_point);
        }

        move_direction();
        //PlayerRB.velocity = new Vector2(x_input * walking_speed, 0);
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



            move_setup("crouching");
        }
        else if (Input.GetKey(KeyCode.LeftShift) | Input.GetKey(KeyCode.RightShift))
        {



            sh.StopWalking();
            sh.StopSwimming();
            sh.StopDragging();


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
            move_setup("running");

        }
        /*
        else if (Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.D))
        {
            move_setup("walking");

            

            PlayerRB.velocity = new Vector2(x_input * running_speed, 0);
        }
        */

        //There is a repeat in the code here I changed it so it makes more sense


        else if (Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.D))
        {




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
            move_setup("walking");
        }
        else
        {
            //PlayerRB.velocity = new Vector2(0, PlayerRB.velocity.y);
            //animator_walking("none");
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

            //Debug.Log("Get swimming sound");

            if (swimming_cond1 || swimming_cond2)
            {
                Debug.Log("Playing swimming sound");
                sh.PlaySwimming();

            }


            move_setup("swimming");

        }

        move_direction();

        if (Mathf.Abs(x_input) == 0)
        {
            sh.StopWalking();
        }
    }

    public void contact_check()
    {
        LayerMask masky = LayerMask.GetMask("Ground", "Crate", "Water", "Enemy");
        Debug.Log("mask" + masky);

        

        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, Vector3.down, 3, masky);
        Debug.Log("ground hits: " + hit2D.collider != null);
        if (hit2D.collider != null && !jumping_routine_ongoing)
        {

            Debug.Log("ground hits: " + hit2D.collider.tag);
            if (hit2D.collider.CompareTag("Ground"))
            {
                feetContact = true;
                feetContact_water = false;
                feetContact_ground = true;

            }
            else if (hit2D.collider.CompareTag("Crate") || hit2D.collider.CompareTag("Enemy"))
            {
                //Debug.Log("feetcontact");
                feetContact = true;
                feetContact_water = false;
                feetContact_ground = false;
            }
            else if (hit2D.collider.CompareTag("Water"))
            {

                feetContact_water = true;
                feetContact_ground = false;
            }
            





            //Debug.Log("raycast check hit2D: " + hit2D.collider.tag);
            //Debug.Log("raycast layerCheck: " + hit2D.collider.gameObject.layer);
        }
        else if(jumping_routine_ongoing && PlayerRB.velocity.y < 0 && hit2D.collider != null) //only when player is falling can feetcontact reactivate
        {
            feetContact = true;
        }
        else
        {
            feetContact = false;
            feetContact_water = false;
            feetContact_ground = false;
        }

        /*else
        {
            Debug.Log("raycast check hit2D: null");
        }
        */
    }


    public void move_setup(string whichisit)
    {
        if (whichisit.Equals("running"))
        {
            spritePlayer.sortingLayerName = "Player";
            animator_walking(whichisit);

            isCrouching = false;
            isHidden = false;
            isRunning = true;

            //PlayerRB.velocity = new Vector2(x_input * running_speed, 0);



            PlayerRB.velocity = new Vector2(x_input * running_speed, PlayerRB.velocity.y);
        }
        else if (whichisit.Equals("crouching"))
        {
            animator_walking(whichisit);
            isCrouching = true;
            isRunning = false;

            if (withinHiding)
            {
                isHidden = true;

                spritePlayer.sortingLayerName = "Player_Hidden";
            }
            else
            {
                isHidden = false;
                spritePlayer.sortingLayerName = "Player";
            }


            //PlayerRB.velocity = new Vector2(x_input * crouching_speed, 0); In case player falls through world but should not happen
            PlayerRB.velocity = new Vector2(x_input * crouching_speed, PlayerRB.velocity.y);

        }
        else if (whichisit.Equals("walking"))
        {
            spritePlayer.sortingLayerName = "Player";
            animator_walking(whichisit);

            isCrouching = false;
            isHidden = false;
            isRunning = false;


            PlayerRB.velocity = new Vector2(x_input * walking_speed, PlayerRB.velocity.y);

        }
        else if (whichisit.Equals("swimming"))
        {

            animator_walking(whichisit);
            isCrouching = false;



        }
    }


    public void move_direction()
    {
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

            PlayerRB.velocity = new Vector2(0, PlayerRB.velocity.y);
            animator_walking("none");
        }

        anim.SetFloat("dirX", currDirection.x);
        anim.SetFloat("dirY", currDirection.y);
    }

    public void animator_walking(string whichisit)
    {


        anim.SetBool("walking", false);
        anim.SetBool("crouching", false);
        anim.SetBool("running", false);
        anim.SetBool("swimming", false);
        //anim.SetBool("jumping", false);

        if (!whichisit.Equals("none"))
        {

            anim.SetBool(whichisit, true);

            /*
            sh.StopWalking();
            sh.StopRunning();
            sh.StopSwimming();
            sh.StopDragging();
            */

            // maybe reactivate later 

        }
    }
    private void jumping()
    {
        StartCoroutine(Jumping_Routine());
    }

    IEnumerator Jumping_Routine()
    {
        jumping_routine_ongoing = true;


        animator_walking("none");

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
        //Debug.Log(feetContact);
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

        feetContact = true;
        /*
        if(collision.gameObject.CompareTag("Crate") || collision.gameObject.CompareTag("Enemy"))

        if(collision.gameObject.CompareTag("Crate") || collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Pebble"))

        {
            //Debug.Log("feetcontact");
            feetContact = true;
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            feetContact = true;
            feetContact_ground = true;
        }
        */
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
        //feetContact = false;
        /*
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
        */
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hideable_Object"))
        {

            withinHiding = true;
            //Debug.Log("within Hiding" + withinHiding);
        }

        /*
        if (collision.gameObject.CompareTag("Water"))
        {
            //Debug.Log("feetcontact in water");
            feetContact_water = true;
        }
        */
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Hideable_Object"))
        {
            withinHiding = false;
        }
        /*
        if (collision.gameObject.CompareTag("Water"))
        {
            //Debug.Log("feetcontact out of water");
            feetContact_water = false;
        }
        */
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        /*
        if (collision.gameObject.CompareTag("Water"))
        {
            //Debug.Log("feetcontact in water");
            feetContact_water = true;
        }
        */
    }
    #endregion

    #region Return Functions
    public Rigidbody2D returnPlayerRB()
    {
        return PlayerRB;
    }
    #endregion
}