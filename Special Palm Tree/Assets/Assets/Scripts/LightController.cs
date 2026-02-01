using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class LightController : MonoBehaviour
{
    private Light2D targetLight;
    public float brightLight = 5f;
    public bool isExtinguished = false;
    public bool isOn = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetLight = GetComponent<Light2D>();
    }

    public void increaseLight()
    {
        targetLight.intensity = brightLight;
        isOn = true;
    }

    public void Extinguish()
    {
        if (isExtinguished) return;
        if (!isOn) return;
        StartCoroutine(ExtinguishSequence());
    }

    private IEnumerator ExtinguishSequence()
    {
        isExtinguished = true;
        yield return new WaitForSeconds(30f);
        targetLight.intensity = 0.1f;
        isOn = false;
    }
}
