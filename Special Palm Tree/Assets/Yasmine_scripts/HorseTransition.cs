using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class HorseTransition : MonoBehaviour
{
    public Image HorseImage;
    public Image EvilHorseImage;
    public Image blackOverlay;
    public float fadeDuration = 1f;
    public string gameSceneName = "GameScene";
    public AccessoryManager accessoryManager;
    public Transform redEyes;

    public void PlayTransition()
    {
        HorseImage.gameObject.SetActive(false);
        EvilHorseImage.gameObject.SetActive(true);
        CopyAccessories();
        StartCoroutine(FadeToBlack());
    }
    void CopyAccessories()
    {
        foreach(Transform anchor in HorseImage.transform)
        {
            Transform evilAnchor = EvilHorseImage.transform.Find(anchor.name);
            if (evilAnchor == null)
            {
                continue;
            }
            foreach (Transform accessory in anchor)
            {
                if (!accessory.gameObject.activeSelf)
                {
                    continue;
                }
                GameObject copy = Instantiate(accessory.gameObject, evilAnchor);
                copy.SetActive(true);
            }
        }
    }
    private System.Collections.IEnumerator FadeToBlack()
    {
        float timer = 0f;
        Color overlayColor = blackOverlay.color;
        blackOverlay.gameObject.SetActive(true);
        redEyes.SetParent(blackOverlay.gameObject.transform);
        PersistentGameData.Instance.accessories = accessoryManager.currentAccessories;

        while (timer < fadeDuration)
        {
            timer+=Time.deltaTime;
            overlayColor.a = Mathf.Clamp01(timer/fadeDuration);
            blackOverlay.color = overlayColor;
            yield return null;
        }
        
        SceneManager.LoadScene(gameSceneName);
    }
}
