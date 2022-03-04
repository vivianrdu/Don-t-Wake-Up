using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Dark : MonoBehaviour
{

    #region Movement_variables
    public float walking_speed;
    [SerializeField]
    Animator anim;
    #endregion

    #region Physics_components
    Rigidbody2D PlayerRB;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
