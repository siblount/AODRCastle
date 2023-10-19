using UnityEngine;

[CreateAssetMenu(menuName = "Battle Stats/Ability Attack")]
public class AbilityAttackStats : ScriptableObject
{
    [Header("Stats: ")]
    /// <summary>
    /// The base damage stat of the attack.
    /// </summary>
    [Range(1, 10)] public byte Damage = 1;
    /// <summary>
    /// The attack penetration of the attack.
    /// </summary>
    [Range(1, 10)] public byte AttackPenetration = 1;
    /// <summary>
    /// Determines the rate of the heat generation of the attack. 
    /// This is a percentage.
    /// </summary>
    [Range(0, 1)] public float HeatRate = 0.01f;


    /// <summary>
    /// This function is called to reset values to default.
    /// </summary>
    protected void Reset()
    {
        Damage = 1;
        AttackPenetration = 1;
        HeatRate = 0.01f;
    }
}
