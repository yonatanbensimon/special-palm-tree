using System.Collections.Generic;
using UnityEngine;

public class AccessoryManager : MonoBehaviour
{
    [Header("Headwear")]
    public GameObject hat;
    public GameObject unicorn;

    [Header("BodywearUpper")]
    public GameObject chain;
    public GameObject ribbonBlack;
    [Header("BodywearLower")]
    public GameObject tutu;
    public Dictionary<string, string> currentAccessories = new Dictionary<string, string>();

    [SerializeField] AudioClip clickClip;

    public void ToggleRibbonBlack()
    {
        bool isActive = ribbonBlack.activeSelf;
        DisableUpperBodywear();
        ribbonBlack.SetActive(!isActive);
        currentAccessories["BodywearUpper"] = !isActive ? "RibbonImage" : "None";
        AudioSource.PlayClipAtPoint(clickClip, transform.position);
    }

    void Awake()
    {
        currentAccessories["Headware"] = "None";
        currentAccessories["BodywearUpper"] = "None";
        currentAccessories["BodywearLower"] = "None";
    }

    public void ToggleUnicorn()
    {
        bool isActive = unicorn.activeSelf;
        DisableAllHeadware();
        unicorn.SetActive(!isActive);
        currentAccessories["Headwear"] = !isActive ? "UnicornImage" : "None";
        AudioSource.PlayClipAtPoint(clickClip, transform.position);
    }

    public void ToggleHat()
    {
        bool isActive = hat.activeSelf;
       DisableAllHeadware();
       hat.SetActive(!isActive);
       currentAccessories["Headwear"] = !isActive ? "HatImage" : "None";
        AudioSource.PlayClipAtPoint(clickClip, transform.position);
    }
    public void ToggleChain()
    {
        bool isActive = chain.activeSelf;
        DisableUpperBodywear();
        chain.SetActive(!isActive);
        currentAccessories["BodywearUpper"] = !isActive ? "ChainImage" : "None";
        AudioSource.PlayClipAtPoint(clickClip, transform.position);
    }
    public void ToggleTutu()
    {
        bool isActive = tutu.activeSelf;
        tutu.SetActive(!tutu.activeSelf);
        currentAccessories["BodywearLower"] = !isActive ? "TutuImage" : "None";
        AudioSource.PlayClipAtPoint(clickClip, transform.position);
    }

    void DisableAllHeadware()
    {
        hat.SetActive(false);
        unicorn.SetActive(false);
        AudioSource.PlayClipAtPoint(clickClip, transform.position);
    }
    void DisableUpperBodywear()
    {
        chain.SetActive(false);
        ribbonBlack.SetActive(false);
        AudioSource.PlayClipAtPoint(clickClip, transform.position);
    }
}
