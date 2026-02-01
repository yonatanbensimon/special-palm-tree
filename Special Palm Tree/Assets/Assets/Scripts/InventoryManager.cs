using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int bearTrapsCounter = 0;

    public void AddTrap()
    {
        bearTrapsCounter++;
    }

    public bool UseTrap()
    {
        if (bearTrapsCounter <= 0) return false;
        
        bearTrapsCounter--;
        return true;
    }
}
