using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{
    private Light2D targetLight;
    public float brightLight = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetLight = GetComponent<Light2D>();
    }

    public void increaseLight()
    {
        targetLight.intensity = brightLight;
    }
}
