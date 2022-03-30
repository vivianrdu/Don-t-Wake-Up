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
        if (playerposition == null || player_in_Game == null)
        {
            anim.SetBool("playerDetected", false);
            patrol();


            return;
        }
        //detected player in line of sight
        else
        {
            anim.SetBool("playerDetected", true); //maybe have to move this for animation
            if (isAttacking == false && (
                (direction.x == 1 && Vector2.Distance(playerposition.position, transform.position) <= 2) |
                (direction.x == -1 && Vector2.Distance(playerposition.position, transform.position) <= 1.5)))
            {

                Debug.Log("Attack");
                StartCoroutine(Attack_routine());
            }
            Move();
        }
    }

    #region Routines
    IEnumerator Attack_routine()
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
        
        RaycastHit2D[] hits = Physics2D.BoxCastAll(DEnemyRB.position + direction, Vector2.one, 0f, Vector2.zero);

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
    #endregion
}
