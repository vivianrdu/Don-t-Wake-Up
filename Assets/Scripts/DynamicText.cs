using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DynamicText : MonoBehaviour
{
    #region Text_variables
    private Text text;
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
        if (text == null)
        {
            return;
        }
        else
        {
            float distFromPlayer = Vector2.Distance(player.position, transform.position);
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
                else if (Input.GetKeyDown(KeyCode.T) && text.text == "push or pull\nhold (l)")
                {
                    StartCoroutine(FadeTextToZeroAlpha(1f, text));
                }
                else if ((Input.GetKeyDown(KeyCode.LeftShift) || (Input.GetKeyDown(KeyCode.RightShift)))
                    && text.text == "hold SHIFT to run")
                {
                    StartCoroutine(FadeTextToZeroAlpha(1f, text));
                }
                else if ((Input.GetKeyDown(KeyCode.Space)) && text.text == "press SPACE to jump on crates")
                {
                    StartCoroutine(FadeTextToZeroAlpha(1f, text));
                }
                else if ((Input.GetKeyDown(KeyCode.F)) && text.text == "press (e) to pick up and throw pebble")
                {
                    StartCoroutine(FadeTextToZeroAlpha(1f, text));
                }
                else if ((Input.GetKeyDown(KeyCode.E)) && text.text == "press (e) to pick up key")
                {
                    StartCoroutine(FadeTextToZeroAlpha(1f, text));
                }
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
