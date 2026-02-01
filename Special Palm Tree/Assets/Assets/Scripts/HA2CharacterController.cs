using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;


public class HA2CharacterController : MonoBehaviour
{
    [Header("Calibration")]
    public float playerSpeed = 4f;
    public float sanityLossModifier = 1.0f;
    public float sanityRechargeModifier = 1.0f;
    public float sanityRechargeDelay = 1.0f;
    
    private Vector2 moveInput;
    private LightController nearbyLight;
    private BearTrapController nearbyBear;
    [SerializeField] GameObject beartrapPrefab;
    private PlayerSpriteAnimator _psa;

    private InventoryManager inventory;
    private Light2D playerLight;
    [SerializeField] private bool isLightOn = true;
    public float brightLight = 1.5f;

    [SerializeField] AudioClip candleOn;
    [SerializeField] AudioClip candleOff;

    [SerializeField] AudioSource heartbeat;

    int health;
    float sanity = 1.0f;
    
    float sanityRechargeDelayTimer;

    public float invulnerabilityDuration = 1.0f;
    private bool isInvulnerable = false;
    private SpriteRenderer playerSprite;


    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnInteract(InputValue value)
    {
        if (!value.isPressed) return;

        if (nearbyLight != null)
        {
            nearbyLight.increaseLight();
        } else if (nearbyBear != null)
        {
            nearbyBear.Collect(inventory);
            nearbyBear = null;
        }
    }

    public void OnBlow(InputValue value)
    {
        if (!value.isPressed) { return; }
        OnBlow();
    }

    public void OnBlow(float _intensity)
    {
        OnBlow();
    }

    private void OnBlow()
    {
        StartCoroutine(ChangeLightSequence(false));
    }

    public void OnIgnite(InputValue value)
    {
        if (!value.isPressed) { return; }
        StartCoroutine(ChangeLightSequence(true));
    }

    public void OnPlaceTrap(InputValue value)
    {
        if (!value.isPressed) { return; }
        // if (nearbyLight == null || !nearbyLight.isOn) { return; }
        if (inventory.UseTrap())
        {
            Instantiate(beartrapPrefab, transform.position, transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(moveInput.x, moveInput.y, 0);
        transform.position += movement * playerSpeed * Time.deltaTime;
        _psa.Running(!Mathf.Approximately(0, movement.sqrMagnitude));
        if (!Mathf.Approximately(0, movement.y))
        {
            _psa.Front(movement.y < 0);
        }

        if (!Mathf.Approximately(0, movement.x))
        {
            _psa.Flip(movement.x < 0);
        }

        if (!IsInLight())
        {
            sanity -= Time.deltaTime * sanityLossModifier;
            var gd = HUD.Data;
            gd.playerSanity = sanity;
            HUD.Data = gd;
            sanityRechargeDelayTimer = sanityRechargeDelay;
        }
        else if (sanity < 1.0f)
        {
            sanityRechargeDelayTimer -= Time.deltaTime;
            if (sanityRechargeDelayTimer <= 0.0f)
            {
                sanity += Time.deltaTime * sanityRechargeModifier;
                var gd = HUD.Data;
                gd.playerSanity = sanity;
                HUD.Data = gd;
            }
        }

        float volumeT = Mathf.InverseLerp(1.0f, 0.0f, sanity);
        float volume = Mathf.Lerp(0.4f, 1.0f, volumeT);
        heartbeat.volume = volume;
    }

    void Start()
    {
        inventory = GetComponent<InventoryManager>();
        playerLight = GetComponentInChildren<Light2D>();
        _psa = GetComponent<PlayerSpriteAnimator>();

        CandleMicrophone.OnBlow += OnBlow;
        sanityRechargeDelayTimer = sanityRechargeDelay;
        isLightOn = true;
        sanity = 1.0f;
        HUD.Refresh();

        playerSprite = GetComponentInChildren<SpriteRenderer>(); 
        health = 3;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<LightController>(out var light))
        {
            if (nearbyLight && nearbyLight != light) nearbyLight.SetHighlight(false);
            nearbyLight = light;
            nearbyLight.SetHighlight(true);
        } else if (other.TryGetComponent<BearTrapController>(out var bear)){
            if (nearbyBear && nearbyBear != bear) nearbyBear.SetHighlight(false);
            nearbyBear = bear;
            nearbyBear.SetHighlight(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<LightController>(out var light))
        {
            if (nearbyLight == light)
            {
                nearbyLight.SetHighlight(false);
                nearbyLight = null;
            }
        } else if (other.TryGetComponent<BearTrapController>(out var bear))
        {
            if (nearbyBear == bear)
            {
                nearbyBear.SetHighlight(false);
                nearbyBear = null;
            }
        }
    }

    public bool IsLightOn()
    {
        return isLightOn;
    }

    public bool IsInLight()
    {
        return IsLightOn() || nearbyLight?.isOn == true;
    }

    private IEnumerator ChangeLightSequence(bool on)
    {
        float target = on ? brightLight : 0.1f;
        while (!Mathf.Approximately(playerLight.intensity - target, 0.0f))
        {
            playerLight.intensity = Mathf.MoveTowards(playerLight.intensity, target, Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        if (on) AudioSource.PlayClipAtPoint(candleOn, transform.position);
        else AudioSource.PlayClipAtPoint(candleOff, transform.position);
        isLightOn = on;
        _psa.CandleOn(on);
    }

    public void TakeDamage()
    {
        if (isInvulnerable) return;

        health--;
        var gd = HUD.Data;
        gd.playerHealth = health;
        HUD.Data = gd;

        if (health <= 0)
        {
            Die();
        } else
        {
            StartCoroutine(InvulnerabilityRoutine());
        }
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;
        
        float elapsed = 0;
        while (elapsed < invulnerabilityDuration)
        {
            playerSprite.enabled = !playerSprite.enabled;
            
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }

        playerSprite.enabled = true;
        isInvulnerable = false;
    }

    public void Die()
    {
        print("Welp, you died ig");
    }
}
