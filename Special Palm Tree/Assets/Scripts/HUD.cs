using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDData
{
    public int numBearTraps = 0;
    public int playerHealth = 3;
    public float horseHealth = 1.0f;
    public float playerSanity = 1.0f;

    public HUDData()
    {
        numBearTraps = 0;
        playerHealth = 3;
        horseHealth = 5;
        playerSanity = 1.0f;
    }
}

public class HUD : MonoBehaviour
{
    [SerializeField] Slider sanitySlider;
    [SerializeField] Slider horseHP;
    [SerializeField] TextMeshProUGUI playerHP;
    [SerializeField] TextMeshProUGUI trapCount;

    public static void Refresh()
    {
        Instance.UpdateHUDComponents();
    }

    public void UpdateHUDComponents()
    {
        if (sanitySlider)
        {
            sanitySlider.value = Data.playerSanity;
        }

        if (horseHP)
        {
            horseHP.value = Data.horseHealth;
        }

        if (playerHP)
        {
            playerHP.text = $"x{Data.playerHealth}";
        }

        if (trapCount)
        {
            trapCount.text = $"x{Data.numBearTraps}";
        }
    }

    static HUD _instance;
    static public HUD Instance
    {
        get
        {
            if (_instance == null || _instance.gameObject.name == "FakeGUI")
            {
                _instance = FindAnyObjectByType<HUD>();
                if (_instance == null)
                {
                    Debug.LogError("No GUI Found!");
                    GameObject go = new GameObject("FakeGUI");
                    _instance = go.AddComponent<HUD>();
                }
            }

            return _instance;
        }
    }

    HUDData _data;
    public static HUDData Data 
    {
        get
        {
            if (Instance._data == null) Instance._data = new();
            return Instance._data;
        }
        
        set
        {
            Instance._data = value;
            Instance.UpdateHUDComponents();
        }
    }
}
