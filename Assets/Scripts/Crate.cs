using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{

    #region Targeting_variables
    public Transform player;
    public bool isGrabbed = false;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void OnCollisionEnter2D (Collision2D coll)
    {
        if (Input.GetKeyDown(KeyCode.T) && coll.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        } 
    }
}
