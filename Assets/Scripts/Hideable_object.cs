using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hideable_object : Non_interactable_Items
{
    // Start is called before the first frame update

    SpriteRenderer spriteRenderer_used;

    void Awake()
    {
        spriteRenderer_used = GetComponent<SpriteRenderer>();
        
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
            if (collision.GetComponent<Player>().isCrouching)
            {
                hideplayer();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }



    #endregion

    #region hiding_function

    private void hideplayer()
    {
        //change sprite here so that boulder is semitransparent and that player object is now on different layer than enemy
        spriteRenderer_used.color = new Color(1f, 1f, 1f, .5f);
    }

    #endregion


}
