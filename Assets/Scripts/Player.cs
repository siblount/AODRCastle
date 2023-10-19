using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// Holds the player data which can be used for adding wearables, updating stats, etc.
    /// </summary>
    public static UserStats PlayerData;
    /// <summary>
    /// The current available number of skill points available.
    /// </summary>
    public static byte SkillPoints
    {
        get => skillPoints;
        set
        {
            // Only notify if we are adding skill points.
            if (value > skillPoints) SkillsNotifier.Notify();
            skillPoints = value;
        }
    }
    private static Player instance;

    [SerializeField] private static byte skillPoints = 4;

    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Debug.LogWarning("Player was already instanced but another instance attempted to be created. Deleting...");
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerData is null)
        {
            PlayerData = (UserStats) Instantiate(Resources.Load("FightData/UserProduction"));
            InstaniateAbilities();
        }
#if UNITY_EDITOR
        if (GameSettings.Instance == null) new GameObject("Game Settings Object", typeof(GameSettings));
#endif
        //DontDestroyOnLoad(this);
    }

    /// <summary>
    /// Call this function to reset the player's data back to default.
    /// </summary>
    public void Reset() => ResetPlayer();

    public static void ResetPlayer()
    {
        PlayerData = (UserStats)Instantiate(Resources.Load("FightData/UserProduction"));
        InstaniateAbilities();
        SkillPoints = 4;
    }

    static void InstaniateAbilities()
    {
        var arr = new Ability[PlayerData.Abilities.Count];
        var barr = new BasicAbility[PlayerData.BasicAbilities.Count];
        PlayerData.Abilities.CopyTo(arr);
        PlayerData.BasicAbilities.CopyTo(barr);
        PlayerData.Abilities.Clear();
        PlayerData.BasicAbilities.Clear();
        foreach (var ab in arr)
        {
            PlayerData.Abilities.Add(Instantiate(ab));
        }
        foreach (var ab in barr)
        {
            PlayerData.BasicAbilities.Add(Instantiate(ab));
        }
    }
}
