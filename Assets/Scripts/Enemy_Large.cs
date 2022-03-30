using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Large : Enemy
{
    

    // Start is called before the first frame update
    void Start()
    {
        DEnemyRB = GetComponent<Rigidbody2D>();
        DEnemyColl = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        isAttacking = false;
        anim.SetBool("playerDetected", false);

        respawn_anchor = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerposition == null)
        {
            anim.SetBool("playerDetected", false);
            return;
        }
        //detected player in line of sight
        else
        {
            anim.SetBool("playerDetected", true); //maybe have to move this for animation
            StartCoroutine(Detected_routine());
        }
    }

    #region Movement_functions
    public new void Move()
    {
        anim.SetBool("Chasing", true);
        move_to_player();
        DEnemyRB.velocity = direction * attack_speed;
        anim.SetFloat("dirX", direction.x);
    }

    #endregion

    #region Triggers and Collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Attack_routine());
        }
    }
    #endregion

    #region Routines
    IEnumerator Detected_routine()
    {
        yield return new WaitForSeconds(1f);
        Move();
    }

    IEnumerator Attack_routine()
    {
        anim.SetBool("Attacking", true);
        DEnemyRB.velocity = Vector2.zero;

        
        playerposition.GetComponent<Player>().enabled = !playerposition.GetComponent<Player>().enabled;

        Debug.Log("Kill player");
        yield return StartCoroutine(playerposition.GetComponent<Player>().Die());
        playerposition.GetComponent<Player>().enabled = !playerposition.GetComponent<Player>().enabled;

        anim.SetBool("Attacking", false);
    }
    #endregion
}
