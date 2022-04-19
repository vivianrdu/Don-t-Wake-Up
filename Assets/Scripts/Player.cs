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
    public bool change_in_direction;


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

    private string current_animation;
    #endregion

    #region Physics_components
    Rigidbody2D PlayerRB;
    BoxCollider2D playercollider;
    public float ground_distance;
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
        current_animation = "start";
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

                //Debug.Log("Calls move");
                Move();

                // If the conditions for jumping is fullfilled 
                if (canJump() && Input.GetKeyDown(KeyCode.Space))
                {
                    sh.StopWalking();
                    sh.StopRunning();
                    sh.StopSwimming();

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

        //Debug.Log("moving crate");
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            move_setup("walking");
        }

        move_direction();
    }

    private void Move()
    {

        // if the player is in the water, only shift, a, d keys will respond
        if (feetContact_water)
        {
            if (Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.D))
            {
                if (Input.GetKey(KeyCode.LeftShift) | Input.GetKey(KeyCode.RightShift))
                {
                    PlayerRB.velocity = new Vector2(x_input * running_speed, PlayerRB.velocity.y);

                }
                else
                {
                    PlayerRB.velocity = new Vector2(x_input * walking_speed, PlayerRB.velocity.y);
                }
            }
            else
            {
                PlayerRB.velocity = new Vector2(x_input, PlayerRB.velocity.y);
            }
            move_setup("swimming");
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Debug.Log("Crouching");
            PlayerRB.velocity = new Vector2(x_input * crouching_speed, PlayerRB.velocity.y);
            if (!feetContact_water)
            {
                move_setup("crouching");
            }
        }
        else if (Input.GetKey(KeyCode.LeftShift) | Input.GetKey(KeyCode.RightShift))
        {
            PlayerRB.velocity = new Vector2(x_input * running_speed, PlayerRB.velocity.y);
            if (!feetContact_water)
            {
                move_setup("running");
            }

        }
        else if (Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.D))
        {
            PlayerRB.velocity = new Vector2(x_input * walking_speed, PlayerRB.velocity.y);
            if (!feetContact_water)
            {
                move_setup("walking");
            }
        }
        else
        {
            move_setup("none");
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
        //Debug.Log("mask" + masky);

        //Bounds boxBounds = playercollider.bounds;
        //Vector2 topRight = new Vector2(boxBounds.center.x + boxBounds.extents.x, boxBounds.center.y + boxBounds.extents.y);
        //Debug.Log("topright x: " + topRight.x + "     topright y: " + topRight.y);
        //Vector2 center = new Vector2(transform.position.x + playercollider.offset.x,transform.position.y + playercollider.offset.y);
        Vector2 feet = new Vector2(playercollider.bounds.center.x ,playercollider.bounds.min.y);
        Vector2 feetsize = new Vector2(playercollider.bounds.extents.x * 2, 0.1f);
        //RaycastHit2D hit2D = Physics2D.Raycast(transform.position, Vector3.down, 0.91f, masky);
        RaycastHit2D hit2D = Physics2D.BoxCast(feet, feetsize ,0 ,Vector3.down, 0.1f ,masky);
        Debug.Log("ground hits: " + hit2D.collider != null);
        if (hit2D.collider != null && !jumping_routine_ongoing)
        {

            Debug.Log("ground hits: " + hit2D.collider.tag);
            if (hit2D.collider.CompareTag("Ground"))
            {
                feetContact = true;
                feetContact_water = false;
                feetContact_ground = true;
                //Debug.Log("ground distance: " + hit2D.distance);
            }
            else if (hit2D.collider.CompareTag("Crate") || hit2D.collider.CompareTag("Enemy"))
            {
                //Debug.Log("feetcontact");
                feetContact = true;
                feetContact_water = false;
                feetContact_ground = false;
                //Debug.Log("Crate distance: " + hit2D.distance);
            }
            else if (hit2D.collider.CompareTag("Water"))
            {

                feetContact_water = true;
                feetContact_ground = false;
                //Debug.Log("water distance: " + hit2D.distance);
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
    }


    public void move_setup(string whichisit)
    {
        if (whichisit.Equals("running"))
        {

            if (!current_animation.Equals("running") || change_in_direction)
            {
                change_in_direction = false;
                current_animation = "running";
                spritePlayer.sortingLayerName = "Player";
                animator_walking(whichisit);

                isCrouching = false;
                isHidden = false;
                isRunning = true;
                sh.StopWalking();
                sh.StopSwimming();
                bool running_cond3 = Mathf.Abs(PlayerRB.velocity.x) < Mathf.Abs(x_input * running_speed - x_input);
                bool running_cond4 = Mathf.Abs(PlayerRB.velocity.x) > Mathf.Abs(-x_input * running_speed + x_input);

                

                if ((running_cond3) || (running_cond4))
                {
                    //Debug.Log("Play running sound");
                    sh.PlayRunning();
                }
                
            }

            
        }
        else if (whichisit.Equals("crouching"))
        {
            if(!current_animation.Equals("crouching") || change_in_direction)
            {
                change_in_direction = false;
                current_animation = "crouching";
                animator_walking(whichisit);
                isCrouching = true;
                isRunning = false;
                sh.StopWalking();

                sh.StopRunning();
                sh.StopSwimming();

            }
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
            

        }
        else if (whichisit.Equals("walking"))
        {
            

            if (!current_animation.Equals("walking") || change_in_direction)
            {
                change_in_direction = false;
                current_animation = "walking";
                spritePlayer.sortingLayerName = "Player";
                animator_walking(whichisit);

                isCrouching = false;
                isHidden = false;
                isRunning = false;

                sh.StopRunning();
                sh.StopSwimming();


                bool walking_cond1 = Mathf.Abs(PlayerRB.velocity.x) < Mathf.Abs(x_input * walking_speed - x_input);
                bool walking_cond2 = Mathf.Abs(PlayerRB.velocity.x) > Mathf.Abs(-x_input * walking_speed + x_input);


                if (walking_cond1 || walking_cond2)
                {
                    //Debug.Log("Playing walking sound");
                    sh.PlayWalking();

                }
            }

            

        }
        else if (whichisit.Equals("swimming"))
        {
            if (!current_animation.Equals("swimming") || change_in_direction)
            {
                change_in_direction = false;
                current_animation = "swimming";
                spritePlayer.sortingLayerName = "Player";
                animator_walking(whichisit);
                isCrouching = false;
                sh.StopWalking();
                sh.StopRunning();
                sh.PlaySwimming();
            }
        }

        else if (whichisit.Equals("none"))
        {
            
            if (!current_animation.Equals("none") || change_in_direction)
            {
                change_in_direction = false;
                spritePlayer.sortingLayerName = "Player";
                animator_walking(whichisit);
                sh.StopWalking();
                sh.StopRunning();
                sh.StopSwimming();
            }
        }
    }


    public void move_direction()
    {
        if (x_input > 0)
        {
            if(currDirection != Vector2.right)
            {
                change_in_direction = true;
            }

            currDirection = Vector2.right;
        }
        else if (x_input < 0)
        {
            if (currDirection != Vector2.left)
            {
                change_in_direction = true;
            }
            currDirection = Vector2.left;
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
        current_animation = "jump";
        StartCoroutine(Jumping_Routine());
    }

    IEnumerator Jumping_Routine()
    {
        jumping_routine_ongoing = true;


        animator_walking("none");
        anim.SetBool("jumping", true);
        yield return new WaitForSeconds(0.1f);
        PlayerRB.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);

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

        if (collision.gameObject.CompareTag("respawn_anchor"))
        {
            respawn_anchor = collision.transform.position;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hideable_Object"))
        {

            withinHiding = true;
            //Debug.Log("within Hiding" + withinHiding);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Hideable_Object"))
        {
            withinHiding = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

    }
    #endregion

    #region Return Functions
    public Rigidbody2D returnPlayerRB()
    {
        return PlayerRB;
    }
    #endregion
}