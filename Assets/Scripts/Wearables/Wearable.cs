using UnityEngine;
using System.Text;
[CreateAssetMenu(menuName ="Wearable/Base")]
public class Wearable : ScriptableObject
{
    /// <summary>
    /// The name of the wearable.
    /// </summary>
    public string Name;
    /// <summary>
    /// Provides the lore of the wearable.
    /// </summary>
    public string Description;
    /// <summary>
    /// Describes the effect of the wearable. For example,
    /// "+2 DMG"
    /// </summary>
    public string Effect => GenerateEffect();
    /// <summary>
    /// The art of the wearable, if any.
    /// </summary>
    public Sprite Art;
    /// <summary>
    /// The owner of the wearable.
    /// </summary>
    public FightData Owner;
    [Header("Wearable Effects")]
    [Range(-10, 10)] public sbyte HP;
    [Range(-10, 10)] public sbyte DMG;
    [Range(-10, 10)] public sbyte AP;
    [Range(-10, 10)] public sbyte Armor;
    private byte lastHP, lastDMG, lastAP, lastArmor;
    public virtual void ApplyWearable(FightData Owner)
    {
        this.Owner = Owner;
        Owner.Stats.CurrentWearable?.RemoveWearable();
        Owner.Stats.CurrentWearable = this;
        ApplyStats();
    }
    public virtual void RemoveWearable()
    {
        Owner.Stats.CurrentWearable = null;
        RemoveStats();
    }


    public virtual string GenerateEffect() => generateEffect().ToString();
    
    protected virtual void ApplyStats()
    {
        lastHP = (byte)Mathf.Max(0, HP);
        lastDMG = (byte)Mathf.Max(DMG, 0);
        lastAP = (byte)Mathf.Max(AP, 0);
        lastArmor = (byte)Mathf.Max(Armor, 0);
        Owner.Stats.HP += lastHP;
        Owner.Stats.DMG += lastDMG;
        Owner.Stats.AP += lastAP;
        Owner.Stats.DEF += lastArmor;
    }

    protected virtual void RemoveStats()
    {
        Owner.Stats.HP -= lastHP;
        Owner.Stats.DMG -= lastDMG;
        Owner.Stats.AP -= lastAP;
        Owner.Stats.DEF -= lastArmor;
    }
    protected virtual StringBuilder generateEffect()
    {
        StringBuilder builder = new(25);
        const string format = "{0} {1}";
        bool commaRequired;
        string e = string.Empty;
        if (commaRequired = HP != 0)
            builder.Append(HP > 0 ? "+" : e).AppendFormat(format, HP, nameof(HP));
        if (DMG != 0)
        {
            if (commaRequired) builder.Append(", ");
            builder.Append(DMG > 0 ? "+" : e).AppendFormat(format, DMG, nameof(DMG));
            commaRequired = true;
        }
        if (Armor != 0)
        {
            if (commaRequired) builder.Append(", ");
            builder.Append(Armor > 0 ? "+" : e).AppendFormat(format, Armor, nameof(Stats.DEF));
            commaRequired = true;
        }
        if (AP != 0)
        {
            if (commaRequired && (commaRequired |= AP != 0)) builder.Append(", ");
            builder.Append(AP > 0 ? "+" : e).AppendFormat(format, AP, nameof(AP));
        }
        return builder;
    }
}
