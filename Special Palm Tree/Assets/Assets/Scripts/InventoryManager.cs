using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int bearTrapsCounter = 0;

    public void AddTrap()
    {
        bearTrapsCounter++;
        var gd = GUI.Data;
        gd.numBearTraps = bearTrapsCounter;
        GUI.Data = gd;

    }

    public bool UseTrap()
    {
        if (bearTrapsCounter <= 0) return false;
        
        bearTrapsCounter--;
        var gd = GUI.Data;
        gd.numBearTraps = bearTrapsCounter;
        GUI.Data = gd;
        return true;
    }
}
