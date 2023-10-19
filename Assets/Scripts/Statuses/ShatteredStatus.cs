using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Statuses/Shattered")]
public class ShatteredStatus : Status
{
    [HideInInspector] private byte lastArmorLost = 0;
    [Range(1,10)]
    [Tooltip("The Armor to reduce when this status is applied onto someone.")]
    public byte ArmorReduction = 1;

    public override bool LoreAvailable { get; protected set; } = false;

    public override void ApplyStatus(FightData data)
    {
        base.ApplyStatus(data);
        lastArmorLost = (byte)Mathf.Max(Owner.Stats.DEF - ArmorReduction, 0);
        Owner.Stats.DEF -= lastArmorLost;
    }

    public override IEnumerator DoStatusLogicPost()
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator DoStatusLogicPre()
    {
        throw new System.NotImplementedException();
    }

    public override void RemoveStatus()
    {
        Owner.Stats.DEF += lastArmorLost;
        base.RemoveStatus();
    }
}

// TODO: Add ability damage multiplier

