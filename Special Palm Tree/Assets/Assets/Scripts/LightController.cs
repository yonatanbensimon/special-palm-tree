using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class LightController : MonoBehaviour
{
    private Light2D targetLight;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public float brightLight = 1.5f;
    public bool isExtinguished = false;
    public bool isOn = false;

    public Sprite unlitSprite;
    public Sprite unlitHighlightedSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetLight = GetComponent<Light2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        animator.enabled = false;
        spriteRenderer.sprite = unlitSprite;
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

    public void SetHighlight(bool highlighted)
    {
        if (isOn || isExtinguished) return;

        spriteRenderer.sprite = highlighted ? unlitHighlightedSprite : unlitSprite;
    }

    private IEnumerator ExtinguishSequence()
    {
        isExtinguished = true;
        yield return new WaitForSeconds(1f);

        animator.enabled = false;
        spriteRenderer.sprite = unlitSprite;
        
        TurnOff();
    }

    private void TurnOff()
    {
        StartCoroutine(ChangeLightSequence(false));
    }

    private IEnumerator ChangeLightSequence(bool on)
    {
        float target = on ? brightLight : 0.1f;

        if (on)
        {
            animator.enabled = true;
        } else
        {
            animator.enabled = false;
            spriteRenderer.sprite = unlitSprite;
        }

        while (!Mathf.Approximately(targetLight.intensity - target, 0.0f))
        {
            targetLight.intensity = Mathf.MoveTowards(targetLight.intensity, target, Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        isOn = on;
    }
}
