using UnityEngine;

public class BearTrapController : MonoBehaviour
{
    public void Collect(InventoryManager inventory)
    {
        inventory.AddTrap();
        Destroy(gameObject);
    }
}
