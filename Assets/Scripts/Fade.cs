using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public Image img;

    private void Start()
    {
        img = GetComponent<Image>();
        StartCoroutine(FadeFromBlack());
    }

    public IEnumerator FadeToBlack()
    {
        Debug.Log("Fade to black");
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }
    }

    public IEnumerator FadeFromBlack()
    {
        Debug.Log("Fade from black");
        // loop over 1 second
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }
    }
}
