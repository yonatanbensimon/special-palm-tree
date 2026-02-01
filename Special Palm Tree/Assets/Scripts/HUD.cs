using UnityEngine;

public struct HUDData
{
    public int numBearTraps;
    public int playerHealth;
    public int horseHealth;
    public float playerSanity;
}

public class HUD : MonoBehaviour
{
    public void UpdateHUDComponents()
    {

    }

    static HUD _instance;
    static public HUD Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<HUD>();
                if (_instance == null) Debug.LogError("No GUI Found!");
                GameObject go = new GameObject("FakeGUI");
                _instance = go.AddComponent<HUD>();
            }

            return _instance;
        }
    }
    
    HUDData _data;
    public static HUDData Data 
    { 
        get => Instance._data;
        set
        {
            Instance._data = value;
            Instance.UpdateHUDComponents();
        }
    }
}
