using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;


public class CharacterController : MonoBehaviour
{
    public float playerSpeed = 4f;
    
    private Vector2 moveInput;
    private LightController nearbyLight;
    private BearTrapController nearbyBear;

    private InventoryManager  inventory;
    private Light2D playerLight;
    private bool isLightOn = true;

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
        if (!value.isPressed) return;

        if (isLightOn)
        {
            playerLight.intensity = 0.1f;
            isLightOn = false;
        } else
        {
            playerLight.intensity = 1.0f;
            isLightOn = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(moveInput.x, moveInput.y, 0);
        transform.position += movement * playerSpeed * Time.deltaTime;
    }

    void Start()
    {
        inventory = GetComponent<InventoryManager>();
        playerLight = GetComponentInChildren<Light2D>();
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
}
