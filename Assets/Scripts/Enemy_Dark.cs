using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class Enemy_Dark : MonoBehaviour
{

    #region Player_Variables
    public Transform playerposition;
    private Vector2 direction;
    #endregion


    #region Movement_variables
    public float walking_speed;
    [SerializeField]
    Animator anim;
    #endregion

    #region Stun_variables
    private float stun;
    public float stun_length;
    #endregion

    #region Physics_components
    Rigidbody2D DEnemyRB;
    BoxCollider2D DEnemyColl;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        DEnemyRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        stun = 0;
        DEnemyColl = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerposition == null)
        {
            DEnemyRB.velocity = new Vector2(0, 0);
            anim.SetBool("playerDetected", false);
            anim.SetBool("Stunned", false);
            return;
        }
        //detected player in line of sight
        else
        { 
            anim.SetBool("playerDetected", true);
            if (anim.GetBool("Stunned") == false)
            {
                Move();
            }
        }
        
    }


    public void Move()
    {

        if (playerposition.position.x > DEnemyRB.transform.position.x)
        {
            direction = new Vector2(1, 0);
        } else
        {
            direction = new Vector2(-1, 0);
        }
        
        DEnemyRB.velocity = direction * walking_speed;
        anim.SetFloat("dirX", direction.x);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Glowing") && coll.GetComponent<Light2D>().pointLightOuterRadius >= 0.2)
        {
            // check to make sure not already stunned
            if (DEnemyColl.enabled)
            {
                stun = stun_length;
                anim.SetBool("Stunned", true);
                DEnemyRB.velocity = new Vector2(0, 0);
                //the kinematic check is to prevent multiple coroutines
                StartCoroutine(Stun_routine());
            }

        
        }
    }

    IEnumerator Stun_routine()
    {
        Debug.Log("Stun routine started");
        //turn off collider
        DEnemyColl.enabled = !DEnemyColl.enabled;

        while (stun >= 0)
        {
            //Debug.Log("coroutine is happening" + stun);
            stun -= Time.deltaTime;
            yield return null;
        }
        DEnemyColl.enabled = !DEnemyColl.enabled;
        anim.SetBool("Stunned", false);
    }

}
