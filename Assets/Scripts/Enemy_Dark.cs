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

    #region Attack_variables
    private bool isAttacking;
    #endregion

    #region Physics_components
    Rigidbody2D DEnemyRB;
    BoxCollider2D DEnemyColl;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        DEnemyRB = GetComponent<Rigidbody2D>();
        DEnemyColl = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        stun = 0;
        isAttacking = false;
        anim.SetBool("playerDetected", false);
        anim.SetBool("Stunned", false);
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
            //not currently stunned
            if (anim.GetBool("Stunned") == false)
            {
                if (isAttacking == false && (
                    (direction.x == 1 && Vector2.Distance(playerposition.position, transform.position) <= 2)|
                    (direction.x == -1 && Vector2.Distance(playerposition.position, transform.position) <= 1)))
                {

                        Debug.Log("Attack");
                        StartCoroutine(Attack_routine());
                }
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
            Debug.Log("Stunned");
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

    IEnumerator Attack_routine()
    {
        isAttacking = true;
        bool hitPlayer = false;
        DEnemyRB.velocity = Vector2.zero;

        anim.SetTrigger("Attacking");

        Debug.Log("started routine");
        yield return new WaitForSeconds(0.1f);
        RaycastHit2D[] hits = Physics2D.BoxCastAll(DEnemyRB.position + direction, Vector2.one, 0f, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.CompareTag("Player"))
            {
                hitPlayer = true;
            }
        }
        if (hitPlayer)
        {
            playerposition.GetComponent<Player>().Die();
        }
        isAttacking = false;
        yield return new WaitForSeconds(2f);
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
