using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enholds all of the abilities created.
/// </summary>
[CreateAssetMenu(menuName = "Abilities/Ability Database")]
public class AbilityDatabase : ScriptableObject
{
    /// <summary>
    /// Holds all <see cref="Ability"/> types including <see cref="BasicAbility"/>.
    /// </summary>
    public static List<Ability> Abilities => Instance.abilities;
    /// <summary>
    /// Holds only abilities of type <see cref="BasicAbility"/>.
    /// </summary>
    public static List<BasicAbility> BasicAbilities => Instance.basicAbilities;
    /// <summary>
    /// Holds only abilities of type <see cref="Ability"/>,
    /// excluding <see cref="BasicAbility"/>.
    /// </summary>
    public static List<Ability> NonBasicAbilities => Instance.nonBasicAbilities;
    /// <summary>
    /// Dictionary holding the string to ability conversions.
    /// </summary>
    public static Dictionary<string, Ability> StringToAbility => Instance.stringToAbility;
    public static AbilityDatabase Instance
    {
        get
        {
            if (instance == null) return instance = (AbilityDatabase) Resources.Load("Ability Database");
            return instance;
        }
    }
    private static AbilityDatabase instance;
    [SerializeField] private List<Ability> abilities;
    private List<Ability> nonBasicAbilities;
    private List<BasicAbility> basicAbilities;
    private Dictionary<string, Ability> stringToAbility;

    private void OnEnable()
    {
        instance = this;
        nonBasicAbilities = new(8);
        basicAbilities = new(2);
        stringToAbility = new(8);
        foreach (var ability in abilities)
        {
            if (ability is BasicAbility basicAbility)
                basicAbilities.Add(basicAbility);
            else
                nonBasicAbilities.Add(ability);
            stringToAbility[ability.NameRaw] = ability; 
        }
    }



}
