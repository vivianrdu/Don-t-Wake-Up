using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class darkness_Island : MonoBehaviour
{

    //private BoxCollider2D first_collider;
    //private BoxCollider2D second_collider;
    public Light2D light_global;
    private float current_intensity;
    // Start is called before the first frame update
    void Awake()
    {

        /* 
         * This was a previous idea
        BoxCollider2D[] collidersObj = gameObject.GetComponentsInChildren<BoxCollider2D>();

        if (collidersObj[0].transform.position.x < collidersObj[1].transform.position.x)
        {
            first_collider = collidersObj[0];
            second_collider = collidersObj[1];
        }
        else
        {
            first_collider = collidersObj[1];
            second_collider = collidersObj[0];
        }
       */

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {

            StopAllCoroutines();
            current_intensity = light_global.intensity;
            
            StartCoroutine(change_global_light(true));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StopAllCoroutines();
            current_intensity = light_global.intensity;
            StartCoroutine(change_global_light(false));
        }
    }


    IEnumerator change_global_light(bool go_dark)
    {
        float totalTransitionTime = 1.5f;
        float elapsedTime = 0;
        Debug.Log("godark");

        while (elapsedTime <= totalTransitionTime)
        {

            if (go_dark)
            {
                light_global.intensity = Mathf.Lerp(current_intensity, 0.2f, elapsedTime / totalTransitionTime);
            }else
            {
                light_global.intensity = Mathf.Lerp(current_intensity, 1, elapsedTime / totalTransitionTime);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

}
