using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    public float playerSpeed = 4f;
    
    private Vector2 moveInput;
    private LightController nearbyLight;

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnInteract(InputValue value)
    {
        if (value.isPressed && nearbyLight != null)
        {
            nearbyLight.increaseLight();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(moveInput.x, moveInput.y, 0);
        transform.position += movement * playerSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<LightController>(out var light))
        {
            nearbyLight = light;
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
        }
    }
}
