using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIFade : MonoBehaviour
{
    public string nextLevel;
    bool fadeIn;
    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void FadeIn()
    {
        StartCoroutine(WaitThenFadeInOverSeconds(1f, 1.5f));
    }

    public void FadeOut()
    {
        StartCoroutine(WaitThenFadeOutOverSeconds(0.5f, 1.5f));
    }

    IEnumerator WaitThenFadeInOverSeconds(float waitTime, float fadeSpeed)
    {
        yield return new WaitForSeconds(waitTime);

        while (image.color.a < 1)
        {
            float newAlpha = image.color.a + Time.deltaTime * fadeSpeed;
            image.color = new Color(image.color.r, image.color.g, image.color.b, newAlpha);
            if (newAlpha >= 1) break;
            yield return null;
        }
        SceneManager.LoadScene(nextLevel);
    }    
    IEnumerator WaitThenFadeOutOverSeconds(float waitTime, float fadeSpeed)
    {
        yield return new WaitForSeconds(waitTime);

        while (image.color.a > 0)
        {
            float newAlpha = image.color.a - Time.deltaTime * fadeSpeed;
            image.color = new Color(image.color.r, image.color.g, image.color.b, newAlpha);
            if (newAlpha <= 0) break;
            yield return null;
        }
    }
}
