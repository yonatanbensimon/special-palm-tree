using System.Collections.Generic;
using UnityEngine;

public class AccessoryManager : MonoBehaviour
{
    [Header("Headwear")]
    public GameObject hat;
    public GameObject unicorn;

    [Header("Bodywear")]
    public GameObject chain;
    public GameObject ribbonBlack;
    public GameObject tutu;
    public Dictionary<string, string> currentAccessories = new Dictionary<string, string>();
    public void ToggleRibbonBlack()
    {
        bool isActive = ribbonBlack.activeSelf;
        DisableAllBodywear();
        ribbonBlack.SetActive(!isActive);
        currentAccessories["Bodywear"] = !isActive ? "RibbonImage" : "None";
    }

    public void ToggleUnicorn()
    {
        bool isActive = unicorn.activeSelf;
        DisableAllHeadware();
        unicorn.SetActive(!isActive);
        currentAccessories["Headwear"] = !isActive ? "UnicornImage" : "None";
    }

    public void ToggleHat()
    {
        bool isActive = hat.activeSelf;
       DisableAllHeadware();
       hat.SetActive(!isActive);
       currentAccessories["Headwear"] = !isActive ? "HatImage" : "None";
    }
    public void ToggleChain()
    {
        bool isActive = chain.activeSelf;
        DisableAllBodywear();
        chain.SetActive(!isActive);
        currentAccessories["Bodywear"] = !isActive ? "ChainImage" : "None";
    }
    public void ToggleTutu()
    {
        bool isActive = tutu.activeSelf;
        tutu.SetActive(!tutu.activeSelf);
        currentAccessories["Bodywear"] = !isActive ? "TutuImage" : "None";
    }

    void DisableAllHeadware()
    {
        hat.SetActive(false);
        unicorn.SetActive(false);
    }
    void DisableAllBodywear()
    {
        chain.SetActive(false);
        ribbonBlack.SetActive(false);
    }
}
