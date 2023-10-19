using UnityEngine;
using System;
using System.Collections.Generic;
[Serializable]
public class FightData
{
    public byte Health
    {
        get => health;
        set => health = (byte)Mathf.Clamp(value, 0, byte.MaxValue);
    }
    public byte BaseDamage
    {
        get => baseDamage;
        set => baseDamage = (byte)Mathf.Clamp(value, 0, byte.MaxValue);
    }
    public byte MaxHealth
    {
        get => maxHealth;
        set => maxHealth = (byte)Mathf.Clamp(value, 0, byte.MaxValue);
    }
    public float DamageMultipler
    {
        get => damageMultiplier;
        set => damageMultiplier = Mathf.Max(value, 0);
    }
    public List<Status> ActiveStatuses = new(3);
    public HashSet<Ability> ActiveAbilities = new(5);
    public HashSet<string> AvailableAbilityNames;
    public Dictionary<string, Ability> NameToAvailableAbility;
    public Stats Stats;
    [SerializeField]
    private byte health, baseDamage, maxHealth;
    [SerializeField] private float damageMultiplier = 1;

    public FightData() { }
    public FightData(Stats data, Fight fight)
    {
        Health = (byte)(data.HP * fight.FightSettings.GlobalHPMultiplier);
        MaxHealth = Health;
        Stats = data;
        NameToAvailableAbility = new(data.Abilities.Count);
        data.Abilities.ForEach((a) => NameToAvailableAbility[a.NameRaw] = a);
        AvailableAbilityNames = new(NameToAvailableAbility.Keys);
    }

    public byte Damage
    {
        get => (byte)(Stats.DMG * DamageMultipler *
        Fight.CurrentFight.FightSettings.GlobalDamageMultiplier);
    }
}

[Serializable]
public class UserFightData : FightData
{
    /// <summary>
    /// An event that is invoked when the heat percentage has changed.
    /// First parameter is the old value, second is new value.
    /// </summary>
    public event StatValueChangedHandler HeatPercentageChanged;
    public HashSet<string> AvailableBasicAbilityNames;
    public delegate void StatValueChangedHandler(float oldValue, float newValue);
    public float HeatPercentage
    {
        get => heatPercentage;
        set
        {
            var oldHeatPercentage = heatPercentage;
            var newHeatPercentage = Mathf.Clamp01(value);
            heatPercentage = newHeatPercentage;
            HeatPercentageChanged?.Invoke(oldHeatPercentage, newHeatPercentage);
            FightUI.Instance?.UpdateHeat();
        }
    }
    private float heatPercentage = 0;

    public UserFightData() { }
    public UserFightData(UserStats data, Fight fight) : base(data, fight)
    {
        Health = (byte)(Health * fight.FightSettings.UserHealthMultiplier);
        MaxHealth = Health;
        AvailableBasicAbilityNames = new(data.BasicAbilities.Count);
        foreach (var ab in data.BasicAbilities)
        {
            AvailableBasicAbilityNames.Add(ab.NameRaw);
            NameToAvailableAbility[ab.NameRaw] = ab;
        }
    }
    public new byte Damage { get => (byte)(base.Damage * Fight.CurrentFight.FightSettings.UserDamageMultiplier); }
}

