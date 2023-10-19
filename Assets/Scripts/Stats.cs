using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// Holds the stats of the character such as Name, HP, DMG, AP, etc.
/// It also holds available wearables and abilities and it's avatar.
/// </summary>
[CreateAssetMenu(menuName = "Battle Stats/Base")]
public class Stats : ScriptableObject
{
    /// <summary>
    /// The name of the character.
    /// </summary>
    [Tooltip("The name of the character.")]
    public string Name;
    /// <summary>
    /// The maximum health of the character.
    /// </summary>
    [Range(1, 10), Header("Stats: ")] public byte HP = 1;
    /// <summary>
    /// The base damage stat of the character.
    /// </summary>
    [Range(1, 10)] public byte DMG = 1;
    /// <summary>
    /// The attack penetration of the character.
    /// </summary>
    [Range(1, 10)] public byte AP = 1;
    /// <summary>
    /// The armor of the character.
    /// </summary>
    [Range(1, 10)] public byte DEF = 1;
    /// <summary>
    /// The avatar of the character.
    /// </summary>
    public Sprite Avatar;
    /// <summary>
    /// The abilities at the character's disposal.
    /// </summary>
    public List<Ability> Abilities = new(5);
    /// <summary>
    /// The wearable the character is currently wearing.
    /// </summary>
    public Wearable CurrentWearable;
    /// <summary>
    /// The wearables at the character's disposal.
    /// </summary>
    public List<Wearable> Wearables = new(5);
    /// <summary>
    /// The statuses to have initialized at the start of a match.
    /// </summary>
    public List<Status> InitialStatuses = new(0);


    /// <summary>
    /// This function is called to reset values to default.
    /// </summary>
    protected void Reset()
    {
        Name = string.Empty;
        HP = 1;
        DMG = 1;
        AP = 1;
        Abilities.Clear();
        CurrentWearable = null;
        Wearables.Clear();
        InitialStatuses.Clear();
        DEF = 1;
    }
}
