using System.Collections;
using System.Collections.Generic;
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


    #region Stunstuff
    private float stun;
    public float stun_length;
    private bool isinlight;
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
            return;
        }
        //Debug.Log("stun:" + stun);
        if(stun <= 0) 
        {
            Move();
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
    }

    /*
     * //need to figure out here to do this with Collision because right now i think it takes line of sight
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Glowing"))
        {
           
            stun = stun_length;
            DEnemyRB.velocity = new Vector2(0, 0);
            Debug.Log("Is it Kinematic?" + DEnemyRB.isKinematic);
            if (!DEnemyRB.isKinematic)
            {

                //the kinematic check is to prevent multiple coroutines
                StartCoroutine(stun_routine());
            }

        }
    }
    */
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Glowing"))
        {
            isinlight = true;
            stun = stun_length;
            DEnemyRB.velocity = new Vector2(0, 0);


            if (DEnemyColl.enabled)
            {

                //the kinematic check is to prevent multiple coroutines
                StartCoroutine(stun_routine());
            }

        
        }
    }

    
    


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Glowing"))
        {
            isinlight = false;
            stun = stun_length;
        }
    }

    IEnumerator stun_routine()
    {

        DEnemyColl.enabled = !DEnemyColl.enabled;
        //Debug.Log("Is it static2?" + DEnemyRB.gameObject.isStatic);

        while (stun >= 0 && isinlight)
        {
            //Debug.Log("coroutine is happening" + stun);
            stun -= Time.deltaTime;

           

            yield return null;
        }
        DEnemyColl.enabled = !DEnemyColl.enabled;
    }

}
