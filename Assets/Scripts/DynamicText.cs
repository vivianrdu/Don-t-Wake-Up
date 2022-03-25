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
    public Transform player;
    #endregion

    void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        float distFromPlayer = Vector2.Distance(player.position, transform.position);
        Debug.Log(distFromPlayer);
        if (distFromPlayer < 3)
        {
            if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
        && text.text == "move\n(a) or (d)")
            {
                StartCoroutine(FadeTextToZeroAlpha(1f, text));
            }
            else if (Input.GetKeyDown(KeyCode.E) && text.text == "collect\n(e)")
            {
                StartCoroutine(FadeTextToZeroAlpha(1f, text));
            }
            else if (Input.GetKeyDown(KeyCode.T) && text.text == "push or pull\n(t) to lock on/off")
            {
                StartCoroutine(FadeTextToZeroAlpha(1f, text));
            }
        }
    }


    IEnumerator FadeTextToFullAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    IEnumerator FadeTextToZeroAlpha(float t, Text i)
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
