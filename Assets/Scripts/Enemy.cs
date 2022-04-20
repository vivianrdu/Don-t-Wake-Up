using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Variables From Parent Class")]
    #region Player_Variables
    public Transform playerposition;
    public Player player_in_Game;
    protected Vector2 direction;
    #endregion

    #region Movement_variables
    public float walking_speed;
    public float attack_speed;
    [SerializeField]
    protected Animator anim;
    #endregion

    #region Patrol_variables
    protected float patrol_stopping_timer;
    #endregion

    #region Attack_variables
    protected bool isAttacking;
    #endregion

    #region Physics_components
    protected Rigidbody2D DEnemyRB;
    protected BoxCollider2D DEnemyColl;
    #endregion

    #region respawn_and_health_variables
    public Vector2 respawn_anchor;
    #endregion

    #region Audio_variables
    DarkEnemySoundHandler sh;
    #endregion

    void Start()
    {
        Startup();

    }

    void Update()
    {


    }

    /** Gets RigidBody, collider, animator, respawn anchor **/
    public void Startup()
    {
        DEnemyRB = GetComponent<Rigidbody2D>();
        DEnemyColl = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        respawn_anchor = this.transform.position;
        sh = GameObject.Find("/DarkEnemySoundHandler").GetComponent<DarkEnemySoundHandler>();
    }

    #region Movement_functions
    public void Move(Rigidbody2D MovedObject, Transform TargetObject)
    /**
     * @param MovedObject: Object that is being moved
     * @param TargetObject: The object MovedObject is moving to
    */
    {
        if (TargetObject.position.x > MovedObject.transform.position.x)
        {
            if (MovedObject.name == "WaterEnemy")
            {
                if (TargetObject.position.y > MovedObject.transform.position.y)
                {
                    direction = new Vector2(1, 1);
                }
                else if (TargetObject.position.y < MovedObject.transform.position.y)
                {
                    direction = new Vector2(1, -1);
                }
                else
                {
                    direction = new Vector2(1, 0);
                }
            }
            else
            {
                direction = new Vector2(1, 0);
            }
        }
        else
        {
            if (MovedObject.name == "WaterEnemy")
            {
                if (TargetObject.position.y > MovedObject.transform.position.y)
                {
                    direction = new Vector2(-1, 1);
                }
                else if (TargetObject.position.y < MovedObject.transform.position.y)
                {
                    direction = new Vector2(-1, -1);
                }
                else
                {
                    direction = new Vector2(-1, 0);
                }
            }
            else
            {
                direction = new Vector2(-1, 0);
            }
        }

        MovedObject.velocity = direction * attack_speed;
        anim.SetFloat("dirX", direction.x);
    }

    public void patrol()
    {

    }

    #endregion

    #region Death_and_Respawn_variables

    public void Reset_position()
    {
        //transform.position = respawn_anchor;
        //reset

    }


    public void darkness_Island_reset()
    {
        transform.position = respawn_anchor;

    }

    #endregion

    #region Triggers and Collisions

    #endregion

    #region Routines
    protected IEnumerator Attack_routine()
    {
        isAttacking = true;
        float attackLength = 1f;
        DEnemyRB.velocity = Vector2.zero;


        anim.SetTrigger("Attacking");

        while (attackLength >= 0)
        {
            attackLength -= Time.deltaTime;
            yield return null;
        }

        Vector2 vector_enemy = Vector2.one;
        if (transform.name == "PeopleEnemy")
        {
            vector_enemy = new Vector2(5, 5);
        }

        RaycastHit2D[] hits = Physics2D.BoxCastAll(DEnemyRB.position + direction, vector_enemy, 0f, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.CompareTag("Player"))
            {
                yield return StartCoroutine(playerposition.GetComponent<Player>().Die());
            }
        }

        isAttacking = false;
        anim.SetBool("Attacking", false);
    }

    public void Attack()
    {
        if (transform.name == "PeopleEnemy" && isAttacking == false && (
                    (direction.x == 1 && Vector2.Distance(playerposition.position, transform.position) <= 3) ||
                    (direction.x == -1 && Vector2.Distance(playerposition.position, transform.position) <= 3)))
        {
            Debug.Log("PeopleEnemy Attack");
            StartCoroutine(Attack_routine());
        }
        else if (isAttacking == false && (
                    (direction.x == 1 && Vector2.Distance(playerposition.position, transform.position) <= 2) ||
                    (direction.x == -1 && Vector2.Distance(playerposition.position, transform.position) <= 1.5)))
        {

            Debug.Log("Attack");
            sh.StopBreathing();
            sh.StopChasing();
            sh.StopScreeching();
            sh.StopSnoring();
            StartCoroutine(Attack_routine());
        }
    }
    #endregion  

}
