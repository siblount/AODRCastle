using UnityEngine;
using System.Text;
[CreateAssetMenu(menuName ="Wearable/Accident Charm")]
public class AccidentCharmWearable : Wearable
{
    [Range(0, 2)] public float DamageReductionMultiplier = 0.6f;
    
    public override void ApplyWearable(FightData Owner)
    {
        base.ApplyWearable(Owner);
        Fight.CurrentFight.AbilityAttack += CurrentFight_AbilityAttack;
    }

    private void CurrentFight_AbilityAttack(Ability obj)
    {
        if (obj is not FireballAbility && obj is not BasicAbility) return;
        if (obj is BasicAbility basicAbility && basicAbility.FlameAbility && Owner == obj.Owner) return;
            obj.WorkingAttackStats.Damage = (byte)Mathf.FloorToInt(obj.WorkingAttackStats.Damage * DamageReductionMultiplier);
    }

    public override void RemoveWearable()
    {
        base.RemoveWearable();
        Fight.CurrentFight.AbilityAttack -= CurrentFight_AbilityAttack;
    }
}
