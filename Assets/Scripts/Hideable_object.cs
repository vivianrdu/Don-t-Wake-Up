using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hideable_object : Non_interactable_Items
{
    // Start is called before the first frame update

    

    void Awake()
    {
       
        
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


    #endregion

    #region hiding_function

    private void hideplayer()
    {
        //change sprite here so that boulder is semitransparent and that player object is now on different layer than enemy
    }

    #endregion


}
