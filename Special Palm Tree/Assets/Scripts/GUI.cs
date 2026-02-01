using UnityEngine;

public struct GUIData
{
    public int numBearTraps;
    public int playerHealth;
    public int horseHealth;
    public float playerSanity;
}

public class GUI : MonoBehaviour
{
    public void UpdateGUIComponents()
    {

    }

    static GUI _instance;
    static public GUI Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<GUI>();
                if (_instance == null) Debug.LogError("No GUI Found!");
                GameObject go = new GameObject("FakeGUI");
                _instance = go.AddComponent<GUI>();
            }

            return _instance;
        }
    }
    
    GUIData _data;
    public static GUIData Data 
    { 
        get => Instance._data;
        set
        {
            Instance._data = value;
            Instance.UpdateGUIComponents();
        }
    }
}
