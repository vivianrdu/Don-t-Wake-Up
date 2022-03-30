using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hideable_object : Non_interactable_Items
{
    // Start is called before the first frame update

    SpriteRenderer spriteRenderer_used;

    Player player;
    bool collision_true;

    void Awake()
    {
        spriteRenderer_used = GetComponent<SpriteRenderer>();
        collision_true = false;
    }

    // Update is called once per frame
    void Update()
    {

    }


    #region Collisions and Triggers

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {

            player = collision.GetComponent<Player>();
            collision_true = true;

            hideplayer();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision_true = false;
    }



    #endregion

    #region hiding_function

    private void hideplayer()
    {
        //change sprite here so that boulder is semitransparent and that player object is now on different layer than enemy
        StartCoroutine(hideplayer_coroutine());
    }

    IEnumerator hideplayer_coroutine ()
    {
        
        while (collision_true)
        {
            if(player.isCrouching)
            {
                spriteRenderer_used.color = new Color(1f, 1f, 1f, .5f);
            }
            else
            {
                spriteRenderer_used.color = new Color(1f, 1f, 1f, 1f);
            }

            yield return null;
        }

        
    }

    #endregion




}
