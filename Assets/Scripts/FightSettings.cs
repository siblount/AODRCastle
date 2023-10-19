using System;
using UnityEngine;
using System.Collections.Generic;
[Serializable]
[CreateAssetMenu(fileName = "Settings/Fight Settings")]
public class FightSettings : ScriptableObject
{
    public static FightSettings Easy => easy == null ? 
        (easy = (FightSettings)Resources.Load("Fight Settings/Easy")) : easy;
    public static FightSettings Medium => medium == null ? 
        (medium = (FightSettings)Resources.Load("Fight Settings/Medium")) : medium;
    public static FightSettings Hard => hard == null ? 
        (hard = (FightSettings)Resources.Load("Fight Settings/Hard")) : hard;
    public static FightSettings Impossible => impossible == null ? 
        (impossible = (FightSettings)Resources.Load("Fight Settings/Impossible")) : impossible;
    public static Dictionary<string, FightSettings> StringToSettings => stringToSetting ??= new()
    {
        { nameof(Easy), Easy },
        { nameof(Medium), Medium },
        { nameof(Hard), Hard },
        { nameof(Impossible), Impossible }
    };
    private static Dictionary<string, FightSettings> stringToSetting;
    private static FightSettings easy, medium, hard, impossible;
    
    [Header("Global Fight Settings: ")]
    [Range(0, 20)] public float GlobalDamageMultiplier = 1;
    [Range(0, 20)] public float GlobalHPMultiplier = 3;
    [Range(0, 255)] public byte MinimumDamage = 0;
    [Range(0, 255)] public byte MaximumDamage = 45;
    [Header("Global User Fight Settings: ")]
    // Use for Easy, Medium, Hard, Mission Impossible Difficulties
    [Range(0, 9)] public float UserDamageMultiplier = 1;
    [Range(0, 3)] public float UserHealthMultiplier = 1;
    [Range(0, 2)] public byte AdditonalArmor = 0;
    [Range(0, 9)] public byte AdditionalAP = 0;
}
