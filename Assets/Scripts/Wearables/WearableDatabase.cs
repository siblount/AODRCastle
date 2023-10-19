using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(menuName = "Wearable/Wearable Database")]
public class WearableDatabase : ScriptableObject
{
    /// <summary>
    /// Holds all <see cref="Wearable"/> types.
    /// </summary>
    public static List<Wearable> Wearables => Instance.wearables;
    public static Dictionary<string, Wearable> StringToWearables => Instance.stringToWearables;
    public static WearableDatabase Instance
    {
        get
        {
            if (instance == null) return instance = (WearableDatabase)Resources.Load("WearableDatabase");
            return instance;
        }
    }

    private static WearableDatabase instance;
    [SerializeField] private List<Wearable> wearables;
    private Dictionary<string, Wearable> stringToWearables;

    private void OnEnable()
    {
        instance = this;
        stringToWearables = new(wearables.Count);
        foreach (var wearable in wearables)
            stringToWearables[wearable.Name] = wearable;
    }

    private void OnDisable()
    {
        instance = null;
    }
}
