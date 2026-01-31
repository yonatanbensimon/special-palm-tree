using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    public float playerSpeed = 4f;
    
    private Vector2 moveInput;
    private LightController nearbyLight;
    private BearTrapController nearbyBear;

    private InventoryManager  inventory;

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

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(moveInput.x, moveInput.y, 0);
        transform.position += movement * playerSpeed * Time.deltaTime;
    }

    void Start()
    {
        inventory = GetComponent<InventoryManager>();
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
