using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;


public class HA2CharacterController : MonoBehaviour
{
    public float playerSpeed = 4f;

    private Vector2 moveInput;
    private LightController nearbyLight;
    private BearTrapController nearbyBear;
    [SerializeField] GameObject beartrapPrefab;

    private InventoryManager inventory;
    private Light2D playerLight;
    [SerializeField] private bool isLightOn = true;
    public float brightLight = 1.0f;

    int health;
    float sanity = 1.0f;
    public float sanityLossModifier = 1.0f;
    public float sanityRechargeModifier = 1.0f;
    public float sanityRechargeDelay = 1.0f;
    float sanityRechargeDelayTimer;


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
        if (nearbyLight == null || !nearbyLight.isOn) { return; }
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

        if (!isLightOn)
        {
            sanity -= Time.deltaTime * sanityLossModifier;
            var gd = GUI.Data;
            gd.playerSanity = sanity;
            GUI.Data = gd;
            sanityRechargeDelayTimer = sanityRechargeDelay;
        }
        else if (sanity < 1.0f)
        {
            sanityRechargeDelayTimer -= Time.deltaTime;
            if (sanityRechargeDelay <= 0.0f)
            {
                sanity += Time.deltaTime * sanityRechargeModifier;
                var gd = GUI.Data;
                gd.playerSanity = sanity;
                GUI.Data = gd;
            }
        }
    }

    void Start()
    {
        inventory = GetComponent<InventoryManager>();
        playerLight = GetComponentInChildren<Light2D>();

        CandleMicrophone.OnBlow += OnBlow;
        sanityRechargeDelayTimer = sanityRechargeDelay;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<LightController>(out var light))
        {
            nearbyLight = light;
        } else if (other.TryGetComponent<BearTrapController>(out var bear)){
            nearbyBear = bear;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<LightController>(out var light))
        {
            if (nearbyLight == light)
            {
                nearbyLight = null;
            }
        } else if (other.TryGetComponent<BearTrapController>(out var bear))
        {
            if (nearbyBear == bear)
            {
                nearbyBear = null;
            }
        }
    }

    public bool IsLightOn()
    {
        return isLightOn;
    }

    private IEnumerator ChangeLightSequence(bool on)
    {
        float target = on ? brightLight : 0.1f;
        while (!Mathf.Approximately(playerLight.intensity - target, 0.0f))
        {
            playerLight.intensity = Mathf.MoveTowards(playerLight.intensity, target, Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        isLightOn = on;
    }

    public void TakeDamage()
    {
        health--;
        var gd = GUI.Data;
        gd.playerHealth = health;
        GUI.Data = gd;

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        print("Welp, you died ig");
    }
}
