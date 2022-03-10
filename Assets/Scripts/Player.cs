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
    #endregion

    #region Animation_components
    Animator anim;
    #endregion

    #region Physics_components
    Rigidbody2D PlayerRB;
    float jumpHeight = 1;
    #endregion

    #region Other_variables
    private float health;
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
    }

    // Update is called once per frame
    void Update()
    {
        // check if movement keys are being pressed WASD + Shift
        x_input = Input.GetAxisRaw("Horizontal");
        y_input = Input.GetAxisRaw("Vertical");

        Move();

        if (health <= 0)
        {
            Die();
        }
    }

    #region Movement_functions
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
        else if (Input.GetKeyDown(KeyCode.Space) == true)
        {
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
    #endregion

    #region Health_functions
    public void Die()
    {
        GameObject img = GameObject.FindWithTag("Fade");
        StartCoroutine(img.GetComponent<Fade>().FadeToBlack());
        Debug.Log("Done fading");
        Destroy(this.gameObject);
        GameObject gm = GameObject.FindWithTag("GameController");
        Debug.Log("Reloading");
        gm.GetComponent<GameManager>().DarkScene();
    }
    #endregion
}