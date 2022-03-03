using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Movement_variables
    public float walking_speed;
    public float running_speed;
    float x_input;
    float y_input;
    [SerializeField]
    Animator anim;
    #endregion

    #region Physics_components
    Rigidbody2D PlayerRB;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // check if movement keys are being pressed WASD + Shift
        x_input = Input.GetAxisRaw("Horizontal");
        y_input = Input.GetAxisRaw("Vertical");
    }

    #region Movement_functions
    private void Move()
    {
        // if shift pressed and WASD pressed, set anim.running = true
        if (Input.GetKey(KeyCode.LeftShift) | Input.GetKey(KeyCode.RightShift))
        {
            if (x_input > 0)
            {
                PlayerRB.velocity = Vector2.right * running_speed;
            }
            else if (x_input < 0)
            {
                PlayerRB.velocity = Vector2.left * running_speed;
            }
            else if (y_input > 0)
            {
                PlayerRB.velocity = Vector2.up * running_speed;
            }
            else if (y_input < 0)
            {
                PlayerRB.velocity = Vector2.down * running_speed;
            }
        }
        // else if WASD pressed, set anim.walking = true
        else if ((Input.GetKey(KeyCode.LeftShift) == false) | (Input.GetKey(KeyCode.RightShift)) == false)
        {
            if (x_input > 0)
            {
                PlayerRB.velocity = Vector2.right * walking_speed;
            }
            else if (x_input < 0)
            {
                PlayerRB.velocity = Vector2.left * walking_speed;
            }
            else if (y_input > 0)
            {
                PlayerRB.velocity = Vector2.up * walking_speed;
            }
            else if (y_input < 0)
            {
                PlayerRB.velocity = Vector2.down * walking_speed;
            }
        }
        // else anim.walking and anim.running = false
        else
        {
            PlayerRB.velocity = Vector2.zero;
            anim.SetBool("walking", false);
            anim.SetBool("running", false);
        }
    }
    #endregion
}
