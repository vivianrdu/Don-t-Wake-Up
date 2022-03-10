using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DynamicText : MonoBehaviour
{
    #region Text_variables
    private Text text;
    #endregion

    #region Other_variables
    private bool alreadyRunning;
    #endregion

    #region Targeting_variables
    public Transform nightlight;
    #endregion

    void Start()
    {
        text = GetComponent<Text>();
        alreadyRunning= false;
    }

    void Update()
    {
        if (alreadyRunning == false && text != null)
        {
            if (nightlight.GetComponent<Nightlight>().timerIsRunning)
            {
                alreadyRunning = true;
                StartCoroutine(FadeTextToZeroAlpha(1f, text));
            }
        }
        
    }


    public IEnumerator FadeTextToFullAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    public IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
        Destroy(text);
    }
}
