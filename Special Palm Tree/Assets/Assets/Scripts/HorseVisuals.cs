using UnityEngine;
using System.Collections.Generic;

public class HorseVisuals : MonoBehaviour
{
    public GameObject sideHorse;
    public GameObject upHorse;

    public GameObject sideCorn;
    public GameObject sideHat;
    public GameObject sideBow;
    public GameObject sideIce;
    public GameObject sideTutu;

    public GameObject topCorn;
    public GameObject topHat;
    public GameObject topBow;
    public GameObject topIce;
    public GameObject topTutu;

    // TutuImage
    // UnicornImage
    // ChainImage
    // HatImage

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ApplyAccessories();
    }

    public void ApplyAccessories()
    {
        var equipped = PersistentGameData.Instance.accessories;
        
        bool hasUnicorn = equipped.ContainsValue("UnicornImage");
        sideCorn.SetActive(hasUnicorn);
        topCorn.SetActive(hasUnicorn);

        bool hasHat = equipped.ContainsValue("HatImage");
        sideHat.SetActive(hasHat);
        topHat.SetActive(hasHat);

        bool hasBow = equipped.ContainsValue("BowImage");
        sideBow.SetActive(hasBow);
        topBow.SetActive(hasBow);

        bool hasIce = equipped.ContainsValue("ChainImage");
        sideIce.SetActive(hasIce);
        topIce.SetActive(hasIce);

        bool hasTutu = equipped.ContainsValue("TutuImage");
        sideTutu.SetActive(hasTutu);
        topTutu.SetActive(hasTutu);
        
    }
}
