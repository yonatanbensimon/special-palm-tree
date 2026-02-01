using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class InsanityTransition : MonoBehaviour
{
    [Header("Delay before going to Main Menu")]
    public float delaySeconds = 5f;

    [Header("Blackout Fade")]
    public Image fadeImage;
    public float fadeDuation = 1f;

    void Start()
    {
        StartCoroutine(WaitAndFade());
    }
    IEnumerator WaitAndFade()
    {
        yield return new WaitForSeconds(delaySeconds);
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            Color c = fadeImage.color;
            c.a = 0;
            fadeImage.color = c;

            float time = 0;
            while(time < fadeDuation)
            {
                time += Time.deltaTime;
                c.a = Mathf.Clamp01(time/fadeDuation);
                fadeImage.color= c;
                yield return null;
            }
        }
        SceneManager.LoadScene("TitlePageScene");
    }
}
