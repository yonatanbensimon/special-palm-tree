using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int bearTrapsCounter = 0;

    public void AddTrap()
    {
        bearTrapsCounter++;
        var gd = HUD.Data;
        gd.numBearTraps = bearTrapsCounter;
        HUD.Data = gd;

    }

    public bool UseTrap()
    {
        if (bearTrapsCounter <= 0) return false;
        
        bearTrapsCounter--;
        var gd = HUD.Data;
        gd.numBearTraps = bearTrapsCounter;
        HUD.Data = gd;
        return true;
    }
}
