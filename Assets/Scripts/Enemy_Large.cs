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
            
            Move();
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
    IEnumerator Attack_routine()
    {
        isAttacking = true;
        float attackLength = 1f;
        DEnemyRB.velocity = Vector2.zero;

        anim.SetBool("Attacking", true);

        while (attackLength >= 0)
        {
            attackLength -= Time.deltaTime;
            yield return null;
        }
        
        RaycastHit2D[] hits = Physics2D.BoxCastAll(DEnemyRB.position + direction, Vector2.one, 0f, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            Debug.Log("casting raycasts");
            if (hit.transform.CompareTag("Player"))
            {
                Debug.Log("Kill player");
                yield return StartCoroutine(playerposition.GetComponent<Player>().Die());
            }
        }

        isAttacking = false;
        anim.SetBool("Attacking", false);
    }
    #endregion
}
