using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nightlight : MonoBehaviour
{
    #region Animation_variables
    private Animator anim;
    #endregion

    #region Physics_components
    BoxCollider2D nightlightBC;
    public float radius;
    #endregion

    #region Targeting_variables
    public Transform player;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        nightlightBC = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }
        else
        {
            TurnOn();
        }
    }

    public void TurnOn()
    {
        Debug.Log("timer is " + anim.GetFloat("timer").ToString());

        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, radius, Vector2.zero);
        foreach (RaycastHit2D hit in hits)
        {
            if (anim.GetFloat("timer") > 0)
            {
                Debug.Log("Player detected.");
                GetComponent<Animator>().SetBool("on", true);
                anim.SetFloat("timer", anim.GetFloat("timer") - Time.deltaTime);
                Debug.Log("timer at " + anim.GetFloat("timer").ToString());
            }
            else if (anim.GetFloat("timer") <= 0)
            {
                GetComponent<Animator>().SetBool("on", false);
                anim.SetFloat("timer", -1);
                Debug.Log("timer at " + anim.GetFloat("timer").ToString());
            }
        }
    }
}
