using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(menuName = "Statuses/Status Database")]
public class StatusDatabase : ScriptableObject
{

    /// <summary>
    /// Holds all <see cref="Status"/> types.
    /// </summary>
    public static List<Status> Statuses => Instance.statuses;
    public static Dictionary<string, Status> StringToStatus => Instance.stringToStatus;


    public static StatusDatabase Instance
    {
        get
        {
            if (instance == null) return instance = (StatusDatabase)Resources.Load("Status Database");
            return instance;
        }
    }
    private static StatusDatabase instance;
    [SerializeField] private List<Status> statuses;
    private Dictionary<string, Status> stringToStatus;

    private void OnEnable()
    {
        instance = this;
        stringToStatus = new(statuses.Count);
        foreach (var status in statuses)
            stringToStatus[status.Name] = status;
    }

}

