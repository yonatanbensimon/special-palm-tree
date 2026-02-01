using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BearTrapController : MonoBehaviour
{
    public Sprite beartrapUnset;
    public Sprite beartrapUnsetHighlighted;
    public Sprite beartrapSet;
    public Sprite beartrapSetHighlighted;

    [SerializeField] private AudioClip snapClip;

    SpriteRenderer spriteRenderer;

    public bool isSet = false;
    public bool isHighlighted = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Collect(InventoryManager inventory)
    {
        inventory.AddTrap();
        Destroy(gameObject);
    }

    public void SetHighlight(bool highlighted)
    {
        isHighlighted = highlighted;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = isSet ? highlighted ? beartrapSetHighlighted : beartrapSet : highlighted ? beartrapUnsetHighlighted : beartrapUnset;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (isSet && collision.gameObject.TryGetComponent<HorseAI>(out var horse))
        {
            horse.TakeDamage();
            AudioSource.PlayClipAtPoint(snapClip, horse.transform.position);
            isSet = false;
            SetHighlight(isHighlighted);
            // Destroy Beartrap if we want
            Destroy(gameObject);
        }

    }
}
