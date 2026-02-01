using System.Collections.Generic;
using UnityEngine;

public class PersistentGameData : MonoBehaviour
{
    public Dictionary<string, string> accessories;

    private static PersistentGameData _instance;
    public static PersistentGameData Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("PersistentGameDataObject");
                DontDestroyOnLoad(go);
                _instance = go.AddComponent<PersistentGameData>();
                _instance.Init();
            }

            return _instance;
        }
    }

    private void Init()
    {
        accessories = new Dictionary<string, string>();
    }
}
