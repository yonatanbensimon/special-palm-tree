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
        StartCoroutine(ChangeLightSequence(true));
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
        yield return new WaitForSeconds(1f);
        TurnOff();
    }

    private void TurnOff()
    {
        StartCoroutine(ChangeLightSequence(false));
    }

    private IEnumerator ChangeLightSequence(bool on)
    {
        float target = on ? brightLight : 0.1f;
        while (!Mathf.Approximately(targetLight.intensity - target, 0.0f))
        {
            targetLight.intensity = Mathf.MoveTowards(targetLight.intensity, target, Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        isOn = on;
    }
}
