using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Statuses/Rage")]
public class RageStatus : Status
{
    public override bool LoreAvailable { get; protected set; } = false;
    [Range(0, 1)] public float UserDamageMultiplier = 0.2f;
    [Range(0, 1)] public float TargetDamageMultiplier = 0.2f;

    public override void ApplyStatus(FightData data)
    {
        base.ApplyStatus(data);
        Owner.DamageMultipler += UserDamageMultiplier;
        Target.DamageMultipler += TargetDamageMultiplier;
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
        Owner.DamageMultipler -= UserDamageMultiplier;
        Target.DamageMultipler -= TargetDamageMultiplier;
        base.RemoveStatus();
    }
}


